using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This node is just to show how invert the status returned by a child
public class Inverter : Node
{
    public Inverter(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        //This will have only one child to invert it's status, therefore, children[0]
        Status childStatus = children[0].Process();
        if (childStatus == Status.RUNNING) return Status.RUNNING;
        
        //Invert the status to make more sense
        //This case is used to get something more logic about the status of the hasMoney Function 
        if (childStatus == Status.FAILURE) 
            return Status.SUCCESS;
        else
        {
            return Status.FAILURE;
        }
    }
}
