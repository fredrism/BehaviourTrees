using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_EntryPoint : Decorator
	{
		public BT_EntryPoint(Node child) : base(child)
		{
		}

		public override StatusFlag Evaluate()
		{
			StatusFlag s = child.Evaluate();

			if(s != StatusFlag.Running)
			{
				child.Reset();
			}

			return s;
		}
	}
}