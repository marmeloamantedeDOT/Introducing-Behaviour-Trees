using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : MonoBehaviour
{
    BehaviorTree tree;
    public GameObject diamond;
    public GameObject van;
    NavMeshAgent agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public enum ActionState { IDLE, WORKING};
    ActionState state = ActionState.IDLE;    
     
    
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();

        tree = new BehaviorTree();
        Node steal = new Node("Steal Something");
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);

        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);   
        tree.AddChild(steal);

        /*Node eat = new Node("Eat Something");
        Node pizza = new Node("Go To Pizza Shop");
        Node buy = new Node("Buy Pizza");

        eat.AddChild(pizza);
        eat.AddChild(buy);
        tree.AddChild(eat); */

        tree.PrintTree();

        tree.Process();
    }

    public Node.Status GoToDiamond()
    {
        //agent.SetDestination(diamond.transform.position);
        //return Node.Status.SUCCESS;
        return GoToLocation(diamond.transform.position);
    }

    public Node.Status GoToVan()
    {
        return GoToLocation(van.transform.position);
    }
    Node.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, this.transform.position);
        if (state == ActionState.IDLE) {
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
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
