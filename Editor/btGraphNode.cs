using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using System;

[System.Serializable]
public class btGraphNode
{
	public enum NodeType
	{
		Leaf, Decorator, Composite, EntryPoint
	}

	public string title;
	public string GUID;
	public NodeType type;
	public List<string> connections = new List<string>();

	public Dictionary<string, string> variables;

	public List<string> variableNames; // used for serialization
	public List<string> variableValues; //

	public Rect position;

	public btGraphNode(btGraphNode clone, bool cloneGuid = false)
	{
		this.title = clone.title;
		this.variables = clone.variables;
		this.GUID = (cloneGuid) ? clone.GUID : Guid.NewGuid().ToString();
		this.type = clone.type;
	}

	public btGraphNode()
	{
		this.variables = new Dictionary<string, string>();
		this.GUID = Guid.NewGuid().ToString();
	}

	public void StoreVariables()
	{
		variableNames = new List<string>();
		variableValues = new List<string>();

		foreach(string key in variables.Keys)
		{
			variableNames.Add(key);
			variableValues.Add(variables[key]);
		}
	}

	public void LoadVariables()
	{
		variables = new Dictionary<string, string>();

		for (int i = 0; i < variableValues.Count; i++)
		{
			variables.Add(variableNames[i], variableValues[i]);
		}
	}
}

public class btGraphNodeUI : Node
{
	public btGraphNode node;
	public string GUID;

	public TextField nameField;
	public TextField varField;

	public btGraphNodeUI(btGraphNode node)
	{
		this.node = node;
		this.GUID = node.GUID;
		title = node.title;

		foreach (string key in node.variables.Keys)
		{
			varField = new TextField(key, 512, false, false, 'a');
			varField.ElementAt(0).style.minWidth = 40;

			varField.RegisterValueChangedCallback<string>((e) =>
			{
				node.variables[key] = e.newValue;
			});

			varField.value = node.variables[key];

			ElementAt(0).Add(varField);
		}

		this.SetPosition(new Rect(node.position.x, node.position.y, 0, 0));
		this.style.width = 200;
		
		if (node.type != btGraphNode.NodeType.EntryPoint)
		{
			AddPort(Direction.Input);
		}
		if(node.type != btGraphNode.NodeType.Leaf)
		{
			AddPort(Direction.Output);
		}

		RefreshExpandedState();
		RefreshPorts();

		userData = GUID;
	}

	public void AddPort(Direction direction)
	{
		Port p = InstantiatePort(Orientation.Horizontal, direction, Port.Capacity.Single, typeof(float));
		if(direction == Direction.Input)
		{
			p.name = "input";
			p.portName = "input";
			inputContainer.Add(p);
		}
		else
		{
			p.name = outputContainer.childCount.ToString();
			p.portName = outputContainer.childCount.ToString();
			outputContainer.Add(p);
		}

		RefreshExpandedState();
		RefreshPorts();
	}

	public void RemovePort(Port p)
	{
		outputContainer.Remove(p);

		int i = 0;
		foreach(VisualElement e in outputContainer.Children())
		{
			e.name = i++.ToString();
		}
	}

	public void OnConnected()
	{
		if(node.type == btGraphNode.NodeType.Composite)
		{
			AddPort(Direction.Output);
		}
	}

	public void OnDisconnected(Port p)
	{
		if(p.parent != this)
		{
			return;
		}

		if (node.type == btGraphNode.NodeType.Composite)
		{
			RemovePort(p);
		}
	}

	public void SetStatusColor(BT.StatusFlag status)
	{
		switch (status)
		{
			case BT.StatusFlag.Failure:
				style.backgroundColor = Color.red;
				break;
			case BT.StatusFlag.Success:
				style.backgroundColor = Color.green;
				break;
			case BT.StatusFlag.Running:
				style.backgroundColor = Color.blue;
				break;
			case BT.StatusFlag.None:
				style.backgroundColor = Color.gray;
				break;
		}
	}
}
