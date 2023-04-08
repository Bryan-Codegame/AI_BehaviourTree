using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : Node
{
   public BehaviourTree()
   {
      name = "Tree";
   }

   public BehaviourTree(string n)
   {
      name = n;
   }

   public void PrintTree()
   {
      string treePrintout = "";
      Stack<Node> nodeStack = new Stack<Node>();
      Node currentNode = this;
      nodeStack.Push(currentNode);

      while (nodeStack.Count != 0)
      {
         Node nextNode = nodeStack.Pop();
         treePrintout += nextNode.name + "\n";
         for (int i = nextNode.children.Count - 1; i >= 0; i--)
         {
            nodeStack.Push(nextNode.children[i]);
         }
      }
      
      Debug.Log(treePrintout);
   }
}
