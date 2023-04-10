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
        agent = this.GetComponent<NavMeshAgent>();
        
        tree = new BehaviourTree();
        Node steal = new Node("Steal Something");
        Node goToDiammond = new Node("Go To Diamond");
        Node goToVan = new Node("Go To Van");
        
        steal.AddChild(goToDiammond);
        steal.AddChild(goToVan);
        tree.AddChild(steal);
        
        tree.PrintTree();
        
        //The agent goes to the diamond.
        agent.SetDestination(diamond.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
