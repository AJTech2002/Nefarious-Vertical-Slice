using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPCAI.Actions
{
	public class FollowAction : Action
	{
		public float StoppingDistance
		{
			get { return stoppingDistance;  }
			set { stoppingDistance = value; }
		}

		private float stoppingDistance = 0.8f;

		private Transform target;

		//different constructors depending on how many points already existing at
		//time of action creaction and what format they're in.
		public FollowAction (NavMeshAgent agent)
		{
			this.agent = agent;

			updateRequired = true;
		}
		
		public FollowAction (NavMeshAgent agent, Transform target)
		{
			this.agent = agent;
			this.target = target;

			updateRequired = true;
		}

		public FollowAction (NavMeshAgent agent, Transform target, float stoppingDistance)
		{
			this.agent = agent;
			this.target = target;
			this.stoppingDistance = stoppingDistance;

			updateRequired = true;
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

		void GotoTarget ()
		{
			// Returns if no points have been set up
			if (target == null)
				return;

			// Set the agent to go to the currently selected destination.
			agent.destination = target.position;

			// Enabling auto-braking means the agent will slow down
			//as it approaches the target.
			agent.autoBraking = true;
			//The agent will stop within StoppingDistance of the target.
			agent.stoppingDistance = StoppingDistance;
		}

		//Overriding methods from Action abstract class.
		public override void StartAction ()
		{
			base.StartAction();

			GotoTarget();
		}

		public override void UpdateAction ()
		{
			base.UpdateAction();

			GotoTarget();
		}

		public override void ResumeAction ()
		{
			base.ResumeAction();

			GotoTarget();
		}
	}
}