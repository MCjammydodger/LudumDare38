using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour {

    [SerializeField]
    private Material enemyControlledMat;
    [SerializeField]
    private Material conqueredMat;
    [SerializeField]
    private MeshRenderer[] islandRenderers;

    private Health[] destroyableObjects;
    private int numOfDestroyableObjects;

    public delegate void OnConquered();
    public event OnConquered LandConqueredEvent;

    private void Start()
    {
        destroyableObjects = transform.GetComponentsInChildren<Health>();
        numOfDestroyableObjects = destroyableObjects.Length;
        foreach(Health h in destroyableObjects)
        {
            h.DeathEvent += OnDestroy;
        }
        foreach(MeshRenderer r in islandRenderers)
        {
            ChangeLandColour(enemyControlledMat);
        }
    }

    public void OnDestroy()
    {
        numOfDestroyableObjects -= 1;
        if (numOfDestroyableObjects == 0)
        {
            if (LandConqueredEvent != null)
            {
                LandConqueredEvent();
                ChangeLandColour(conqueredMat);
            }
        }
    }

    private void ChangeLandColour(Material mat)
    {
        foreach(MeshRenderer r in islandRenderers)
        {
            r.material = mat;
        }
    }

}
