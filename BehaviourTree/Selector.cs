using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class Selector : Composite
	{
		public Selector(params Node[] children) : base(children)
		{

		}

		public override StatusFlag Evaluate()
		{
			foreach(Node n in children)
			{
				switch(n.Evaluate())
				{
					case StatusFlag.Success:
						Reset();
						return StatusFlag.Success;

					case StatusFlag.Running:
						return StatusFlag.Running;

					case StatusFlag.Failure:
						break;
				}
			}

			Reset();
			return StatusFlag.Failure;
		}
	}
}


