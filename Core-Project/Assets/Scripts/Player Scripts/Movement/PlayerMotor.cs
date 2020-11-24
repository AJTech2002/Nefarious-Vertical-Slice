using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerMotor 
{
    [Header("References")]
    public Transform cam;
    public CapsuleCollider col;

    [Header("Player")]
    public float playerSpeed;


    [Header("Physics")]
    public float gravity;
    public LayerMask discludePlayer;
    public int solverIterations;
    public float collisionRadius;

    [Header("Velocity Clamp")]
    public Vector2 maxVelocity;



    public Vector3 returnMovementDirection (float horizontalValue, float verticalValue)
    {
        Vector3 p = cam.transform.TransformDirection
        (
            new Vector3(horizontalValue,
            0,
            verticalValue) * playerSpeed * Time.deltaTime
        );

        p.x = Mathf.Clamp(p.x, -maxVelocity.x, maxVelocity.x);
        p.z = Mathf.Clamp(p.z, -maxVelocity.y, maxVelocity.y);
        p.y = 0;
        return p;
    }

    public Vector3 finalDirection (Vector3 movementDirection)
    {
        return movementDirection + (new Vector3(0,gravity,0)) * Time.deltaTime;
    }

    public void AdjustTransformForCollision (Transform transform, Vector3 expectedDir)
    {
        //Check all nearby colliders (except self)
        Collider[] c = Physics.OverlapSphere(transform.position, collisionRadius, discludePlayer);
        transform.position += expectedDir;
        //Custom Collision Implementation
        foreach (Collider col in c)
        {
            Vector3 penDir = new Vector3();
            float penDist = 0f;
           
            for (int i = 0; i < solverIterations; i++)
            {
                bool d = Physics.ComputePenetration(col, col.transform.position, col.transform.rotation, col, transform.position + expectedDir, transform.rotation, out penDir, out penDist);

                if (d == false) continue;

                transform.position += -penDir.normalized * penDist;
               
            }

        }

        
    }

    public void FixedUpdate(Transform transform)
    {
        //Target velocity from WASD keys
        Vector3 targetVelocity = cam.transform.TransformDirection
            (
                new Vector3(Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical")) * playerSpeed * Time.deltaTime
            );

        //Clamping Velocity
        var velocityChange = targetVelocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocity.x, maxVelocity.x);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocity.y, maxVelocity.y);
        velocityChange.y = 0;

        //Check all nearby colliders (except self)
        Collider[] c = Physics.OverlapSphere(transform.position, collisionRadius, discludePlayer);

        //Custom Collision Implementation
        foreach (Collider col in c)
        {
            Vector3 penDir = new Vector3();
            float penDist = 0f;
            Vector3 newDir = velocityChange;

            for (int i = 0; i < solverIterations; i++)
            {
                bool d = Physics.ComputePenetration(col, col.transform.position, col.transform.rotation, col, transform.position + newDir, transform.rotation, out penDir, out penDist);


                if (d == false) continue;

                transform.position += -penDir.normalized * penDist;
                newDir = -penDir.normalized * penDist;
            }

        }

        //Moves the player towards the desired velocity with the added gravity
        transform.position += (velocityChange + Physics.gravity * Time.deltaTime);//Target velocity from WASD keys
        
    }



}
