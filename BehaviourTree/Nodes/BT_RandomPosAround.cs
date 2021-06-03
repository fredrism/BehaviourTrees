using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BT
{
	public class BT_RandomPosAround : Leaf
	{
		public BTVariable<Vector3> result;
		public BTVariable<Transform> center;
		public float radius;

		int i = 0;
		const int maxIterations = 128;
		protected override StatusFlag Init()
		{
			i = 0;
			return StatusFlag.Running;
		}

		protected override StatusFlag Process()
		{
			Vector3 v = Random.onUnitSphere * radius;
			v.y = 0;
			v += center.Get().position;

			if(NavMesh.SamplePosition(v, out NavMeshHit hit, 4, NavMesh.AllAreas))
			{
				result.Set(hit.position);
				return StatusFlag.Success;
			}

			i++;

			if(i == maxIterations)
			{
				return StatusFlag.Failure;
			}

			return StatusFlag.Running;
		}
	}
}