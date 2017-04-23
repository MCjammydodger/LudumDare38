using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour {

    private Transform world;
    private float speed = 10f;
    private float rotateSpeed = 50f;
    private Vector3 prevPos;
    private Quaternion prevRot;
    private BoatTurret[] turrets;

    [SerializeField]
    private GameObject smoke1;
    [SerializeField]
    private GameObject smoke2;

    private Health health;
    private void Start()
    {
        world = FindObjectOfType<WorldGeneration>().transform;
        health = GetComponent<Health>();
        turrets = transform.GetComponentsInChildren<BoatTurret>();
    }
	private void Update()
    {
        if (!GameManager.Paused)
        {
            Movement();
        }

        if (health.currentHealth < health.maxHealth /2)
        {
            smoke1.SetActive(true);
        }else
        {
            smoke1.SetActive(false);

        }
        if (health.currentHealth < health.maxHealth / 4)
        {
            smoke2.SetActive(true);
        }else
        {
            smoke2.SetActive(false);

        }
    }

    private void Movement()
    {
        //Move forward and back:
        transform.RotateAround(world.transform.position, transform.right, Input.GetAxis("Vertical") * speed * Time.deltaTime);
        //Rotate left and right:
        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime);
        //Make the boat "stick" to the planets surface:
        transform.position = (transform.position - world.position).normalized * world.GetComponent<WorldGeneration>().radius;
        //Make the boat perpendicular to the planets surface:
        Vector3 perpendicularToSurface = Vector3.ProjectOnPlane(transform.forward, transform.position - world.position);
        transform.rotation = Quaternion.LookRotation(perpendicularToSurface, transform.up);
    }

    public void IncreaseBulletRange(float newRange)
    {
        foreach(BoatTurret t in turrets)
        {
            t.missileRange += newRange;
        }
    }

    public void IncreaseBulletDamage(float newDamage)
    {
        foreach(BoatTurret t in turrets)
        {
            t.missileDamage += newDamage;
        }
    }

}
