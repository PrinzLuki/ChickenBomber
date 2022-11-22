using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private Transform slingshot;
    [SerializeField] private int amountOfBirdsToSpawn;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private Vector3 spawnLocation;
    [SerializeField] private Quaternion spawnRotation;
    [SerializeField] private GameObject[] spawnableBirds;
    [SerializeField] private GameObject[] birds;
    //[SerializeField] private GameObject[] spawnableEnemies;


    private void Start()
    {
        GetSpawnTransform();

        SpawnBirds();
    }

    private void GetSpawnTransform()
    {
        spawnLocation = slingshot.position;
    }

    private void SpawnBirds()
    {
        if (spawnableBirds.Length < 0) return;

        birds = new GameObject[amountOfBirdsToSpawn];

        for (int i = 0; i < birds.Length; i++)
        {
            spawnOffset = new Vector3(spawnOffset.x, spawnOffset.y, spawnOffset.z);
            birds[i] = spawnableBirds[Random.Range(0, spawnableBirds.Length)];
            Instantiate(birds[i], spawnLocation + spawnOffset, spawnRotation);
        }
    }
}
