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
    [Header("Enemies")]
    [SerializeField] private int amountOfEnemies;
    [SerializeField] private List<GameObject> spawnedEnemies;
    [Header("Shots")]
    [SerializeField] private int amountOfShots;
    [Header("Points")]
    [SerializeField] private int currentPoints;
    [SerializeField] private int pointsNeededOneStar;
    [SerializeField] private int pointsNeededTwoStar;
    [SerializeField] private int pointsNeededThreeStar;



    [Header("Actions")]
    [SerializeField] private bool loadNextBird = false;
    [SerializeField] private bool calculatePoints = false;
    public static event Action<Rigidbody> OnReload;

    private void Start()
    {
        CheckLevelProfile();

        GetSpawnTransform();

        GetSpawnedEnemies();

        SpawnBirds();

        DamageableEntity.OnDestroyObject += AddPointsEntity;
        DamageableEnemyEntity.OnDestroyEnemyObject += AddPointsEnemy;
    }

    /// <summary>
    /// Gets the position of the slingshot and sets the spawnLocation
    /// </summary>
    private void GetSpawnTransform()
    {
        spawnLocation = slingshot.position;
    }

    private void GetSpawnedEnemies()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnedEnemies.Add(transform.GetChild(i).gameObject);
        }

        amountOfEnemies = spawnedEnemies.Count;
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
        amountOfShots--;
        if (spawnedBirds.Count <= 0 || IsEnemiesKilled())
        {
            currentBirdInSlingshot = null;
            //No more Birds
            //Check Enemies and Points
            CalculateOverallPoints();
            SaveLevelPoints(currentPoints);
            return;
        }
        currentBirdInSlingshot = spawnedBirds[0];
        spawnedBirds.Remove(spawnedBirds[0]);
        OnReload?.Invoke(currentBirdInSlingshot.GetComponent<Rigidbody>());
        if (spawnedBirds.Count > 0)
        {
            var routine = StartCoroutine(UpdateOrder());
        }
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

    private void AddPointsEntity(int points, Vector3 _)
    {
        currentPoints += points;
    }
    private void AddPointsEnemy(int points, Vector3 _, GameObject enemy)
    {
        currentPoints += points;
        spawnedEnemies.Remove(enemy);
        amountOfEnemies -= 1;
    }

    private bool IsEnemiesKilled()
    {
        if (spawnedEnemies.Count <= 0)
        {
            return true;
            //GET POINTS FOR REMAINING BIRDS
        }
        else
        {
            return false;
        }
    }

    private void CalculateOverallPoints()
    {
        if (currentPoints >= pointsNeededThreeStar)
        {
            //Three Star
            Debug.Log("Three Star");
            SaveLevelFinished(true);
        }
        else if (currentPoints >= pointsNeededTwoStar && currentPoints < pointsNeededThreeStar)
        {
            //Two Star
            Debug.Log("Two Star");
            SaveLevelFinished(true);
        }
        else if (currentPoints >= pointsNeededOneStar && currentPoints < pointsNeededTwoStar)
        {
            //One Star
            Debug.Log("One Star");
            SaveLevelFinished(true);
        }
        else
        {
            //No Star
            Debug.Log("No Star");
            SaveLevelFinished(false);
        }
    }


    #region Saves
    private void SaveLevelPoints(int points)
    {
        if (SaveData.LevelProfile.levelDatas[0].points < points)
            SaveData.LevelProfile.levelDatas[0].points = points;
        SaveLevelProfile();
    }
    private void SaveLevelFinished(bool value)
    {
        SaveData.LevelProfile.levelDatas[0].finished = value;
        SaveLevelProfile();
    }
    private void SaveLevelUnlocked(bool value)
    {
        SaveData.LevelProfile.levelDatas[0 + 1].unlocked = value;
        SaveLevelProfile();
    }

    private void CheckLevelProfile()
    {
        LoadLevelProfile();
        if (!SaveData.LevelProfile.hasSaveData)
        {
            Debug.Log("has no levelprofile saved");
            CreateNewLevelProfile();
            Debug.Log(SaveData.LevelProfile.levelDatas[0].points);
        }
        else
        {
            Debug.Log("has levelprofile saved");
            Debug.Log(SaveData.LevelProfile.levelDatas[0].points);
        }
    }

    /// <summary>
    /// Creates new levelprofile and saves it
    /// </summary>
    private void CreateNewLevelProfile()
    {
        SaveData.LevelProfile.levelDatas = new LevelData[1];

        for (int i = 0; i < SaveData.LevelProfile.levelDatas.Length; i++)
        {
            SaveData.LevelProfile.levelDatas[i] = new LevelData();
            SaveData.LevelProfile.levelDatas[i].finished = false;
            SaveData.LevelProfile.levelDatas[i].unlocked = true;
            SaveData.LevelProfile.levelDatas[i].points = 0;
            SaveData.LevelProfile.levelDatas[i].name = "TEST";
        }

        SaveLevelProfile();

    }

    private void SaveLevelProfile()
    {
        SaveData.LevelProfile.hasSaveData = true;
        SerializationManager.Save("levelData", SaveData.LevelProfile);
    }

    private void LoadLevelProfile()
    {
        SaveData.LevelProfile = (LevelProfile)SerializationManager.Load(Application.persistentDataPath + "/saves/levelData.cutebirds");
    }

    #endregion

    private void OnValidate()
    {
        if (loadNextBird)
        {
            loadNextBird = false;
            NextBird();
        }
        if (calculatePoints)
        {
            calculatePoints = false;
            CalculateOverallPoints();
        }
    }
}
