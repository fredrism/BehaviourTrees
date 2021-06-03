using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class BehaviourTree
{
	public Node root;
	public Blackboard blackboard;

	public BehaviourTree(Node root)
	{
		this.root = root;
	}

	public void Init()
	{
		blackboard = Blackboard.CreateFromTree(this);
	}
}
