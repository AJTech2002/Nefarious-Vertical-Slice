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
using System.Collections.Generic;
using UnityEngine;

//This script is provided by the Unity Game Engine for free use

namespace UnityEngine.Rendering
{

    [ExecuteAlways]
    public class FreeCamera : MonoBehaviour
    {
     
        public float m_LookSpeedController = 120f;
        public float m_LookSpeedMouse = 10.0f;
        public float m_MoveSpeed = 10.0f;
        public float m_MoveSpeedIncrement = 2.5f;
        public float m_Turbo = 10.0f;

        private static string kMouseX = "Mouse X";
        private static string kMouseY = "Mouse Y";

        private static string kVertical = "Vertical";
        private static string kHorizontal = "Horizontal";

        private static string kSpeedAxis = "Mouse ScrollWheel";

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.L))
            {
                if (Cursor.visible == true)
                {
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = false;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }

            float inputRotateAxisX = 0.0f;
            float inputRotateAxisY = 0.0f;
            
            inputRotateAxisX = Input.GetAxis(kMouseX) * m_LookSpeedMouse;
            inputRotateAxisY = Input.GetAxis(kMouseY) * m_LookSpeedMouse;
        
           
            float inputChangeSpeed = Input.GetAxis(kSpeedAxis);
            if (inputChangeSpeed != 0.0f)
            {
                m_MoveSpeed += inputChangeSpeed * m_MoveSpeedIncrement;
                if (m_MoveSpeed < m_MoveSpeedIncrement) m_MoveSpeed = m_MoveSpeedIncrement;
            }

            float inputVertical = Input.GetAxis(kVertical);
            float inputHorizontal = Input.GetAxis(kHorizontal);


            bool moved = inputRotateAxisX != 0.0f || inputRotateAxisY != 0.0f || inputVertical != 0.0f || inputHorizontal != 0.0f;
            if (moved)
            {
                float rotationX = transform.localEulerAngles.x;
                float newRotationY = transform.localEulerAngles.y + inputRotateAxisX;

                // Weird clamping code due to weird Euler angle mapping...
                float newRotationX = (rotationX - inputRotateAxisY);
                if (rotationX <= 90.0f && newRotationX >= 0.0f)
                    newRotationX = Mathf.Clamp(newRotationX, 0.0f, 90.0f);
                if (rotationX >= 270.0f)
                    newRotationX = Mathf.Clamp(newRotationX, 270.0f, 360.0f);

                transform.localRotation = Quaternion.Euler(newRotationX, newRotationY, transform.localEulerAngles.z);

                float moveSpeed = Time.deltaTime * m_MoveSpeed;
                if (Input.GetMouseButton(1))
                    moveSpeed *= Input.GetKey(KeyCode.LeftShift) ? m_Turbo : 1.0f;
                else
                    moveSpeed *= Input.GetAxis("Fire1") > 0.0f ? m_Turbo : 1.0f;

                Vector3 speculativeDirection = ((transform.forward * moveSpeed * inputVertical)) + (transform.right * moveSpeed * inputHorizontal) ;
                RaycastHit hit;

                //Prevent collision with walls
                if (Physics.SphereCast(transform.position,1,speculativeDirection,out hit, 10))
                {
                    if (hit.distance < 0.251f) return;
                }

                transform.position += transform.forward * moveSpeed * inputVertical;
                transform.position += transform.right * moveSpeed * inputHorizontal;
              
            }
        }
    }
}
