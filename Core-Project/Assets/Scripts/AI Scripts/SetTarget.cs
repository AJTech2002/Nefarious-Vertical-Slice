using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTarget : MonoBehaviour
{
	private static float DISTANCE = 50f;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, DISTANCE))
			{
				//draw invisible ray cast/vector
				Debug.DrawLine (ray.origin, hit.point);

				transform.position = hit.point;
			}
		}
	}
}
