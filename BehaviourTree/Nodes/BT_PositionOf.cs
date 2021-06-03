using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_PositionOf : Leaf
	{
		public BTVariable<Transform> transform;
		public BTVariable<Vector3> result;

		public override StatusFlag Evaluate()
		{
			Transform t = transform.Get();

			if(t != null)
			{
				result.Set(t.position);
				return StatusFlag.Success;
			}

			return StatusFlag.Failure;
		}
	}
}