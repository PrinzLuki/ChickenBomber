using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    [SerializeField] Animator[] crowdAnimators;
    [SerializeField] float cheerTime;
    bool isCheering = false;

    void Start()
    {
        DamageableEntity.OnDestroyObject += StartCheering;
    }

    void OnDestroy()
    {
        DamageableEntity.OnDestroyObject -= StartCheering;
    }
    void SetCheeringAnimationState(bool isCheering)
    {
        foreach (var crowdAnimator in crowdAnimators)
        {
            crowdAnimator.SetBool("IsCheering",isCheering);
        }
    }
    void StartCheering(int points,Vector3 _,GameObject go)
    {
        if (go.GetComponent<DamageableEnemyEntity>() != null)
        {
            StartCoroutine(Cheer());
        }
    }

    IEnumerator Cheer()
    {
        //Play sound
        isCheering = true;
        SetCheeringAnimationState(true);

        yield return new WaitForSeconds(cheerTime);

        SetCheeringAnimationState(false);
        isCheering = false;
    }
}
