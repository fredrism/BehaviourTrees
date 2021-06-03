using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_FindAnyTarget : Leaf
	{
		public BTVariable<BaseAI> owner;
		public BTVariable<IDamager> result;
		public float range = 20;

		Collider[] inRange;
		int i = 0;

		protected override StatusFlag Init()
		{
			inRange = Physics.OverlapSphere(owner.Get().transform.position, range);

			if (inRange == null)
			{
				return StatusFlag.Failure;
			}
			if (inRange.Length == 0)
			{
				return StatusFlag.Failure;
			}

			return base.Init();
		}
	
		protected override StatusFlag Process()
		{
			if(owner.Get().HasLOS(inRange[i].transform))
			{
				IDamager newTarget = inRange[i].GetComponent<IDamager>();

				if (newTarget != null)
				{
					return StatusFlag.Success;
				}
			}

			if(i == inRange.Length)
			{
				return StatusFlag.Failure;
			}

			i++;
			return StatusFlag.Running;
		}
	}
}
