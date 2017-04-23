using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    [SerializeField]
    private float speed = 2f;

    public float damage = 20f;

    public float range = 1f;
    [SerializeField]
    private GameObject explosionPrefab;

    private Transform world;

    public Transform OwnTurret;
    void Start()
    {
        world = FindObjectOfType<WorldGeneration>().transform;
        Destroy(gameObject, range);
    }
	void Update () {
        transform.RotateAround(world.position, transform.right, speed * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != OwnTurret && other.transform.parent != OwnTurret)
        {
            Health h = other.GetComponent<Health>();
            if(h != null)
            {
                h.TakeDamage(damage);
            }
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(explosion, 5f);
            Destroy(gameObject);
        }
    }
}
