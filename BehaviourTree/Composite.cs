using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public abstract class Composite : Node
	{
		public Node[] children;

		public Composite(params Node[] children)
		{
			this.children = children;
		}

		public override IEnumerable<Node> Children()
		{
			return children;
		}

		public override void Reset()
		{
			foreach(Node n in children)
			{
				n.Reset();
			}
		}
	}
}