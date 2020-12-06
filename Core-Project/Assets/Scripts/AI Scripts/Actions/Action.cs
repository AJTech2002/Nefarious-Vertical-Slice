using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPCAI.Actions
{
	public abstract class Action
	{
		public bool Started
		{
			get { return started; }
		}

		public bool UpdateRequired
		{
			get { return updateRequired; }
		}

		private   bool started        = false;
		protected bool updateRequired = false;

		protected NavMeshAgent agent;

		public virtual void StartAction ()
		{
			started = true;
		}

		public virtual void UpdateAction () {}

		public virtual void PauseAction () {}

		public virtual void ResumeAction () {}

		public virtual void StopAction ()
		{
			agent.destination = agent.transform.position;
		}

		public virtual void ReachedTargetPoint () {}
	}
}