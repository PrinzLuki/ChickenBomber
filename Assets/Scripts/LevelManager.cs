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
    [Header("Remaining Settings")]
    [SerializeField] private int remainingBirdPoints;
    [SerializeField] private Vector3 remainingPopUpOffset;
    [Header("Points")]
    [SerializeField] private int currentPoints;
    [SerializeField] private int pointsNeededOneStar;
    [SerializeField] private int pointsNeededTwoStar;
    [SerializeField] private int pointsNeededThreeStar;



    [Header("Actions")]
    [SerializeField] private bool loadNextBird = false;
    [SerializeField] private bool calculatePoints = false;
    public static event Action<Rigidbody> OnReload;
    public static event Action OnWin;
    public static event Action OnLose;

    private void Start()
    {
        CheckLevelProfile();

        GetSpawnTransform();

        GetSpawnedEnemies();

        SpawnBirds();

        DamageableEntity.OnDestroyObject += AddPointsEntity;
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
            amountOfShots = 0;
            StartCoroutine(ToggleGameOver(1f));
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

    private void AddPointsEntity(int points, Vector3 _, GameObject entity)
    {
        currentPoints += points;
        UIManager.Instance.SetPointsUI(currentPoints);
        if (spawnedEnemies.Contains(entity))
        {
            spawnedEnemies.Remove(entity);
            amountOfEnemies -= 1;
        }
    }

    private void AddPointsForRemainingBirds(int points, Vector3 popUpOffset)
    {
        StartCoroutine(SpawnPopUpPointRemaining(points, popUpOffset));
    }

    private IEnumerator SpawnPopUpPointRemaining(int points, Vector3 popUpOffset)
    {
        var pointPopUp = GetComponent<UIObjectSpawner>();
        float pointMultiplier = 1f;
        for (int i = 0; i < spawnedBirds.Count; i++)
        {
            pointPopUp.SpawnPointsObject(points, spawnedBirds[i].transform.position + popUpOffset, spawnedBirds[i]);
            currentPoints += Mathf.FloorToInt(points * pointMultiplier);
            UIManager.Instance.SetPointsUI(currentPoints);
            pointMultiplier += 0.1f;
            yield return new WaitForSeconds(1f);
        }
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

    private IEnumerator ToggleGameOver(float timer)
    {
        InputManager.Instance.enabled = false;

        AddPointsForRemainingBirds(remainingBirdPoints, remainingPopUpOffset);

        yield return new WaitForSeconds(spawnedBirds.Count + timer);

        CalculateOverallPoints();

        if (currentPoints >= pointsNeededOneStar)
        {
            ToggleGameOverVictory();
        }
        else
        {
            ToggleGameOverLose();
        }

    }

    private void ToggleGameOverLose()
    {
        OnLose?.Invoke();
        StartCoroutine(UIManager.Instance.IActivateGameOverUI(1f, false));
        SaveLevelPoints(currentPoints);
    }

    private void ToggleGameOverVictory()
    {
        OnWin?.Invoke();
        StartCoroutine(UIManager.Instance.IActivateGameOverUI(1f, true));
        SaveLevelPoints(currentPoints);
    }

    private void CalculateOverallPoints()
    {
        if (currentPoints >= pointsNeededThreeStar)
        {
            Debug.Log("Three Star");
            UIManager.Instance.SetStars(3);
            SaveLevelFinished(true);
        }
        else if (currentPoints >= pointsNeededTwoStar && currentPoints < pointsNeededThreeStar)
        {
            Debug.Log("Two Star");
            UIManager.Instance.SetStars(2);
            SaveLevelFinished(true);
        }
        else if (currentPoints >= pointsNeededOneStar && currentPoints < pointsNeededTwoStar)
        {
            Debug.Log("One Star");
            UIManager.Instance.SetStars(1);
            SaveLevelFinished(true);
        }
        else
        {
            Debug.Log("No Star");
            UIManager.Instance.SetStars(0);
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
