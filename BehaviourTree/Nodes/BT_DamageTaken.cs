using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_DamageTaken : Leaf
	{
		public BTVariable<Damageable> health;
		public BTVariable<IDamager> damageDealer;

		private int lastHealth = -1;

		protected override StatusFlag Init()
		{
			if (lastHealth == -1)
			{
				lastHealth = health.Get().currentHealth;
			}

			return base.Init();
		}

		protected override StatusFlag Process()
		{
			if (health.Get().currentHealth < lastHealth)
			{
				damageDealer.Set(health.Get().lastRecievedDamageFrom);
				return StatusFlag.Success;
			}

			return StatusFlag.Failure;
		}
	}
}