using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSound : MonoBehaviour
{
    [SerializeField] private GameObject spawnedSound;

    void Start()
    {
        Instantiate(spawnedSound);
    }

}
