using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : BTAgent
{
    private const int DIAMOND_PRICE = 100;
    
    public GameObject diamond;
    public GameObject van;
    public Transform attachPos;
    
    //We use both doors because the agent has to select the door where he want to enter to.
    public GameObject backDoor;
    public GameObject frontDoor;
    
    [Range(0, 1000)] public int money = 800;
    
    // This Start() is not the same of BTAgent's Start(), that is the reason why we use "new" keyword
    new void Start()
    {
        base.Start();
        
        Sequence steal = new Sequence("Steal Something");
        Selector openDoor = new Selector("Open Door");
        Inverter invertHasMoney = new Inverter("Invert Has Money");
        
        //This si a conditional if it is SUCCESS, the agent proceed with the sequence.
        Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);
       
        //We need to use backdoor and frontdoor inside of a selector
        Leaf goToBackDoor = new Leaf("Go To BackDoor", GoToBackDoor);
        Leaf goToFrontDoor = new Leaf("Go To FrontDoor", GoToFrontDoor);
        
        Leaf goToDiammond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);
        
        //INVERTER
        invertHasMoney.AddChild(hasGotMoney);
        
        //SELECTOR
        openDoor.AddChild(goToFrontDoor); //Choose this at first because of the order
        openDoor.AddChild(goToBackDoor);
        
        //SEQUENCE
        steal.AddChild(invertHasMoney);
        steal.AddChild(openDoor); //selector
        steal.AddChild(goToDiammond);
        steal.AddChild(goToVan);
        
        //BEHAVIOUR TREE
        tree.AddChild(steal);
        
        tree.PrintTree();
    }

    //AGENT ACTIONS

    /*This function is going to invert the status returned to make more sense*/
    public Node.Status HasMoney()
    {
        if (money < 500)
        {
            return Node.Status.FAILURE; //invert SUCCESS
        }

        return Node.Status.SUCCESS; //invert FAILURE
    }
    public Node.Status GoToBackDoor()
    {
        return GoToDoor(backDoor);
    }

    public Node.Status GoToFrontDoor()
    {
        return GoToDoor(frontDoor);
    }

    public Node.Status GoToDiamond()
    {
        Node.Status s = GoToLocation(diamond.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            //diamond.transform.SetParent(attachPos);
            diamond.transform.parent = attachPos;
        }
        return s;
    }
    
    public Node.Status GoToVan()
    {
        Node.Status s = GoToLocation(van.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            Destroy(diamond);
            money += DIAMOND_PRICE;
        }
        return s;
    }

    //SET STATUS
    public Node.Status GoToDoor(GameObject door)
    {
        Node.Status s = GoToLocation(door.transform.position);
        //If agent gets to the door
        if (s == Node.Status.SUCCESS)
        {
            if (!door.GetComponent<Lock>().isLocked)
            {
                door.SetActive(false);
                return Node.Status.SUCCESS;
            }
            //If it is failure, select next child, in other words, slect other door
            return Node.Status.FAILURE;
        }
        else
        {
            return s;
        }
    }
    
    new Node.Status GoToLocation(Vector3 destination)
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
    
    //Update?
    //it has not update method because it's parent has.
}
