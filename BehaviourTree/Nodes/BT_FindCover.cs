using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_FindCover : Leaf
	{
		public int resolution;

		public BTVariable<BaseAI> owner;
		public BTVariable<IDamager> hideFrom;
		public BTVariable<Vector3> result;

		int i = 0;
		const int maxDistance = 15;
		Vector3 startDir = Vector3.forward;

		protected override StatusFlag Init()
		{
			i = 0;
			if(owner.Get() != null && hideFrom.Get() != null)
			{
				startDir = Quaternion.Euler(0, Random.Range(0, 360), 0) * startDir;

				return StatusFlag.Running;
			}

			return StatusFlag.Failure;
		}

		protected override StatusFlag Process()
		{
			if(i > resolution)
			{
				return StatusFlag.Failure;
			}

			Vector3 pos = owner.Get().transform.position;
			Vector3 targetPos = hideFrom.Get().GetTransform().position + Vector3.up;
			Vector3 dir = Quaternion.Euler(0, 360f * ((float)i / resolution), 0) * startDir;

			if(Physics.Raycast(targetPos, dir, out RaycastHit hitA, maxDistance))
			{
				if (Physics.Raycast(hitA.point + dir*0.1f, dir, out RaycastHit hitB, maxDistance))
				{
					if (Physics.CapsuleCast(hitB.point - Vector3.up*0.5f, hitB.point + Vector3.up*0.5f, 0.25f, -dir, out RaycastHit hitC))
					{
						result.Set(hitB.point - dir.normalized * hitC.distance - Vector3.up);
					}
				}
			}

			if(Vector3.Distance(pos, result) < maxDistance)
			{
				return StatusFlag.Success;
			}

			return StatusFlag.Running;
		}
	}
}