using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPCAI.Actions;

namespace NPCAI
{
	public class BaseAI : MonoBehaviour
	{
		public float Health
		{
			get { return health; }
			set {health = value; }
		}

		private float health = 100;

		protected NavMeshAgent agent;

		private Action currentAction;

		void Awake ()
		{
			agent = GetComponent<NavMeshAgent>();
		}

		public void SetCurrentAction (Action action)
		{
			if (currentAction != null) currentAction.PauseAction();

			if (action.Started) action.ResumeAction();
			else action.StartAction();

			currentAction = action;
		}

		void Update ()
		{
			// Choose what to do next when the agent gets
			// close to the current one.
			if (!agent.pathPending && agent.remainingDistance < 0.5f)
				currentAction.ReachedTargetPoint();
		}
	}
}