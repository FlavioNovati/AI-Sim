using UnityEngine;
using PushdownAutomata;
using System.Collections.Generic;
using UnityEngine.AI;

public class LumberjackController : MonoBehaviour
{
    [SerializeField] private  Forest _forestToCut = null;
    [SerializeField] private WoodStash _woodStash = null;
    
    private IPickable _pickedObject = null;
    private ITree _treeTarget;

    //TODO: REMAKE PRIVATE
    public PDA_Machine _stateMachine = new PDA_Machine();
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();       
    }

    private void Start()
    {
        PDA_State_GetTreeTarget getTreeTarget = new PDA_State_GetTreeTarget("Get Tree Target", _forestToCut);
        getTreeTarget.OnTreeAcquired += HarvestTree;
        _stateMachine.Add(getTreeTarget);
    }

    private void FixedUpdate()
    {
        _stateMachine.Process();
    }

    private void HarvestTree(Tree tree)
    {
        _treeTarget = tree;

        List<PDA_State> harvestTreeRoutine = new List<PDA_State>();

        PDA_State_MoveToTarget moveTowardsTree = new PDA_State_MoveToTarget("Move towards Tree", _agent, _treeTarget, 2f);
        PDA_State_AttackTarget cutTree = new PDA_State_AttackTarget("Attack Tree", _treeTarget, 2f, 3f);
        //Once the tree is cutted pick the drop
        cutTree.OnTargetDropped += PickUpDrop;

        harvestTreeRoutine.Add(moveTowardsTree);
        harvestTreeRoutine.Add(cutTree);

        _stateMachine.Add(harvestTreeRoutine);
    }

    /// <summary>
    /// Reach IPickable and pick it up
    /// </summary>
    /// <param name="dropToPickUp"></param>
    private void PickUpDrop(IPickable dropToPickUp)
    {
        List<PDA_State> pickUpDropRoutine = new List<PDA_State>();

        PDA_State_MoveToTarget moveTowardsDrop = new PDA_State_MoveToTarget("Move towards target", _agent, ref dropToPickUp, 0.15f);
        PDA_State_PickUpObject pickUpObject = new PDA_State_PickUpObject("Pick up target drop", ref dropToPickUp);
        pickUpObject.OnPickUp += PickUpObject;

        pickUpDropRoutine.Add(moveTowardsDrop);
        pickUpDropRoutine.Add(pickUpObject);

        _stateMachine.Add(pickUpDropRoutine);
    }

    /// <summary>
    /// Store IPickable inside a local variable
    /// </summary>
    /// <param name="pickable"></param>
    private void PickUpObject(IPickable pickable)
    {
        _pickedObject = pickable;

        //if is a trunk -> save it in the stash
        if (_pickedObject.Transform.gameObject.TryGetComponent<Trunk>(out Trunk trunk))
            StoreTrunk(pickable, _woodStash);
    }

    private void StoreTrunk(IPickable pickable, IStash stash)
    {
        List<PDA_State> stashPickableRoutine = new List<PDA_State>();

        PDA_State_MoveToTarget moveTowardsStash = new PDA_State_MoveToTarget("Move towards target", _agent, ref stash, 3f);
        PDA_State_StoreToStash storeToStash = new PDA_State_StoreToStash("Store pickable to stash", pickable, stash);

        stashPickableRoutine.Add(moveTowardsStash);
        stashPickableRoutine.Add(storeToStash);

        _stateMachine.Add(stashPickableRoutine);

        storeToStash.OnFinished += RecycleWorkRoutine;
    }

    private void RecycleWorkRoutine()
    {
        PDA_State_GetTreeTarget getTreeTarget = new PDA_State_GetTreeTarget("Get Tree Target", _forestToCut);
        getTreeTarget.OnTreeAcquired += HarvestTree;
        _stateMachine.Add(getTreeTarget);
    }
}
