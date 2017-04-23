using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private Slider completionBar;
    [SerializeField]
    private Text gameOverText;
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject resumeButton;
    [SerializeField]
    private GameObject hud;
    [SerializeField]
    private Transform notifications;
    [SerializeField]
    private GameObject notificationPrefab;

    private GameObject playerObject;

    private Health playerHealth;

    private bool startMenuTimer = false;

    private float menuTimer = 0f;

    private void Start()
    {
        playerObject = FindObjectOfType<Boat>().gameObject;
        playerHealth = playerObject.GetComponent<Health>();
        playerHealth.HealthChangedEvent += OnHealthChanged;
        playerHealth.DeathEvent += OnDeath;

        healthBar.maxValue = playerHealth.maxHealth;
        healthBar.value = healthBar.maxValue;

        gameOverText.gameObject.SetActive(false);
        menu.SetActive(false);
        hud.SetActive(false);

        FindObjectOfType<GameManager>().VictoryEvent += OnVictory;
    }

    public void ShowHud(bool show)
    {
        hud.SetActive(show);
    }
    public void InitialiseCompletionBar(int totalIslands)
    {
        completionBar.maxValue = totalIslands;
        completionBar.value = 0;
    }
    public void UpdateCompletionBar(int numUnconquered)
    {
        completionBar.value = completionBar.maxValue - numUnconquered;
    }
    public void OnHealthChanged(float newHealth)
    {
        healthBar.value = newHealth;
    }

    public void OnDeath()
    {
        GameOver("Game Over:\nYou Died");
    }

    public void OnVictory()
    {
        GameOver("Victory:\nYou have conquered the world!");
    }

    public void GameOver(string message)
    {
        hud.SetActive(false);
        gameOverText.text = message;
        gameOverText.gameObject.SetActive(true);
        startMenuTimer = true;
    }
    public void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }

    public void ShowMenu(bool show, bool withResume = false)
    {
        if (withResume)
        {
            resumeButton.SetActive(true);
        }else
        {
            resumeButton.SetActive(false);
        }
        menu.SetActive(show);
    }

    public void NewNotification(string message)
    {
        GameObject newObject = Instantiate(notificationPrefab, notifications, false);
        newObject.GetComponentInChildren<Text>().text = message;
        Destroy(newObject, 4f);
    }
    private void Update()
    {
        // If the game had ended, wait a certain number of seconds before showing the menu.
        if (startMenuTimer)
        {
            menuTimer += Time.deltaTime;
            if(menuTimer >= 2)
            {
                gameOverText.gameObject.SetActive(false);
                ShowMenu(true);
                FindObjectOfType<GameManager>().Pause(true, false);
            }
        }
    }
}
