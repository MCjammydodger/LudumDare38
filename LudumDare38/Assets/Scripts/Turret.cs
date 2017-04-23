using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    public GameObject missilePrefab;
    [SerializeField]
    protected Transform[] missileSpawns;

    protected float missileStrength;

    protected Transform playerTransform;

    protected float reloadTime = 0.6f;

    protected float timeSinceFiring = 0;

    protected float range = 20f;

    protected virtual void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        timeSinceFiring = reloadTime;
    }

    private void Update()
    {
        if (playerTransform != null && !GameManager.Paused)
        {
            RotateTurret();
            FireMissile();
        }
    }


    protected virtual void RotateTurret()
    {

        if (Vector3.Distance(transform.position, playerTransform.position) <= range)
        {

            transform.LookAt(playerTransform, transform.parent.forward);

        }

    }

    protected virtual void FireMissile()
    {
        if(timeSinceFiring > reloadTime && Vector3.Distance(transform.position, playerTransform.position) <= range)
        {
            foreach (Transform t in missileSpawns)
            {
                GameObject missile = Instantiate(missilePrefab, t.position, t.rotation);
                //missile.transform.localEulerAngles = new Vector3(missile.transform.localEulerAngles.x, missile.transform.localEulerAngles.y, -90);

                missile.GetComponent<Missile>().OwnTurret = transform;
                
            }
            timeSinceFiring = 0;
        }
        timeSinceFiring += Time.deltaTime;
    }
}
