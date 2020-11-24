using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerMotor motor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = motor.returnMovementDirection(h, v);
        motor.AdjustTransformForCollision(this.transform, motor.finalDirection(dir));*/

        motor.FixedUpdate(this.transform);
      
    }
}
