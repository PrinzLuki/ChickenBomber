using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotAttrap : MonoBehaviour
{
    [SerializeField] GameObject bird;
    public Vector3 offsetDirection;
    public float boost = 10f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BoostInDirection();
            Destroy(gameObject);
        }
    }

    void BoostInDirection()
    {
        Rigidbody birdRb = bird.GetComponent<BaseBird>().GetRb();
        birdRb.AddForce(offsetDirection * boost, ForceMode.Impulse);
        bird.GetComponent<BaseBird>().EnableAbility();

        //StartCoroutine(EnableAbility());
    }

    //IEnumerator EnableAbility()
    //{
    //    yield return new WaitForSeconds(0.2f);
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(this.transform.position, offsetDirection);
    }

}
