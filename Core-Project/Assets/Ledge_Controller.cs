using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge_Controller : MonoBehaviour
{
    [Header("Climable Layer")]
    public LayerMask climbable;

    [Header("Detection Points")]
    public Vector3 top;
    public Vector3 mid;
    public Vector3 bottom;

    public float ledgeClimbSpeed;

    [Header("References")]
    public RaycastHit objectHit;
    public Player_Controller controller;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position+ controller.playerModel.TransformDirection(top), 0.1f);
        Gizmos.DrawWireSphere(transform.position + controller.playerModel.TransformDirection(bottom), 0.1f);
        Gizmos.DrawWireSphere(transform.position + controller.playerModel.TransformDirection(mid), 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectCase, 0.1f);
    
    }

    private Vector3 detectCase;

    private RaycastHit surface()
    {
        Ray ray = new Ray(transform.position, controller.playerModel.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, climbable))
        {

            
            controller.playerModel.forward = -objectHit.normal;
            return hit;
        }

        return objectHit;
    }

    //Top & Bottom Edge Case or Comes off Side (Don't handle corner and transitions right now).
    private void DetectEnd()
    {
        //Top RayCast (for top case)...

        Ray bottomRay = new Ray(transform.position + controller.playerModel.TransformDirection(bottom), -controller.playerModel.up);
        RaycastHit hitH;
        if (Physics.Raycast(bottomRay, out hitH, 1f))
        {
            Disable();
            return;
        }

        Ray topRay = new Ray(transform.position+ controller.playerModel.TransformDirection(top), controller.playerModel.forward);
        RaycastHit hit = new RaycastHit();
    
        if (!Physics.Raycast(topRay, out hit, 0.5f, climbable))
        {
            

            topRay = new Ray(transform.position + top + controller.playerModel.forward * 0.5f, - objectHit.transform.up);
           
            if (Physics.Raycast(topRay, out hit, 20, climbable))
            {
                if (hit.transform == objectHit.transform)
                {
                    Debug.DrawRay(topRay.origin, topRay.direction, Color.yellow, 0.1f);
                    detectCase = hit.point;

                    transform.position = new Vector3(hit.point.x, hit.point.y + controller.height, hit.point.z);
                    Disable();
                    return;
                }
            }

        }

        Ray midRay = new Ray(transform.position, controller.playerModel.forward);
        RaycastHit midHit;

        if (!Physics.Raycast(topRay, out hit, 0.3f, climbable) || hit.transform != objectHit.transform)
        {
            Disable();
        }

    }


    private Vector3 InputV()
    {
        Vector3 input;
        input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * ledgeClimbSpeed;
        controller.playerModel.forward = -objectHit.normal;
        input = controller.playerModel.transform.TransformDirection(input);
        //input.z = 0;

        return input;
    }

    private void CollisionCheck(Vector3 v)
    {
            Vector3 added = v;

            //Check all nearby colliders (except self)
            Collider[] c = Physics.OverlapSphere(transform.position, 20, controller.discludePlayer);

            //Custom Collision Implementation
            foreach (Collider col in c)
            {
                Vector3 penDir = new Vector3();
                float penDist = 0f;

                for (int i = 0; i < 2; i++)
                {
                    bool d = Physics.ComputePenetration(col, col.transform.position, col.transform.rotation, this.GetComponent<CapsuleCollider>(), transform.position + added, transform.rotation, out penDir, out penDist);


                    if (d == false) continue;

                Vector3 moveSet = -penDir.normalized * penDist;
                moveSet = controller.playerModel.TransformDirection(moveSet);
                moveSet.z = 0;
                moveSet = controller.playerModel.InverseTransformDirection(moveSet);

                    transform.position += moveSet;

                }

            }

    }

    private void OnEnable()
    {
        controller = this.GetComponent<Player_Controller>();
        controller.controller.enabled = false;

    }

    private void FixedUpdate()
    {
        objectHit = surface();
        transform.position += InputV()*Time.fixedDeltaTime;

        DetectEnd();
    }

    private void LateUpdate()
    {
        CollisionCheck(InputV() * Time.fixedDeltaTime);
    }

    private void Disable()
    {
        controller.Enable();
        this.enabled = false;
    }
}
