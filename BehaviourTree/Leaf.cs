using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public abstract class Leaf : Node
	{
		protected virtual StatusFlag Init()
		{
			return StatusFlag.Running;
		}

		protected virtual StatusFlag Process()
		{
			return StatusFlag.Failure;
		}

		public override StatusFlag Evaluate()
		{
			if(status == StatusFlag.None)
			{
				status = Init();
			}

			if(status == StatusFlag.Running)
			{
				status = Process();
			}

			return status;
		}

		public override void Reset()
		{
			status = StatusFlag.None;
		}
	}
}
