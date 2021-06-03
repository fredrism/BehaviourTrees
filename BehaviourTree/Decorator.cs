using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public abstract class Decorator : Node
	{
		public Node child;

		public Decorator(Node child)
		{
			this.child = child;
		}

		public override IEnumerable<Node> Children()
		{
			return new Node[] { child };
		}

		public override void Reset()
		{
			child.Reset();
		}
	}
}
