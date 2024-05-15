using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    public GameObject ship;
    public Transform shipSpawnPos;
    public float minSpawnTime = 10f;
    public float maxSpawnTime = 30f;

    private void Awake()
    {
    }

    private void Start()
    {
        Invoke("SpawnShip", Random.Range(minSpawnTime, maxSpawnTime));
    }

    private void Update()
    {
        
    }

    public void SpawnShip()
    {
        Instantiate(ship, shipSpawnPos);
        Invoke("SpawnShip", Random.Range(minSpawnTime, maxSpawnTime));
    }
}
