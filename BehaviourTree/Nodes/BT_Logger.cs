using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_Logger : Leaf
	{
		public BTVariable<string> message;

		protected override StatusFlag Process()
		{
			Debug.Log(message.ToString());
			return StatusFlag.Success;
		}
	}
}