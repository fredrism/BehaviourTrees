using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ScopeInfo
{
	public int firstLine = 0;
	public Stack<int> properties = new Stack<int>();
	public string symbols = "";
	public bool isBlock = false;
	public bool isInProperty = false;
}


public class btGraphCodeGen
{
	Stack<ScopeInfo> scopes = new Stack<ScopeInfo>();
	List<string> result = new List<string>();

	public void GenerateCode(btGraph graph)
	{
		string path = EditorUtility.SaveFilePanel("Save Code", Application.dataPath, "behaviour" + ".cs", "cs");

		string[] l = path.Split('/');
		string[] o = l[l.Length - 1].Split('.');
		string name = o[0];

		scopes.Push(
			new ScopeInfo
			{
				isBlock = true
		});


		WriteSymbol(DefaultUsings);
		WhiteSpace();
		BeginBlock("public class " + name + " : BehaviourTreeBuilder");
		Property("BehaviourTree bt");

		BeginBlock("public override BehaviourTree Init(BaseAI ai)");

		BeginProperty();
		BeginParams("BehaviourTree bt = new BehaviourTree");

		btGraphNode root = graph.nodes.Find(x => x.title == "BT_EntryPoint");
		ProcessNode(root, graph);

		EndScope();
		EndProperty();

		Property("bt.Init()");
		WriteSymbol(DefaultVariables.Replace("\n", "\n" + indent));
		Property("return bt");

		EndScope();
		EndScope();



		string combined = "";
		foreach(string s in result)
		{
			combined += s + "\n";
		}

		File.WriteAllText(path, combined);
		AssetDatabase.Refresh();
	}

	public void ProcessNode(btGraphNode node, btGraph graph)
	{
		BeginProperty();
		BeginParams("new BT." + node.title);

		List<btGraphNode> children = graph.nodes.FindAll(x => node.connections.Contains(x.GUID));
		foreach (btGraphNode child in children)
		{
			ProcessNode(child, graph);
		}
		EndScope();
		BeginInitializer("");
		foreach(string key in node.variables.Keys)
		{
			string value = node.variables[key];
			if(value[0] == '#')
			{
				value = "\"" + value.Substring(1).Replace(" ", "") + "\"";
			}

			Property(key + " = " + value);
		}
		EndScope();
		EndProperty();
	}

	public void WhiteSpace()
	{
		result.Add("");
	}

	public void EndScope()
	{
		ScopeInfo info = scopes.Pop();

		if(info.firstLine == result.Count - 1)
		{
			result.RemoveAt(result.Count - 1);
			return;
		}

		result[result.Count - 1] = result[result.Count - 1].Replace(",", "");

		string c = info.symbols[1].ToString();
		if (!info.isBlock && !info.isInProperty)
		{
			c += ";";
		}

		WriteSymbol(c);
	}

	public void BeginBlock(string line)
	{
		WriteSymbol(line);
		WriteSymbol("{");

		scopes.Push(
			new ScopeInfo()
			{
				firstLine = result.Count - 1,
				symbols = "{}",
				isBlock = true,
				isInProperty = !scopes.Peek().isBlock
			});

	}

	public void BeginInitializer(string line)
	{
		if(line.Length != 0)
		{
			WriteSymbol(line);
		}
		
		WriteSymbol("{");

		scopes.Push(
			new ScopeInfo()
			{
				firstLine = result.Count - 1,
				symbols = "{}",
				isBlock = false,
				isInProperty = !scopes.Peek().isBlock
			});
	}

	public void BeginParams(string line)
	{
		if (line.Length != 0)
		{
			WriteSymbol(line);
		}

		WriteSymbol("(");

		scopes.Push(
			new ScopeInfo()
			{
				firstLine = result.Count - 1,
				symbols = "()",
				isBlock = false,
				isInProperty = !scopes.Peek().isBlock
			});
	}

	public void BeginProperty()
	{
		ScopeInfo scope = scopes.Peek();

		scope.properties.Push(0);
	}

	public void EndProperty()
	{
		ScopeInfo scope = scopes.Peek();
		scope.properties.Pop();
		scope.properties.Push(result.Count-1);
		if(!scope.isBlock)
		{
			result[result.Count - 1] = result[result.Count - 1] + ",";
		}
	}

	public void Property(string line)
	{
		BeginProperty();
		WriteLine(line);
		EndProperty();
	}

	public void WriteLine(string line)
	{
		ScopeInfo scope = scopes.Peek();

		if(scope.isBlock)
		{
			line += ";";
		}

		result.Add(indent + line);
	}

	public void WriteSymbol(string symbol)
	{
		result.Add(indent + symbol);
	}

	public string indent
	{
		get
		{
			string s = "";
			for(int i = 0; i < scopes.Count-1; i++)
			{
				s += "\t";
			}

			return s;
		}
	}

	const string DefaultUsings = "using System.Collections; \nusing System.Collections.Generic; \nusing UnityEngine; \nusing UnityEngine.AI; \nusing BT; \n \n \n";
	const string DefaultVariables = "bt.blackboard.TrySet(\"transform\", ai.transform);\nbt.blackboard.TrySet(\"owner\",ai); \nbt.blackboard.TrySet(\"player\", PlayerCharacter.Get());\n";
}
