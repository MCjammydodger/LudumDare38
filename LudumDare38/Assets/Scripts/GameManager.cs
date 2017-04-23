using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private Texture2D crosshairImage;

    private GameObject playerObject;
    private Land[] islands;

    private int numOfUnconqueredIslands;

    public delegate void OnVictory();
    public event OnVictory VictoryEvent;

    public static bool Paused = false;
    private bool gameOver = false;

    private PlayerUI playerUI;

    private enum PowerupType {Nothing, RangeIncrease, DamageIncrease, HealthRegen };

    [SerializeField]
    private PowerupType[] powerups;

    private int powerupIndex = 0;
    [SerializeField]
    private Transform enemyBoatSpawn;
    [SerializeField]
    private GameObject enemyBoatPrefab;

    private float cameraHeight = 150;

    private bool spawnNewBoat = true;

    private bool introSequence = true;
    private float camSpeed = 100f;
    private Color startColour = Color.black;
    private Color endColour = new Color(83f/255f, 150f/255f, 255f/255f);
    private float camStartDistance;

    private void Start()
    {
        Pause(false, false);
        Paused = true;
        camStartDistance = Camera.main.transform.localPosition.y - cameraHeight;
        Cursor.SetCursor(crosshairImage, new Vector2(crosshairImage.width / 2, crosshairImage.height / 2), CursorMode.Auto);

        playerObject = FindObjectOfType<Boat>().gameObject;
        playerObject.GetComponent<Health>().DeathEvent += OnDeath;
        islands = FindObjectsOfType<Land>();
        numOfUnconqueredIslands = islands.Length;

        foreach(Land i in islands)
        {
            i.LandConqueredEvent += OnIslandConquered;
        }

        playerUI = FindObjectOfType<PlayerUI>();
        playerUI.InitialiseCompletionBar(numOfUnconqueredIslands);

        powerups = new PowerupType[numOfUnconqueredIslands];
        powerups[0] = PowerupType.RangeIncrease;
        for(int p = 1; p < powerups.Length; p++)
        {
            if(p % 9 == 0)
            {
                powerups[p] = PowerupType.RangeIncrease;
            }else if(p % 6 == 0)
            {
                powerups[p] = PowerupType.HealthRegen;
            }else if(p % 3 == 0)
            {
                powerups[p] = PowerupType.DamageIncrease;
            }else
            {
                powerups[p] = PowerupType.Nothing;
            }
        }
    }

    private void SpawnEnemyBoat()
    {
        GameObject enemyBoat = Instantiate(enemyBoatPrefab, enemyBoatSpawn.position, enemyBoatSpawn.rotation);
        enemyBoat.GetComponent<Health>().DeathEvent += OnEnemyBoatDeath;
    }

    public void OnEnemyBoatDeath()
    {
        spawnNewBoat = true;
    }
    public void OnDeath()
    {
        gameOver = true;
        Camera.main.transform.SetParent(null);
    }

    public void OnIslandConquered()
    {
        numOfUnconqueredIslands -= 1;
        playerUI.UpdateCompletionBar(numOfUnconqueredIslands);
        if (numOfUnconqueredIslands == 0)
        {
            gameOver = true;
 
            if (VictoryEvent != null)
            {
                VictoryEvent();
            }
        }
        GivePowerUp();
    }
    
    private void GivePowerUp()
    {
        if (powerupIndex < powerups.Length)
        {
            switch (powerups[powerupIndex])
            {
                case PowerupType.RangeIncrease:
                    playerObject.GetComponent<Boat>().IncreaseBulletRange(0.2f);
                    playerUI.NewNotification("Island conquered! Your missiles now have increased range.");
                    break;
                case PowerupType.DamageIncrease:
                    playerObject.GetComponent<Boat>().IncreaseBulletDamage(5f);
                    playerUI.NewNotification("Island conquered! Your missiles now have increased damage.");
                    break;
                case PowerupType.HealthRegen:
                    playerObject.GetComponent<Health>().TakeDamage(-200f);
                    playerUI.NewNotification("Island conquered! Repairs were made to your ship.");
                    break;
                case PowerupType.Nothing:
                    playerUI.NewNotification("Island conquered!");
                    break;
            }
        }
        powerupIndex++;
    }
    private void Update()
    {
        if (introSequence)
        {
            
            Camera.main.transform.Translate(Vector3.forward * (camSpeed += 20f) * Time.deltaTime);
            float currentDistance = (Camera.main.transform.localPosition.y - cameraHeight);
            float t = 1 - (currentDistance / camStartDistance);
            Camera.main.backgroundColor = Color.Lerp(startColour, endColour, t);
            if(Camera.main.transform.localPosition.y <= cameraHeight)
            {
                Vector3 pos = Camera.main.transform.localPosition;
                pos.y = cameraHeight;
                Camera.main.transform.localPosition = pos;
                introSequence = false;
                playerUI.ShowHud(true);
                Paused = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            Pause(!Paused);
        }

        if (!Paused && spawnNewBoat)
        {
            if(Vector3.Distance(enemyBoatSpawn.position, playerObject.transform.position) > 20f)
            {
                spawnNewBoat = false;
                SpawnEnemyBoat();
            }
        }
    }

    public void Pause(bool pause)
    {
        Pause(pause, true);
    }
    public void Pause(bool pause, bool showMenu)
    {
        if (showMenu)
        {
            playerUI.ShowMenu(pause, true);
        }
        Paused = pause;
        if (pause)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            Time.timeScale = 0;
        }else
        {
            Cursor.SetCursor(crosshairImage, new Vector2(crosshairImage.width/2, crosshairImage.height/2), CursorMode.Auto);
            Time.timeScale = 1;
        }
    }

}
