using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_WaitForSeconds : Leaf
	{
		public float length = 5;
		float startTime;
		protected override StatusFlag Init()
		{
			startTime = Time.time;
			return base.Init();
		}

		protected override StatusFlag Process()
		{
			if(startTime + length < Time.time)
			{
				return StatusFlag.Success;
			}

			return StatusFlag.Running;
		}
	}
}
