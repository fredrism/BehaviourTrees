using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_Chase : Leaf
	{
		public BTVariable<Transform> target;
		public BTVariable<BaseAI> owner;
		public BTVariable<float> distance;

		Vector3 targetPos;

		protected override StatusFlag Init()
		{
			if(target.Get() == null || owner.Get() == null)
			{
				return StatusFlag.Failure;
			}

			targetPos = new Vector3(-10000, -10000, -10000);
			owner.Get().agent.isStopped = false;
			owner.Get().StartRun();
			return StatusFlag.Running;
		}

		protected override StatusFlag Process()
		{
			Transform trgt = target.Get();
			BaseAI own = owner.Get();

			if (Vector3.Distance(trgt.position, own.transform.position) < distance)
			{
				own.agent.isStopped = true;
				return StatusFlag.Success;
			}

			if(Vector3.Distance(trgt.position, targetPos) > 4)
			{
				targetPos = trgt.position;
				own.agent.SetDestination(targetPos);
			}

			return StatusFlag.Running;
		}
	}
}