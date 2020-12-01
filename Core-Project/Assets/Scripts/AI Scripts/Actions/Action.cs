using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCAI.Actions
{
	public abstract class Action
	{
		public bool Started
		{
			get { return started; }
			set { started = started || value; }
		}

		private bool started = false;

		public virtual void StartAction ()
		{
			started = true;
		}

		public virtual void PauseAction () {}

		public virtual void ResumeAction () {}

		public virtual void StopAction () {}

		public virtual void ReachedTargetPoint () {}
	}
}