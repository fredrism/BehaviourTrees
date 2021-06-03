using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_Succeeder : Decorator
	{
		public BT_Succeeder(Node child) : base(child)
		{
		}

		public override StatusFlag Evaluate()
		{
			child.Evaluate();
			status = StatusFlag.Success;
			return StatusFlag.Success;
		}
	}
}
