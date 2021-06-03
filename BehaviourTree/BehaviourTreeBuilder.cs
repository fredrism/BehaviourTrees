using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourTreeBuilder
{
	public virtual BehaviourTree Init(BaseAI ai)
	{
		return null;
	}
}
