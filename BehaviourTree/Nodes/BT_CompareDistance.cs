using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public enum BTComparison
	{
		Less, Equal, Greater
	}

	public class BT_CompareDistance : Leaf
	{
		public BTVariable<Transform> lhs;
		public BTVariable<Vector3> rhs;
		public float distance;
		public BTComparison comparer;

		protected override StatusFlag Process()
		{
			float dist = Vector3.Distance(lhs.Get().position, rhs.Get());

			switch(comparer)
			{
				case BTComparison.Equal:
					if (Mathf.Abs(distance - dist) < 0.01)
					{
						return StatusFlag.Success;
					}
					return StatusFlag.Failure;

				case BTComparison.Less:
					if (dist < distance)
					{
						return StatusFlag.Success;
					}
					return StatusFlag.Failure;

				case BTComparison.Greater:
					if (dist > distance)
					{
						return StatusFlag.Success;
					}
					return StatusFlag.Failure;
			}

			return StatusFlag.Failure;
		}
	}
}