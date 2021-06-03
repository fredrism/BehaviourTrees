using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class ExampleBT : MonoBehaviour
{
    BehaviourTree behaviourTree;

    void Start()
    {
        behaviourTree.Init();
        behaviourTree.blackboard.Set("msg", "hello");
    }

    const int UPDATERATE = 30;
    int i = 0;

    void Update()
	{
        if(i > UPDATERATE)
		{
            behaviourTree.root.Evaluate();
            i = 0;
        }

        i++;
    }
}
