using UnityEngine;

public class WorldGeneration : MonoBehaviour {

    [SerializeField]
    private GameObject landPrefab;
    [SerializeField]
    private Transform graphics;

    public float radius;

    private int locations = 10;

    private bool[][] occupied;


    private int numberOfIslands = 40;

    private void Start()
    {
        radius = graphics.localScale.x/2;
        occupied = new bool[locations][];
        for (int i = 0; i < occupied.Length; i++)
        {
            occupied[i] = new bool[locations];
        }

        //Not going to use this for now: hand placing islands.
        //GenerateWorld();
    }

    private void GenerateWorld()
    {
        for (int i = 0; i < numberOfIslands; i++)
        {
            float x = Random.Range(0, 360);
            float y = Random.Range(0, 360);

            GameObject newLand = Instantiate(landPrefab, new Vector3(transform.position.x, transform.position.y, radius), Quaternion.identity, transform);
            newLand.transform.RotateAround(transform.position, Vector3.up, x);
            newLand.transform.RotateAround(transform.position, Vector3.right, y);
            newLand.transform.rotation = Quaternion.LookRotation(transform.position - newLand.transform.position);
        }
    }
}
