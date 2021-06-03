using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_HasLOS : Leaf
	{
		public BTVariable<Transform> target;
		public BTVariable<BaseAI> owner;

		protected override StatusFlag Process()
		{
			BaseAI ai = owner.Get();
			return ai.HasLOS(target) ? StatusFlag.Success : StatusFlag.Failure;
		}
	}
}