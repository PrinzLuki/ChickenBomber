using System.Collections;
using System.Collections.Generic;
using UnityEditor.Advertisements;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] int steps;
    

    void Start()
    {
        lineRenderer.positionCount = steps;
        lineRenderer.enabled = false;
    }

    public void SetTrajectoryLineActive(bool active)
    {
        lineRenderer.enabled = active;
    }
    public void SetLineRenderPositions(Rigidbody rb,Vector3 currentPosition,Vector2 velocity)
    {
        lineRenderer.SetPositions(CalculateTrajectoryLinePositions(rb, currentPosition, velocity, steps));
    }
    Vector3[] CalculateTrajectoryLinePositions(Rigidbody rb,Vector3 currentPosition,Vector3 velocity,int steps)
    {
        Vector3[] positions = new Vector3[steps];

        lineRenderer.SetPosition(0,currentPosition);
        positions[0] = currentPosition;

        float timeStep = Time.fixedDeltaTime * Physics2D.velocityIterations;
        Vector3 gravityAcceleration = Physics.gravity * rb.mass * timeStep * timeStep;
        float drag = 1f - timeStep * rb.drag;
        Vector3 moveStep = velocity * timeStep;

        for (int i = 1; i < steps; i++)
        {
            moveStep += gravityAcceleration;
            moveStep *= drag;
            currentPosition += moveStep;
            positions[i] = currentPosition;
        }

        return positions;
    }
}
