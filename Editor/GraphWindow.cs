using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;
using UnityEditor.Experimental.GraphView;

public class GraphWindow : EditorWindow
{
	private btGraphView graphview;
	private btGraph graph;
	private string filePath = "";

	[MenuItem("AI/GraphEditor")]
	public static void Open()
	{
		GraphWindow wnd = EditorWindow.GetWindow(typeof(GraphWindow)) as GraphWindow;
		wnd.titleContent = new GUIContent("AI Graph Editor");
		wnd.Show();
	}

	void OnEnable()
	{
		graphview = new btGraphView
		{
			name = "BehaviourTree Editor",
			window = this
		};

		rootVisualElement.Add(graphview);
		graphview.StretchToParentSize();

		BuildToolbar();

		if(filePath != "")
		{
			Load(filePath);
		}
		else
		{
			BuildGraph(new btGraph(), "");
		}
	}

	void BuildToolbar()
	{
		Toolbar t = new Toolbar();

		Button newgraph = new Button(() =>
		{
			BuildGraph(new btGraph(), "");
		});
		newgraph.text = "New";

		Button load = new Button(() =>
		{
			Load();
		});
		load.text = "Load";

		Button save = new Button(() =>
		{
			Save(filePath);
		});
		save.text = "Save";

		Button saveas = new Button(() =>
		{
			Save();
		});
		saveas.text = "Save As";

		Button generate = new Button(() =>
		{
			GenerateCode();
		});
		generate.text = "Generate Code";

		t.Add(newgraph);
		t.Add(load);
		t.Add(save);
		t.Add(saveas);
		t.Add(generate);

		rootVisualElement.Add(t);
	}

	void OnDisable()
	{
		rootVisualElement.Remove(graphview);
	}

	void Load(string path = "")
	{
		if(path.Length == 0)
		{
			path = EditorUtility.OpenFilePanel("Open Graph", Application.dataPath, "bt");
		}
		
		if(path.Length != 0)
		{
			string json_graph = File.ReadAllText(path);

			if(json_graph.Length != 0)
			{
				btGraph g = JsonUtility.FromJson<btGraph>(json_graph);

				if(g != null)
				{
					foreach (btGraphNode node in g.nodes)
					{
						node.LoadVariables();
					}

					Debug.Log("Loaded graph from: " + path);
					BuildGraph(g, path);
				}
			}
		}
	}

	void Save(string path = "")
	{
		if (path.Length == 0)
		{
			path = EditorUtility.SaveFilePanel("Save Graph", Application.dataPath, "BehaviourGraph.bt", "bt");
		}
		
		if(path.Length != 0)
		{
			foreach(btGraphNode node in graph.nodes)
			{
				node.StoreVariables();
			}

			string json_graph = JsonUtility.ToJson(graph, true);
			if(json_graph.Length != 0)
			{
				Debug.Log("Saved graph to: " + path);
				File.WriteAllText(path, json_graph);
			}
		}
	}

	void BuildGraph(btGraph g, string path)
	{
		filePath = path;
		graph = g;

		graphview.ClearGraph();
		graphview.graph = g;

		List<btGraphNodeUI> uinodes = new List<btGraphNodeUI>();

		foreach(btGraphNode n in g.nodes)
		{
			btGraphNodeUI uinode = new btGraphNodeUI(n);
			graphview.AddElement(uinode);
			uinodes.Add(uinode);
		}
		
		for (int i = 0; i < uinodes.Count; i++)
		{
			btGraphNode node = graph.nodes[i];
			btGraphNodeUI uinode = uinodes[i];

			for (int j = 0; j < node.connections.Count; j++)
			{
				btGraphNodeUI connected = uinodes.Find(x => x.GUID == node.connections[j]);
				graphview.Connect(uinode, connected, j);
			}
		}
	}

	void GenerateCode()
	{
		btGraphCodeGen codeGenerator = new btGraphCodeGen();
		codeGenerator.GenerateCode(graph);
	}
}
