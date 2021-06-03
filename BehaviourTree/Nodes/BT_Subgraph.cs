using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public class BT_Subgraph : Leaf
	{
		public BTVariable<BehaviourTree> graph;
		public BTVariable<BaseAI> owner;

		protected override StatusFlag Init()
		{
			graph.Get().blackboard.Set("owner_baseai", owner.Get());
			return base.Init();
		}

		protected override StatusFlag Process()
		{
			return graph.Get().root.Evaluate();
		}
	}
}