using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BT
{
	public class Blackboard
	{
		object[] data;
		Dictionary<string, int> indexer;

		public Blackboard(string[] variables)
		{
			data = new object[variables.Length];
			indexer = new Dictionary<string, int>();

			for (int i = 0; i < variables.Length; i++)
			{
				indexer.Add(variables[i], i);
			}
		}

		public int IndexOf(string variable)
		{
			return indexer[variable];
		}

		public static Blackboard CreateFromTree(BehaviourTree bt)
		{
			List<string> variables = new List<string>();

			ProcessNode(bt.root, variables);
			Blackboard blackboard = new Blackboard(variables.ToArray());
			ConnectNodes(bt.root, blackboard);

			return blackboard;
		}

		private static void ConnectNodes(Node n, Blackboard blackboard)
		{
			Type t = n.GetType();
			FieldInfo[] fields = t.GetFields();

			foreach (FieldInfo f in fields)
			{
				IBTVar _v = (f.GetValue(n) as IBTVar);

				if (_v != null)
				{
					_v.Connect(blackboard);
				}
			}

			foreach (Node child in n.Children())
			{
				ConnectNodes(child, blackboard);
			}
		}

		private static void ProcessNode(Node n, List<string> variables)
		{
			Type t = n.GetType();
			FieldInfo[] fields = t.GetFields();

			foreach (FieldInfo f in fields)
			{
				IBTVar _v = (f.GetValue(n) as IBTVar);

				if (_v != null)
				{
					if (!variables.Contains(_v.GetName()))
					{
						variables.Add(_v.GetName());
					}
				}
			}

			foreach (Node child in n.Children())
			{
				ProcessNode(child, variables);
			}
		}

		public void Set(string variable, object data)
		{
			this.data[IndexOf(variable)] = data;
		}

		public void Set(int variable, object data)
		{
			this.data[variable] = data;
		}

		public bool TrySet(string variable, object data)
		{
			variable = variable.Replace(" ", "");

			if(indexer.ContainsKey(variable))
			{
				this.data[IndexOf(variable)] = data;
				return true;
			}

			Debug.Log("Failed to set variable: " + variable);

			return false;
		}

		public object Get(string variable)
		{
			return this.data[IndexOf(variable)];
		}

		public object Get(int variable)
		{
			return this.data[variable];
		}
	}
}
