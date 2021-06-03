using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_PlayAnimation : Leaf
	{
		public BTVariable<Animator> animator;
		public string animationName;

		protected override StatusFlag Init()
		{
			animator.Get().Play(animationName, -1);
			return base.Init();
		}

		protected override StatusFlag Process()
		{
			if (!animator.Get().GetCurrentAnimatorStateInfo(0).IsName(animationName))
			{
				return StatusFlag.Success;
			}

			return StatusFlag.Running;
		}
	}
}
