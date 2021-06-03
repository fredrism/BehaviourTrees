using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;
using System;
using System.Reflection;

public static class btGraphNodePreset
{
	public static List<btGraphNode> AllNodes(btGraphNode.NodeType type)
	{
		List<Type> types = new List<Type>();
		Type baseType = typeof(Composite);
		switch (type)
		{
			case btGraphNode.NodeType.Leaf:
				baseType = typeof(Leaf);
				break;
			case btGraphNode.NodeType.Decorator:
				baseType = typeof(Decorator);
				break;
			case btGraphNode.NodeType.Composite:
				baseType = typeof(Composite);
				break;
			case btGraphNode.NodeType.EntryPoint:
				baseType = typeof(Decorator);
				break;
		}

		foreach (Type t in typeof(BehaviourTree).Assembly.GetTypes())
		{
			if(t.IsSubclassOf(baseType))
			{
				types.Add(t);
			}
		}

		List<btGraphNode> nodes = new List<btGraphNode>();

		foreach(Type t in types)
		{
			string title = t.Name;
			Dictionary<string, string> vars = new Dictionary<string, string>();

			foreach (FieldInfo f in t.GetFields())
			{
				if(f.IsPublic)
				{
					if(f.Name == "children" || f.Name == "child")
					{
						continue;
					}

					string val = f.FieldType.Name.Replace("`", "");
					if(val == "BTVariable1")
					{
						val = "#bt";
					}

					if(f.FieldType.IsGenericType)
					{
						val += "<" + f.FieldType.GetGenericArguments()[0].Name + ">";
					}

					vars.Add(f.Name, val);
				}
			}

			btGraphNode node = new btGraphNode
			{
				title = title,
				variables = vars,
				type = type
			};

			nodes.Add(node);
		}

		return nodes;
	}
}