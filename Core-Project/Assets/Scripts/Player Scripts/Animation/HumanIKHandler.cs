using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanIKHandler : MonoBehaviour
{
    public Animator animator;
    public float snapDistance;
    public float footWidth;
    public float weight;
    public LayerMask discludePlayer;

    private void OnAnimatorIK(int layerIndex)
    {
        FootPlace(HumanBodyBones.LeftFoot, AvatarIKGoal.LeftFoot);
        FootPlace(HumanBodyBones.RightFoot, AvatarIKGoal.RightFoot);
    }

    private void FootPlace(HumanBodyBones foot, AvatarIKGoal goal)
    {
        Transform footPosition = animator.GetBoneTransform(foot);

        Ray ray = new Ray(footPosition.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, discludePlayer))
        {
            if (hit.distance <= snapDistance)
            {
                weight = (hit.distance) / (snapDistance-footWidth);
                animator.SetIKPositionWeight(goal, weight);
                animator.SetIKPosition(goal, new Vector3(hit.point.x, hit.point.y + footWidth, hit.point.z));

                //Rotation

                Vector3 slopeCorrected = -Vector3.Cross(hit.normal, transform.right);

                Quaternion leftFootRot = Quaternion.LookRotation(slopeCorrected, hit.normal);

                animator.SetIKRotationWeight(goal, weight);
                animator.SetIKRotation(goal, leftFootRot);

            }
        }

    }

}
