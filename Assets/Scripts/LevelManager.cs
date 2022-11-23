using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform slingshot;
    [Header("Bird Amount")]
    [SerializeField] private int amountOfBirdsToSpawn;
    [Header("Spawn Settings")]
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private Vector3 spawnLocation;
    [SerializeField] private Quaternion spawnRotation;
    [Header("Birds")]
    [SerializeField] private int currentBirdIndex = 0;
    [SerializeField] private GameObject currentBirdInSlingshot;
    [SerializeField] private GameObject[] spawnableBirds;
    [SerializeField] private GameObject[] spawnedBirds;
    [Header("Shots")]
    [SerializeField] private int amountOfShots;
    [Header("Points")]
    [SerializeField] private float currentPoints;


    [Header("Other Settings")]
    private int offsetIndex = 1;

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

        amountOfShots = spawnedBirds.Length;

        currentBirdIndex = 0;

        currentBirdInSlingshot = spawnedBirds[currentBirdIndex];

        OnReload?.Invoke(currentBirdInSlingshot.GetComponent<Rigidbody>());
    }

    /// <summary>
    /// Spawns the birds in a the array order
    /// </summary>
    private void SpawnBirdsInOrder()
    {
        if (spawnableBirds.Length < 0) return;

        spawnedBirds = new GameObject[amountOfBirdsToSpawn];

        int birdIndex = 0;

        for (int i = 0; i < spawnedBirds.Length; i++)
        {
            Vector3 newSpawnOffset = new Vector3(spawnOffset.x * offsetIndex, spawnOffset.y, spawnOffset.z);
            offsetIndex++;
            GameObject birdClone = Instantiate(spawnableBirds[birdIndex], spawnLocation + newSpawnOffset, spawnRotation);
            birdClone.GetComponent<BaseBird>().OnDeactivationBird += NextBird;
            //birdClone.GetComponent<BaseBird>().OnDestroyBird += RemoveBirdFromArray;
            spawnedBirds[i] = birdClone;
            birdIndex++;
            if (birdIndex > spawnableBirds.Length - 1) birdIndex = 0;
        }
    }

    /// <summary>
    /// "Reloads" the next bird
    /// </summary>
    private void NextBird()
    {
        currentBirdIndex++;
        if (currentBirdIndex > spawnedBirds.Length - 1)
        {
            currentBirdIndex = spawnedBirds.Length - 1;
            Debug.Log("No more birds");
            return;
        }
        currentBirdInSlingshot = spawnedBirds[currentBirdIndex];
        OnReload?.Invoke(currentBirdInSlingshot.GetComponent<Rigidbody>());
        //currentBirdInSlingshot.transform.position = spawnLocation;
        UpdateOrder();
    }

    /// <summary>
    /// Updates the order of the birds
    /// </summary>
    private void UpdateOrder()
    {
        int nextBirdsIndex = currentBirdIndex + 1;

        offsetIndex = 1;

        for (int i = nextBirdsIndex; i < spawnedBirds.Length; i++)
        {
            if (spawnedBirds[i] == null) continue;

            Vector3 newSpawnOffset = new Vector3(spawnOffset.x * offsetIndex, spawnOffset.y, spawnOffset.z);
            spawnedBirds[i].transform.position = spawnLocation + newSpawnOffset;
            offsetIndex++;
        }

    }

    //private void RemoveBirdFromArray()
    //{
    //}

    private void OnValidate()
    {
        if (loadNextBird)
        {
            loadNextBird = false;
            NextBird();
        }
    }
}
