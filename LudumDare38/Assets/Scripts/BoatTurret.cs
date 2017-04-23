using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatTurret : Turret {

    public float missileRange;
    public float missileDamage;
    protected override void Start()
    {
        base.Start();
        missileRange = missilePrefab.GetComponent<Missile>().range;
        missileDamage = missilePrefab.GetComponent<Missile>().damage;
    }
    protected override void RotateTurret()
    {
        if (transform.root.GetComponent<EnemyBoat>())
        {
            base.RotateTurret();
        }
        else
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.localPosition.y));
            transform.LookAt(mousePos);
        }
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

    }

    protected override void FireMissile()
    {
        if (transform.root.GetComponent<EnemyBoat>())
        {
            if (timeSinceFiring > reloadTime && Vector3.Distance(transform.position, playerTransform.position) <= range)
            {
                Fire();
                timeSinceFiring = 0;
            }
            timeSinceFiring += Time.deltaTime;
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    private void Fire()
    {
        foreach (Transform t in missileSpawns)
        {
            GameObject missile = Instantiate(missilePrefab, t.position, transform.rotation);
            missile.GetComponent<Missile>().range = missileRange;
            missile.GetComponent<Missile>().damage = missileDamage;
            missile.GetComponent<Missile>().OwnTurret = transform.root;
        }
    }
}
