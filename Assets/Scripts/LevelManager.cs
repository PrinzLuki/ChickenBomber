using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform slingshot;
    [Header("Bird Amount")]
    [SerializeField] private int amountOfBirdsToSpawn;
    [Header("Order Update Settings")]
    private float updateOrderTime = 2f;
    private float updateOrderTimeBetweenBirds = 0.5f;
    [Header("Spawn Settings")]
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private Vector3 spawnLocation;
    [SerializeField] private Quaternion spawnRotation;
    [Header("Birds")]
    [SerializeField] private GameObject currentBirdInSlingshot;
    [SerializeField] private GameObject[] spawnableBirds;
    [SerializeField] private List<GameObject> spawnedBirds;
    [Header("Shots")]
    [SerializeField] private int amountOfShots;
    [Header("Points")]
    [SerializeField] private float currentPoints;

    [Header("Actions")]
    [SerializeField] private bool loadNextBird = false;
    public static event Action<Rigidbody> OnReload;

    private void Start()
    {
        GetSpawnTransform();

        SpawnBirds();

    }

    /// <summary>
    /// Gets the position of the slingshot and sets the spawnLocation
    /// </summary>
    private void GetSpawnTransform()
    {
        spawnLocation = slingshot.position;
    }

    /// <summary>
    /// Spawns birds
    /// </summary>
    private void SpawnBirds()
    {
        SpawnBirdsInOrder();

        amountOfShots = amountOfBirdsToSpawn;

        currentBirdInSlingshot = spawnedBirds[0];
        spawnedBirds.Remove(spawnedBirds[0]);
        OnReload?.Invoke(currentBirdInSlingshot.GetComponent<Rigidbody>());
        StartCoroutine(UpdateOrder());
    }

    /// <summary>
    /// Spawns the birds in a the array order
    /// </summary>
    private void SpawnBirdsInOrder()
    {
        if (spawnableBirds.Length < 0) return;

        spawnedBirds = new List<GameObject>();

        int birdIndex = 0;
        int offsetIndex = 1;

        for (int i = 0; i < amountOfBirdsToSpawn; i++)
        {
            Vector3 newSpawnOffset = new Vector3(spawnOffset.x * offsetIndex, spawnOffset.y, spawnOffset.z);
            offsetIndex++;
            GameObject birdClone = Instantiate(spawnableBirds[birdIndex], spawnLocation + newSpawnOffset, spawnRotation);
            birdClone.GetComponent<BaseBird>().OnReloadBird += NextBird;
            spawnedBirds.Add(birdClone);
            birdIndex++;
            if (birdIndex > spawnableBirds.Length - 1) birdIndex = 0;
        }
    }

    /// <summary>
    /// "Reloads" the next bird
    /// </summary>
    private void NextBird()
    {
        if (spawnedBirds.Count <= 0) currentBirdInSlingshot = null;
        if (currentBirdInSlingshot == null || spawnedBirds.Count <= 0) return;
        currentBirdInSlingshot = spawnedBirds[0];
        spawnedBirds.Remove(spawnedBirds[0]);
        OnReload?.Invoke(currentBirdInSlingshot.GetComponent<Rigidbody>());
        if (spawnedBirds.Count > 0)
            StartCoroutine(UpdateOrder());
    }


    /// <summary>
    /// Updates the order of the birds
    /// </summary>
    private IEnumerator UpdateOrder()
    {
        int offsetIndex = 1;
        for (int i = 0; i < spawnedBirds.Count; i++)
        {
            Vector3 newSpawnOffset = new Vector3(spawnOffset.x * offsetIndex, spawnOffset.y, spawnOffset.z);

            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            float t = 0;

            while (t < updateOrderTimeBetweenBirds)
            {
                t += Time.deltaTime;
                spawnedBirds[i].transform.position = Vector3.Slerp(spawnedBirds[i].transform.position, spawnLocation + newSpawnOffset, t / updateOrderTime);
                yield return wait;
            }
            offsetIndex++;
        }

    }

    private void OnValidate()
    {
        if (loadNextBird)
        {
            loadNextBird = false;
            NextBird();
        }
    }
}
