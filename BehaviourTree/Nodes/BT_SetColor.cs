using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_SetColor : Leaf
	{
		public BTVariable<MeshRenderer> target;
		public BTVariable<Color> color;

		protected override StatusFlag Init()
		{
			MeshRenderer mr = target.Get();

			if(mr != null)
			{
				mr.material.color = color;
				return StatusFlag.Success;
			}

			return StatusFlag.Failure;
		}
	}
}