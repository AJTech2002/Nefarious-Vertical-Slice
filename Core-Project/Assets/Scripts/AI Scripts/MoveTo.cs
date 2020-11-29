using UnityEngine;
using UnityEngine.AI;
	
public class MoveTo : MonoBehaviour {
	
	public Transform goal;

	private NavMeshAgent agent;
	
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		agent.destination = goal.position; 
	}

	void Update ()
	{
		if (Time.frameCount % 200 == 0)
			agent.destination = goal.position;
	}
}