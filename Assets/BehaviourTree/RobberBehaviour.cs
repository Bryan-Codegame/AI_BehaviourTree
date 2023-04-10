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

    public enum ActionState
    {
        IDLE,
        WORKING
    }
    private ActionState state = ActionState.IDLE;
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

    //AGENT ACTIONS
    public Node.Status GoToDiamond()
    {
        return GoToLocation(diamond.transform.position);
    }
    
    public Node.Status GoToVan()
    {
        return GoToLocation(van.transform.position);
    }

    //SET STATUS
    Node.Status GoToLocation(Vector3 destination)
    {
        //Distance between destination (Diamond or Van) and agent's position 
        float distanceToTarget = Vector3.Distance(destination, transform.position);
        if (state == ActionState.IDLE)
        {
            //The agent goes to the diamond or to the van.
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
        //pathEndPosition = That pos where the agent has to arrive.
        //if >= 2 means the agent doesn't get there yet.
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (distanceToTarget < 2)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }

        return Node.Status.RUNNING;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
