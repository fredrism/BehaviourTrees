using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_NotNull : Leaf
	{
		public BTVariable<object> variable;

		protected override StatusFlag Init()
		{
			return base.Init();
		}

		protected override StatusFlag Process()
		{
			return (variable.Get() == null) ? StatusFlag.Failure : StatusFlag.Success;
		}
	}
}
