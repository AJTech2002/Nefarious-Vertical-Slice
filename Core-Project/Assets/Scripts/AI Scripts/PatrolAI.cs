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

		[SerializeField]
		private float stoppingDistance = 0.8f;

		Action patrolAction;
		Action followAction;

		void Start ()
		{
			patrolAction = new PatrolAction(agent, points);
			followAction = new FollowAction(agent, Player_Controller.player.transform, stoppingDistance);

			SetCurrentAction(patrolAction);

			StartCoroutine(CheckShouldFollow());
		}

		IEnumerator CheckShouldFollow ()
		{
			while (true)
			{
				if (PlayerVisable())
					SetCurrentAction(followAction);
				else
					SetCurrentAction(patrolAction);

				yield return new WaitForSeconds(0.1f);
			}
		}
	}
}