

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

    //!!SLOPE ANGLE CALCULATION IS WRONG CONDUCT WITH TWO RAYS!


    public bool characterMotorEnabled = true;

    [Header("Vars")]
    public Vector3 bottomPoint;
    public float characterSpeed;
    public float jumpSpeed;
    public LayerMask discludePlayer;

    [Header("References")]
    public CharacterController controller;
    public Ledge_Controller ledge_controller;
    public CapsuleCollider col;
    public Transform cam;
    public Transform playerModel;

    [Header("Additional Options")]
    public bool snapOnSlopeDescend;
    public float height;
    public float fixSpeed;

    public float gravityScale;
    Vector3 input;
    float verticalForce;
    float initialCharacterSpeed = 0f;
    private RaycastHit hit;
    private bool grounded;
    private Vector3 velocity;
    private Vector3 lastPos;
    private float groundAngle;
    private float initialSphereDist;
    public float maxShit;
    private void Awake()
    {
        initialCharacterSpeed = characterSpeed;
        initialSphereDist = dist;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(playerModel.TransformPoint(bottomPoint),0.01f);
    }

    private void Update()
    {
        if (characterMotorEnabled)
        {
            GainInput();
            //THIS USED TO BE IN FIXED UPDATE AT A TIMESTEP OF 0.001 however this saves performance.
            controller.Move(v);
        }

    }

    public void Enable()
    {
        playerModel.up = Vector3.up;
        characterMotorEnabled = true;
        controller.enabled = true;
    }

    private void GainInput()
    {
            input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * characterSpeed;
            input = playerModel.TransformDirection(input);
            input.y = 0;

            if (input.magnitude >= 0.2f)
                playerModel.eulerAngles = new Vector3(playerModel.eulerAngles.x, cam.transform.eulerAngles.y, playerModel.eulerAngles.z);

            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                verticalForce += jumpSpeed;
            }
    
       
    }

    void GroundCheck()
    {
        (grounded, groundAngle, hit) = IsGrounded();

        if (groundAngle > controller.slopeLimit) grounded = false;
    }

    void VerticalForces()
    {
        if (verticalForce > Physics.gravity.y* gravityScale && !grounded)
        {
            verticalForce += Physics.gravity.y * gravityScale * Time.fixedDeltaTime;
        }

        if (grounded && verticalForce < 0f)
        {
            verticalForce = 0f;
        };

    }

    void SlopeClimbing()
    {
        if (verticalForce == 0 && groundAngle <= controller.slopeLimit)
        {

            float perc = 1 - (groundAngle / controller.slopeLimit);
            characterSpeed = Mathf.Clamp(initialCharacterSpeed * perc, initialCharacterSpeed * 0.4f, initialCharacterSpeed);

        }
        else characterSpeed = initialCharacterSpeed;
       

        if (groundAngle < 5) dist = maxShit;
        else dist = initialSphereDist;
        
    }

    void WallSlipCheck(Vector3 v)
    {
        // || (Mathf.Abs(input.magnitude*characterSpeed)>0.1f && velocity.magnitude<=0.05f) OTher Check for Stuckkledd
        if ((velocity.y == 0 && verticalForce != 0) )
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
                    bool d = Physics.ComputePenetration(col, col.transform.position, col.transform.rotation, this.GetComponent<CapsuleCollider>(), transform.position + added, transform.rotation, out penDir, out penDist);


                    if (d == false) continue;

                    transform.position += -penDir.normalized * penDist;

                }

            }

        }
    }

    void SnappingBehaviour()
    {
        snapOnSlopeDescend = groundAngle >= 5 && groundAngle <= controller.slopeLimit;

        if (grounded && verticalForce == 0f && snapOnSlopeDescend)
        {
            
            transform.position = new Vector3(transform.position.x, hit.point.y +height, transform.position.z);
        }
    }
    

    void LedgeClimbAttempt()
    {
        if (verticalForce != 0 && !grounded)
        {
            Ray ray = new Ray(transform.position, playerModel.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 0.5f, ledge_controller.climbable))
            {
                if (hit.distance < 0.3f)
                {
                    //Climbable Surface
                    if (Vector3.Angle(hit.normal, Vector3.up) >= 60)
                    {
                        //Give over control to ledge controller.
                        characterMotorEnabled = false;
                        ledge_controller.enabled = true;
                        ledge_controller.objectHit = hit;
                    }
                }
            }

        }


        //Final Move

    }

    Vector3 v = Vector3.zero;
    void FixedUpdate()
    {
        if (characterMotorEnabled)
        {
            v = input * Time.fixedDeltaTime;

            GroundCheck();

            VerticalForces();

            SlopeClimbing();

            v.y += verticalForce * Time.fixedDeltaTime;


            LedgeClimbAttempt();



            WallSlipCheck(v);

            SnappingBehaviour();

            velocity = (transform.position - lastPos) / Time.fixedDeltaTime;



            lastPos = transform.position;

        }
    }

   
    public float dist;
    public float radius;
    public (bool,float, RaycastHit) IsGrounded()
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
                return (false, Vector3.Angle(hit.normal, Vector3.up), hit);
            }
        }
        else
        {
            return (false,0, new RaycastHit());
        }
    }
}
