using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BT
{
	public class BT_FollowPath : Leaf
	{
		BTVariable<BaseAI> owner;
		BTVariable<AI_Path> path;

		int currentPoint = 0;
		Vector3 nextPoint = new Vector3();

		protected override StatusFlag Init()
		{
			BaseAI ai = owner.Get();
			AI_Path p = path.Get();
			currentPoint = p.ClosestPoint(ai.transform.position);
			nextPoint = p.NextPoint(ref currentPoint);
			ai.agent.SetDestination(nextPoint);
			ai.agent.isStopped = false;

			return base.Init();
		}

		protected override StatusFlag Process()
		{
			BaseAI ai = owner.Get();
			AI_Path p = path.Get();

			if (Vector3.Distance(ai.transform.position, nextPoint) < 0.5f)
			{
				nextPoint = p.NextPoint(ref currentPoint);
				ai.agent.SetDestination(nextPoint);
			}
			if(ai.agent.pathStatus == NavMeshPathStatus.PathInvalid)
			{
				ai.agent.isStopped = true;
				return StatusFlag.Failure;
			}

			return StatusFlag.Running;
		}
	}
}
