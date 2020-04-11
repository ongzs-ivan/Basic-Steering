using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnPrefab;
    public Transform spawnParent;

    public float spawnRadius;

    public int spawnCount;

    private void Start()
    {
        GameObject temp;
        for (int i = 0; i < spawnCount; i++)
        {
            temp = Instantiate(spawnPrefab, this.transform.position + Random.insideUnitSphere * spawnRadius, Random.rotation);
            temp.transform.parent = spawnParent;
        }
    }
}
