using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class btGraph
{
	public List<btGraphNode> nodes = new List<btGraphNode>();

	public btGraphNodeUI AddNode(btGraphNode node)
	{
		node = new btGraphNode(node);
		nodes.Add(node);

		return new btGraphNodeUI(node);
	}

	public btGraphNodeUI AddNode(btGraphNode node, Vector2 mousePos)
	{
		node.position = new Rect(mousePos.x, mousePos.y, 0, 0);
		nodes.Add(node);

		return new btGraphNodeUI(node);
	}
}
