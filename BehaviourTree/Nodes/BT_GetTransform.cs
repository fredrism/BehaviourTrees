using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_GetTransform : Leaf
	{
		public BTVariable<Transform> result;
		public BTVariable<object> value;

		protected override StatusFlag Init()
		{
			object val = value.Get();

			MonoBehaviour b = val as MonoBehaviour;

			if(b == null)
			{
				return StatusFlag.Failure;
			}

			result.Set(b.transform);

			return StatusFlag.Success;
		}
	}
}