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

		[SerializeField]
		private float health = 100;
		[SerializeField]
		private float viewRadius = 4;

		protected NavMeshAgent agent;

		private Action currentAction;

		void Awake ()
		{
			agent = GetComponent<NavMeshAgent>();
		}

		public void SetCurrentAction (Action action)
		{
			if (currentAction == action) return;

			if (currentAction != null) currentAction.PauseAction();

			if (action.Started) action.ResumeAction();
			else action.StartAction();

			currentAction = action;
		}

		//If performance becoming a problem, this could be changed to coroutine that
		//runs once every .1f seconds.
		void Update ()
		{
			// Choose what to do next when the agent gets
			// close to the current one.
			if (!agent.pathPending && agent.remainingDistance < 0.5f + agent.stoppingDistance)
				currentAction.ReachedTargetPoint();

			if (currentAction.UpdateRequired)
				currentAction.UpdateAction();
		}

		//returns true if the player is less than viewRadius units from the NPC.
		protected bool PlayerVisable ()
		{
			return (Player_Controller.player.transform.position - transform.position).sqrMagnitude < viewRadius*viewRadius;
		}
	}
}