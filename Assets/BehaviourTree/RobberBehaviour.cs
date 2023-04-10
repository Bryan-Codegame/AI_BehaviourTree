using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : MonoBehaviour
{
    private BehaviourTree tree;
    public GameObject diamond;
    public GameObject van;
    private NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        tree = new BehaviourTree();
        Node steal = new Node("Steal Something");
        Leaf goToDiammond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);
        
        steal.AddChild(goToDiammond);
        steal.AddChild(goToVan);
        tree.AddChild(steal);
        
        tree.PrintTree();

        tree.Process();
    }

    public Node.Status GoToDiamond()
    {
        //The agent goes to the diamond.
        agent.SetDestination(diamond.transform.position);
        return Node.Status.SUCCESS;
    }
    
    public Node.Status GoToVan()
    {
        //The agent goes to the diamond.
        agent.SetDestination(van.transform.position);
        return Node.Status.RUNNING;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
