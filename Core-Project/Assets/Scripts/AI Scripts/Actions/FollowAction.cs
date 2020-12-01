using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowAction : Action
{
	private Transform target;
	private NavMeshAgent agent;

	//different constructors depending on how many points already existing at
	//time of action creaction and what format they're in.
	public FollowAction (NavMeshAgent agent)
	{
		this.agent = agent;
	}
	
	public FollowAction (NavMeshAgent agent, Transform target)
	{
		this.agent = agent;
		this.target = target;
	}

	public Transform GetTarget ()
	{
		return target;
	}

	public void SetTarget (Transform target)
	{
		this.target = target;

		GotoTarget();
	}

	void GotoTarget () {
		// Returns if no points have been set up
		if (target == null)
			return;

		// Set the agent to go to the currently selected destination.
		agent.destination = target.position;

		// Enabling auto-braking means the agent will slow down
		//as it approaches the target.
		agent.autoBraking = true;
	}
	//Overriding methods from Action abstract class.
	public override void StartAction ()
	{
		base.StartAction();

		GotoTarget();
	}

	public override void PauseAction () { base.PauseAction(); }

	public override void ResumeAction ()
	{
		base.ResumeAction();

		GotoTarget();
	}

	public override void StopAction () { base.StopAction(); }

	public override void ReachedTargetPoint () { base.ReachedTargetPoint(); }
}
