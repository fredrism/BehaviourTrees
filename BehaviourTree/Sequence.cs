using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class Sequence : Composite
	{
		public Sequence(params Node[] children) : base(children)
		{

		}


		public override StatusFlag Evaluate()
		{
			bool anyChildIsRunning = false;

			foreach (Node n in children)
			{
				switch (n.Evaluate())
				{
					case StatusFlag.Success:
						break;

					case StatusFlag.Running:
						anyChildIsRunning = true;
						break;

					case StatusFlag.Failure:
						Reset();
						return StatusFlag.Failure;
				}
			}
			status = anyChildIsRunning ? StatusFlag.Running : StatusFlag.Success;
			
			if(status == StatusFlag.Success)
			{
				Reset();
			}

			return status;
		}
	}
}
