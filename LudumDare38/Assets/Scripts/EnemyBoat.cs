using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoat : MonoBehaviour {

    private Transform world;
    private bool finishedTurning = false;
    int dir = 0;

    [SerializeField]
    private GameObject smoke1;
    [SerializeField]
    private GameObject smoke2;

    private Health health;
    private void Start()
    {
        world = FindObjectOfType<WorldGeneration>().transform;
        health = GetComponent<Health>();
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, 10f))
        {
            if (finishedTurning)
            {
                finishedTurning = false;
                dir = Random.Range(0, 2);
            }


            Rotate();
        }
        else
        {
            if (!finishedTurning)
            {
                finishedTurning = true;
                Rotate();
            }
            transform.RotateAround(world.transform.position, transform.right, 5f * Time.deltaTime);
        }

        transform.position = (transform.position - world.position).normalized * world.GetComponent<WorldGeneration>().radius;

        //Make the boat perpendicular to the planets surface:
        Vector3 perpendicularToSurface = Vector3.ProjectOnPlane(transform.forward, transform.position - world.position);
        transform.rotation = Quaternion.LookRotation(perpendicularToSurface, transform.up);

        if (health.currentHealth < health.maxHealth / 2)
        {
            smoke1.SetActive(true);
        }
        if (health.currentHealth < health.maxHealth / 4)
        {
            smoke2.SetActive(true);
        }
    }

    private void Rotate()
    {
        if (dir == 0)
        {
            transform.Rotate(Vector3.up, 90f * Time.deltaTime);
        }
        else
        {
            transform.Rotate(Vector3.up, -90f * Time.deltaTime);
        }
    }
}
