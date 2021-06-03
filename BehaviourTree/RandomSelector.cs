using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class RandomSelector : Composite
	{
		public RandomSelector(params Node[] children) : base(children)
		{

		}

		public override StatusFlag Evaluate()
		{
			int i = Random.RandomRange(0, children.Length-1);

			return children[i].Evaluate();
		}
	}
}

