using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject textObjPrefab;
    [SerializeField] Vector3 popUpTargetScale;
    [SerializeField] float popUpScaleSpeed;
    [SerializeField] float popUpDestroyDelay;

    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        DamageAbleObject.OnDestroyObject += SpawnPointsObject;
    }
    void OnDestroy()
    {
        DamageAbleObject.OnDestroyObject -= SpawnPointsObject;
    }
    public void SpawnPointsObject(int points,Vector3 position)
    {
        var instance = Instantiate(textObjPrefab, position, Quaternion.identity);

        SetRotationTowardsCamera(instance.transform);
        SetPointsText(instance.GetComponentInChildren<TMP_Text>(),points.ToString());

        var routine = StartCoroutine(ScaleObject(instance.transform));
    }

    void SetPointsText(TMP_Text text,string points)
    {
        text.text = points;
    }
    void SetRotationTowardsCamera(Transform transformToTorient)
    {
        var direction = (mainCamera.transform.position - transformToTorient.position).normalized;

        transformToTorient.forward = new Vector3(0,0,direction.z);
    }
    IEnumerator ScaleObject(Transform objectToScale)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        while (objectToScale.localScale != popUpTargetScale)
        {
            objectToScale.localScale = Vector3.MoveTowards(objectToScale.localScale,popUpTargetScale,popUpScaleSpeed);
            yield return wait;
        }

        Destroy(objectToScale.gameObject,popUpDestroyDelay);
       
    }
    
}
