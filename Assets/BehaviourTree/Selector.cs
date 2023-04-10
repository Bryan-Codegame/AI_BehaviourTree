using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    public Selector(string n)
    {
        name = n;
    }
    public override Status Process()
    {
        Status childStatus = children[currentChild].Process();
        if (childStatus == Status.RUNNING) return Status.RUNNING;

        //Since it's a selector, it will select just one option. if the option slected get to Success
        //the currentChild = 0 to restart the selector and don't exceute both children
        if (childStatus == Status.SUCCESS)
        {
            currentChild = 0;
            return Status.SUCCESS;
        }

        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.SUCCESS;
        }

        return Status.RUNNING;
    }
}
