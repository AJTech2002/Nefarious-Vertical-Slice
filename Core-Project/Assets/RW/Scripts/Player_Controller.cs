

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{

    /* 
        TODO: This is a prototype class only, it is not to be used during production due to the various bugs such as: 
        # Inability to descend slopes smoothly (it jumps because it looses contact with ground and applies gravity, need snapping)
        # Proper grounding function
        # Snapping to ground without breaking everything else (preferrably done through the character controller move)
        # Slope Angle calculation isn't always right (need a more intense appraoch at this)...
        # Goes up really fast on slopes [Will need an entire rewrite in the future...]
    */


    public float characterSpeed;
    public float jumpSpeed;
    public LayerMask discludePlayer;
    public CharacterController controller;
    public CapsuleCollider col;
    public Transform cam;
    public Transform playerModel;
    public bool snapOnSlopeDescend;

    Vector3 input;

    float verticalForce;

    float initialCharacterSpeed = 0f;

    private void Awake()
    {
        initialCharacterSpeed = characterSpeed;
    }

    private void Update()
    {
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * characterSpeed;
        input = playerModel.TransformDirection(input);
        input.y = 0;

        if (input.magnitude >= 0.2f)
        playerModel.eulerAngles = new Vector3(playerModel.eulerAngles.x, cam.transform.eulerAngles.y, playerModel.eulerAngles.z);



        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
           verticalForce += jumpSpeed ;
        }
      

    }


    RaycastHit hit;
    
    public bool grounded;
    public Vector3 velocity;
    public Vector3 maxVelocityCap;
    public Vector3 minVelocityCap;
    public Vector3 lastPos;
    public float slipRate;
    public float height;
    public float fixSpeed;
    void FixedUpdate()
    {
        Vector3 v = input*Time.fixedDeltaTime;

        float groundAngle = 0f;
        RaycastHit hit;

        (grounded,groundAngle, hit) = IsGrounded();

        if (groundAngle > controller.slopeLimit) grounded = false;

        if (verticalForce > Physics.gravity.y && !grounded)
        {
            verticalForce += Physics.gravity.y * Time.fixedDeltaTime;
        }

        if (grounded && verticalForce < 0f) { 
            verticalForce = 0f;
            //Provide Stick
            
        };


        if (groundAngle < controller.slopeLimit)
        {
            float perc = 1-(groundAngle / controller.slopeLimit);
            characterSpeed = Mathf.Clamp(initialCharacterSpeed * perc,initialCharacterSpeed*0.5f,initialCharacterSpeed);
        }

        v.y += verticalForce * Time.fixedDeltaTime;

        controller.Move(v);

        if (grounded && verticalForce == 0f && snapOnSlopeDescend) {
            if (groundAngle < 10f)
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, hit.point.y + height, transform.position.z), fixSpeed * Time.fixedDeltaTime);
            else
                transform.position = new Vector3(transform.position.x, hit.point.y + height, transform.position.z);
            
        }

        velocity = (transform.position - lastPos) / Time.deltaTime;


        // || (Mathf.Abs(input.magnitude*characterSpeed)>0.1f && velocity.magnitude<=0.05f) OTher Check for Stuckkledd
        if ((velocity.y == 0 && verticalForce != 0))
        {
            Vector3 added = v;
            
            //Check all nearby colliders (except self)
            Collider[] c = Physics.OverlapSphere(transform.position, 20, discludePlayer);

            //Custom Collision Implementation
            foreach (Collider col in c)
            {
                Vector3 penDir = new Vector3();
                float penDist = 0f;
              
                for (int i = 0; i < 2; i++)
                {
                    bool d = Physics.ComputePenetration(col, col.transform.position, col.transform.rotation, this.GetComponent<CapsuleCollider>(), transform.position+added, transform.rotation, out penDir, out penDist);


                    if (d == false) continue;

                    transform.position += -penDir.normalized * penDist;
                    
                }

            }

        }

        lastPos = transform.position;

    }

    public float dist;
    public float radius;
    (bool,float, RaycastHit) IsGrounded()
    {
        Vector3 p1 = transform.position;
        float distanceToObstacle = 0;
        RaycastHit hit;
        // Cast a sphere wrapping character controller 0.1 meter down to check if it hits anything
       
        if (Physics.SphereCast(p1, radius, Vector3.down, out hit))
        {

            distanceToObstacle = hit.distance;
            if (distanceToObstacle < dist)
            {
                return (true, Vector3.Angle(hit.normal,Vector3.up), hit);
            }
            else
            {
                return (false,0, new RaycastHit());
            }
        }
        else
        {
            return (false,0, new RaycastHit());
        }
    }
}
