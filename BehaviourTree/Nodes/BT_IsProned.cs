using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_IsProned : Leaf
	{
		public BTVariable<BaseAI> owner;

		protected override StatusFlag Init()
		{
			BaseAI ai = owner.Get();

			return ai.isProned ? StatusFlag.Success : StatusFlag.Running;
		}
	}
}