using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPCAI.Actions;

namespace NPCAI
{
	public class PatrolAI : BaseAI
	{
		public Transform[] points;

		void Start ()
		{
			PatrolAction action = new PatrolAction(agent, points);

			SetCurrentAction(action);
		}
	}
}