using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTAgent : MonoBehaviour
{
    public BehaviourTree tree;
    public NavMeshAgent agent;
    
    public enum ActionState
    {
        IDLE,
        WORKING
    }
    
    public ActionState state = ActionState.IDLE;
    public Node.Status treeStatus = Node.Status.RUNNING;
    
    
    // Start is called before the first frame update
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        tree = new BehaviourTree();
    }
    
    //SET STATUS
    public Node.Status GoToLocation(Vector3 destination)
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
        /*if Equals to SUCCESS because hasGotMoney has to return Success so that
         the agent can start to steal
         */
        //If agent needs money
        if (treeStatus != Node.Status.SUCCESS)
            treeStatus = tree.Process();
    }
}
