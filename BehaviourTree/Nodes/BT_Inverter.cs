using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_Inverter : Decorator
	{
		public BT_Inverter(Node child) : base(child)
		{
		}

		public override StatusFlag Evaluate()
		{
			switch(child.Evaluate())
			{
				case StatusFlag.Success:
					return StatusFlag.Failure;

				case StatusFlag.Failure:
					return StatusFlag.Success;
			}

			return StatusFlag.Running;
		}
	}
}