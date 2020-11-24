/*
 * Copyright (c) 2020 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{

    [Header("Player Options")]
    [SerializeField]
    public float playerSpeed = 10;
    public float maxVelocityChange;
    public int solverIterations = 2; //How many collision iterations it solves for
    public LayerMask discludePlayer;

    [Header("Phys")]
    public Vector3 top;
    public Vector3 bottom;
    public float maxDist;
    public float groundedDist;
    public float stepPred;//Layer mask for player movement

    [Header("Weapon Configuration")]
    public Animator weaponAnimator;
    public Transform aimPoint;
    public Camera camera;
    public FreeCamera cameraController;
    public float fireRate;

    public Image crosshair;
    public AudioSource source;

    private bool aiming = false;
    private float fireTimer = 0f;
    private Rigidbody rBody; //Rigidbody Reference

    [Header("References")]
    public GameObject bullet;
    public Transform playerModel;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        
    }

    //The Update methods deals with the input handling
    private void Update()
    {
    }


    RaycastHit hit;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + top, 0.05f);
        Gizmos.DrawWireSphere(transform.position + bottom, 0.05f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(hit.point, 0.05f);
    }

    //The fixed update is for movement that will be consistent across all devices
    void FixedUpdate()
    {
        CapsuleCollider colC = this.GetComponent<CapsuleCollider>();
        //Target velocity from WASD keys
        Vector3 targetVelocity = camera.transform.TransformDirection
            (
                new Vector3(Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical")) * playerSpeed * Time.deltaTime
            );

        //Clamping Velocity
        var velocityChange = targetVelocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        playerModel.eulerAngles = new Vector3(playerModel.eulerAngles.x, camera.transform.eulerAngles.y, playerModel.eulerAngles.z);

        //Check all nearby colliders (except self)
        Collider[] c = Physics.OverlapSphere(transform.position, 20, discludePlayer);
        Vector3 accumulatedChanges = Vector3.zero;
        //Custom Collision Implementation
        foreach (Collider col in c)
        {
            Vector3 penDir = new Vector3();
            float penDist = 0f;
            Vector3 newDir = velocityChange;
           

            for (int i = 0; i < solverIterations; i++)
            {
                bool d = Physics.ComputePenetration(col, col.transform.position, col.transform.rotation, colC, transform.position + newDir, transform.rotation, out penDir, out penDist);

                if (d == false) continue;

                transform.position += -penDir.normalized * penDist;
                accumulatedChanges += -penDir.normalized * penDist;
                newDir = -penDir.normalized * penDist;
            }

        }

        //Moves the player towards the desired velocity with the added gravity
        Ray ray = new Ray(transform.position + top + velocityChange.normalized * stepPred, Vector3.down);


        if (Physics.Raycast(ray, out hit, maxDist, discludePlayer))
        {
            
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }

        //Grounded Ray
        Ray groundRay = new Ray(transform.position, Vector3.down);
        RaycastHit groundHit;

        bool grounded = false;

        if (Physics.Raycast(groundRay, out groundHit))
        {
            if (groundHit.distance < groundedDist)
            {
                grounded = true;
            }
            else
            {
               
                if (Mathf.Abs(accumulatedChanges.magnitude) <= 0.0001f)
                {
                    velocityChange += Physics.gravity * Time.deltaTime;
                    grounded = false;
                }
            }
        }

        /*
        if (velocityChange != Vector3.zero)
        {
            velocityChange.y = Mathf.Clamp(velocityChange.y,Mathf.Clamp(hit.point.y - (transform.position + bottom).y, -maxVelocityChange, maxVelocityChange),0.1f);
            print("EXEC");
        }
        print(velocityChange.y);
        */

        transform.position += (velocityChange );
        

    }
}
