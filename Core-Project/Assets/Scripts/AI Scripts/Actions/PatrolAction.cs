using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAction : Action
{
	private List<Transform> points;
	private int destPoint = 0;
	private NavMeshAgent agent;

	//different constructors depending on how many points already existing at
	//time of action creaction and what format they're in.
	public PatrolAction (NavMeshAgent agent)
	{
		this.agent = agent;
		points = new List<Transform>();
	}
	
	public PatrolAction (NavMeshAgent agent, Transform point)
	{
		this.agent = agent;
		points = new List<Transform>();
		points.Add(point);
	}

	public PatrolAction (NavMeshAgent agent, Transform[] points)
	{
		this.agent = agent;
		this.points = new List<Transform>(points);
	}

	public PatrolAction (NavMeshAgent agent, List<Transform> points)
	{
		this.agent = agent;
		this.points = new List<Transform>(points);
	}

	public Transform[] GetPoints ()
	{
		return points.ToArray();
	}

	public void AddPoint (Transform point)
	{
		points.Add(point);
	}

	public void InsertPoint (int position, Transform point)
	{
		points.Insert(position, point);
		
		destPoint += position <= destPoint ? 1 : 0;
	}

	public void RemovePoint (Transform point)
	{
		int position = points.IndexOf(point);
		points.Remove(point);

		destPoint -= position < destPoint ? 1 : 0;
	}

	public void RemovePointAt (int position)
	{
		points.RemoveAt(position);

		destPoint -= position < destPoint ? 1 : 0;
	}

	void GotoNextPoint () {
		// Returns if no points have been set up
		if (points.Count == 0)
			return;

		// Choose the next point in the array as the destination,
		// cycling to the start if necessary.
		destPoint = (destPoint + 1) % points.Count;

		// Set the agent to go to the currently selected destination.
		agent.destination = points[destPoint].position;

		// Disabling auto-braking allows for continuous movement
		// between points (ie, the agent doesn't slow down as it
		// approaches a destination point).
		agent.autoBraking = false;
	}

	void ResumeCurrentPoint ()
	{
		// Returns if no points have been set up
		if (points.Count == 0)
			return;
		
		// Set the agent to go to the currently selected destination.
		agent.destination = points[destPoint].position;

		// Disabling auto-braking allows for continuous movement
		// between points (ie, the agent doesn't slow down as it
		// approaches a destination point).
		agent.autoBraking = false;
	}

	//Overriding methods from Action abstract class.
	public override void StartAction ()
	{
		base.StartAction();

		ResumeCurrentPoint();
	}

	public override void PauseAction () { base.PauseAction(); }

	public override void ResumeAction ()
	{
		base.ResumeAction();

		ResumeCurrentPoint();
	}

	public override void StopAction () { base.StopAction(); }

	public override void ReachedTargetPoint ()
	{
		base.ReachedTargetPoint();

		GotoNextPoint();
	}
}
