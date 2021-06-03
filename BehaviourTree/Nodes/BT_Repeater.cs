using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_Repeater : Decorator
	{
		public BTVariable<int> repetitions;

		int i = 0;

		public BT_Repeater(Node child) : base(child)
		{
		}

		public override StatusFlag Evaluate()
		{
			StatusFlag status = child.Evaluate();

			if (status == StatusFlag.Failure)
			{
				return status;
			}

			if(i < repetitions)
			{
				i++;
				return StatusFlag.Running;
			}

			i = 0;
			return StatusFlag.Success;
		}
	}
}