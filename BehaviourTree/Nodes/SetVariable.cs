using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class SetVariable : Leaf
	{
		public BTVariable<object> variable;
		public object value;

		protected override StatusFlag Init()
		{
			variable.Set(value);

			return StatusFlag.Success;
		}
	}
}