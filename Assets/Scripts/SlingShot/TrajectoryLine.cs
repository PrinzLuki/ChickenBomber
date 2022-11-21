using System.Collections;
using System.Collections.Generic;
using UnityEditor.Advertisements;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [SerializeField] Vector3 velocityTest;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] int steps;
    

    void Start()
    {
        lineRenderer.positionCount = steps;
    }
    public void SetLineRenderPositions(Rigidbody rb,Vector2 currentPosition,Vector2 velocity)
    {
        lineRenderer.SetPositions(CalculateTrajectoryLinePositions(rb, Vector2.zero, velocity, steps));
    }
    Vector3[] CalculateTrajectoryLinePositions(Rigidbody rb,Vector3 currentPosition,Vector3 velocity,int steps)
    {
        Vector3[] positions = new Vector3[steps];
        lineRenderer.SetPosition(0, transform.position);
        float timeStep = Time.fixedDeltaTime * Physics2D.velocityIterations;
        Vector3 gravityAcceleration = Physics.gravity * rb.mass * timeStep * timeStep;
        Debug.Log(gravityAcceleration);
        float drag = 1f - timeStep * rb.drag;
        Vector3 moveStep = velocity * timeStep;

        for (int i = 1; i < steps; i++)
        {
            moveStep += gravityAcceleration;
            moveStep *= drag;
            currentPosition += moveStep;
            Debug.Log(currentPosition);
            positions[i] = currentPosition;
        }

        return positions;
    }
}
