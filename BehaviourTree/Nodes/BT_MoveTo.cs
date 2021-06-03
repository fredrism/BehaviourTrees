using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BT
{
	public class BT_MoveTo : Leaf
	{
		public BTVariable<Vector3> position;
		public BTVariable<BaseAI> target;

		int iterations;
		const int maxIterations = 1024;

		protected override StatusFlag Init()
		{
			NavMeshAgent agent = target.Get().agent;

			agent.SetDestination(position);
			agent.isStopped = false;
			target.Get().StartWalk();

			return StatusFlag.Running;
		}

		protected override StatusFlag Process()
		{
			NavMeshAgent agent = target.Get().agent;

			if (Vector3.Distance(position, agent.transform.position) < 0.1f)
			{
				iterations = 0;
				agent.isStopped = true;
				return StatusFlag.Success;
			}
			else if(iterations > maxIterations || agent.pathStatus == NavMeshPathStatus.PathInvalid)
			{
				iterations = 0;
				agent.isStopped = true;
				return StatusFlag.Failure;
			}

			iterations++;
			return StatusFlag.Running;
		}
	}
}