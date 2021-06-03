using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_CompareHealth : Leaf
	{
		public BTVariable<Damageable> lhs;
		public int rhs;

		public BTComparison comparer;

		protected override StatusFlag Process()
		{
			int currentHealth = lhs.Get().currentHealth;

			switch (comparer)
			{
				case BTComparison.Equal:
					if (currentHealth == rhs)
					{
						return StatusFlag.Success;
					}
					return StatusFlag.Failure;

				case BTComparison.Less:
					if (currentHealth < rhs)
					{
						return StatusFlag.Success;
					}
					return StatusFlag.Failure;

				case BTComparison.Greater:
					if (currentHealth > rhs)
					{
						return StatusFlag.Success;
					}
					return StatusFlag.Failure;
			}

			return StatusFlag.Failure;
		}
	}
}