using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class btGraphView : GraphView
{
	public btGraph graph;
	public GraphWindow window;

    public btGraphView()
	{
		styleSheets.Add(Resources.Load<StyleSheet>("BehaviourGraph"));
		SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);


		var grid = new GridBackground();
		Insert(0, grid);
		grid.StretchToParentSize();

		this.AddManipulator(new ContentDragger());
		this.AddManipulator(new SelectionDragger());
		this.AddManipulator(new RectangleSelector());
		this.graphViewChanged = OnGraphViewChanged;
	}

	public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
	{
		List<Port> compatibleports = new List<Port>();

		ports.ForEach((port) =>
		{
			if (startPort != port && startPort.node != port.node)
			{
				
				compatibleports.Add(port);
			}
		});

		return compatibleports;
	}

	public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
	{
		Vector2 mousePos = evt.mousePosition;

		base.BuildContextualMenu(evt);

		evt.menu.InsertAction(0, "Add Composite", (e) =>
		{
			ShowAddNodeMenu(btGraphNode.NodeType.Composite, mousePos);
		});

		evt.menu.InsertAction(0, "Add Decorator", (e) =>
		{
			ShowAddNodeMenu(btGraphNode.NodeType.Decorator, mousePos);
		});

		evt.menu.InsertAction(0, "Add Leaf", (e) =>
		{
			ShowAddNodeMenu(btGraphNode.NodeType.Leaf, mousePos);
		});

		evt.menu.InsertAction(0, "Add EntryPoint", (e) =>
		{
			btGraphNode node = new btGraphNode
			{
				title = "BT_EntryPoint",
				type = btGraphNode.NodeType.Decorator,
			};

			AddElement(graph.AddNode(node, mousePos));
		});
	}

	PopupWindow add_window = null;

	public void ShowAddNodeMenu(btGraphNode.NodeType type, Vector2 mousePosition)
	{
		if(add_window != null)
		{
			Remove(add_window);
			add_window = null;
		}

		List<btGraphNode> options = btGraphNodePreset.AllNodes(type);

		add_window = new PopupWindow();
		add_window.style.width = 200;

		foreach(btGraphNode n in options)
		{
			Button b = new Button(() =>
			{
				AddElement(graph.AddNode(n, mousePosition));
				Remove(add_window);
				add_window = null;
			});

			b.text = n.title;
			add_window.Add(b);
		}
		add_window.style.left = mousePosition.x;
		add_window.style.top = mousePosition.y;
		Add(add_window);
	}

	public void ClearGraph()
	{
		graphElements.ForEach((g) =>
		{
			RemoveElement(g);
		});
	}

	public void Connect(btGraphNodeUI A, btGraphNodeUI B, int port)
	{

		
		Port end = A.outputContainer[port].Q<Port>();
		Port start = B.inputContainer[0].Q<Port>();
		
		Edge e = new Edge
		{
			input = start,
			output = end
		};

		

		start.Connect(e);
		end.Connect(e);
		
		AddElement(e);

		A.OnConnected();
	}

	public GraphViewChange OnGraphViewChanged(GraphViewChange change)
	{
		
		if(change.edgesToCreate != null)
		{
			foreach(Edge e in change.edgesToCreate)
			{
				btGraphNodeUI input = e.input.node as btGraphNodeUI;
				btGraphNodeUI output = e.output.node as btGraphNodeUI;
				output.node.connections.Add(input.GUID);
				output.OnConnected();
			}
		}

		if(change.elementsToRemove != null)
		{
			foreach(VisualElement e in change.elementsToRemove)
			{
				Edge edge = e.Q<Edge>();

				if(edge != null)
				{
					btGraphNodeUI input = edge.input.node as btGraphNodeUI;
					btGraphNodeUI output = edge.output.node as btGraphNodeUI;

					output.node.connections.Remove(input.GUID);
					output.OnDisconnected(edge.input);
					continue;
				}

				btGraphNodeUI node = e.Q<Node>() as btGraphNodeUI;

				if(node != null)
				{
					graph.nodes.Remove(node.node);
				}
			}
		}

		if(change.movedElements != null)
		{
			foreach(GraphElement e in change.movedElements)
			{
				btGraphNodeUI node = e.Q<Node>() as btGraphNodeUI;

				if (node != null)
				{
					node.node.position = node.GetPosition();
				}
			}
		}

		return change;
	}
}
