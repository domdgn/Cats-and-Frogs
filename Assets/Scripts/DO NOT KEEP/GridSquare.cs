using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare : MonoBehaviour
{
    private GameObject spawnedObject;

    public bool HasObject()
    {
        return spawnedObject != null;
    }

    public void SpawnObject(GameObject prefab)
    {
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }

    public void RemoveObject()
    {
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
            spawnedObject = null;
        }
    }
}