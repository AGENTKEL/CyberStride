using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawn : MonoBehaviour
{
    public GameObject shipSound;
    CameraController cameraShake;
    public float minSpawnTime = 10f;
    public float maxSpawnTime = 30f;
    public float minStartSpawnTime = 10f;
    public float maxStartSpawnTime = 30f;

    private void Awake()
    {
        cameraShake = Camera.main.GetComponent<CameraController>();
    }

    private void Start()
    {
        StartDisableShip();
    }

    private void Update()
    {

    }

    public void EnableShip()
    {
        gameObject.SetActive(true);
        cameraShake.StartShaking();
        Instantiate(shipSound);
        Invoke("DisableShip", 0.8f);
    }
    public void DisableShip()
    {
        gameObject.SetActive(false);
        cameraShake.StopShaking();
        Invoke("EnableShip", Random.Range(minSpawnTime, maxSpawnTime));
    }

    public void StartDisableShip()
    {
        gameObject.SetActive(false);
        Invoke("EnableStartShip", Random.Range(minStartSpawnTime, maxStartSpawnTime));
    }

    public void EnableStartShip()
    {
        gameObject.SetActive(true);
        cameraShake.StartShaking();
        Instantiate(shipSound);
        Invoke("DisableShip", 0.8f);
    }
}
