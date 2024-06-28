using UnityEngine;
using PushdownAutomata;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Linq;

public class LumberjackController : MonoBehaviour
{
    [SerializeField] private Transform m_TreeTarget = null;

    //TODO: REMAKE PRIVATE
    public PDA_Machine m_StateMachine = new PDA_Machine();
    private List<PDA_State> m_WorkStack = new List<PDA_State>();
    private NavMeshAgent m_Agent;

    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();

        //Work Instructions
        m_WorkStack.Add(new PDA_State("Get To the Wood", GetToTheWood));
        m_WorkStack.Add(new PDA_State("Cut Down Tree", CutTree));
        PDA_State finalWorkState = new PDA_State("Bring Tree To Stack", BringTreeToStack);
        finalWorkState.OnFinished += AddWorkToMachine;
        m_WorkStack.Add(finalWorkState);

        m_StateMachine.Push(m_WorkStack);
        Debug.Log(m_StateMachine.ToString());
    }

    private void FixedUpdate()
    {
        m_StateMachine.Tick();
    }

    public void Drink()
    {
        PDA_State drinkState = new PDA_State("Searching for water", GetToWaterSource);
        m_StateMachine.PushFirst(drinkState);
    }

    private PDA_TaskStatus GetToWaterSource()
    {
        if (GoToPosition(new Vector3(4f, 0f, 4f)) < 0.5f)
            return PDA_TaskStatus.Finish;
        else
            return PDA_TaskStatus.Process;
    }

    private float GoToPosition(Transform target)
    {
        m_Agent.SetDestination(target.position);
        return Vector3.Distance(transform.position, target.position);
    }

    private float GoToPosition(Vector3 target)
    {
        m_Agent.SetDestination(target);
        return Vector3.Distance(transform.position, target);
    }

    private PDA_TaskStatus GetToTheWood()
    {
        if(GoToPosition(m_TreeTarget) < 0.5f)
            return PDA_TaskStatus.Finish;
        else
            return PDA_TaskStatus.Process;
    }

    private PDA_TaskStatus CutTree()
    {
        return PDA_TaskStatus.Finish;
    }

    private PDA_TaskStatus BringTreeToStack()
    {
        //TODO: Go to pick up trunk and then bring it to the stack

        if (GoToPosition(Vector3.zero) < 0.5f)
            return PDA_TaskStatus.Finish;
        else
            return PDA_TaskStatus.Process;
    }

    private void AddWorkToMachine()
    {
        m_StateMachine.Push(m_WorkStack);
    }
}
