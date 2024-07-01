using UnityEngine;
using PushdownAutomata;
using System.Collections.Generic;
using UnityEngine.AI;

public class LumberjackController : MonoBehaviour, IEntity
{
    [SerializeField] private LumberjackData _settings = null;

    [HideInInspector] public Forest ForestToCut = null;
    [HideInInspector] public IStash WoodStash = null;
    [HideInInspector] public ITarget RestTarget = null;

    private IPickable _pickedObject = null;
    private ITarget _target = null;

    //TODO: REMAKE PRIVATE
    public PDA_Machine _stateMachine = new PDA_Machine();
    private NavMeshAgent _agent = null;

    public Necessity Thirst { get; set; }
    public Necessity Hunger { get; set; }


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        Thirst = new Necessity();
        Thirst.MaxValue = _settings.ThirstNeeds.MaxValue;
        Thirst.StartingValue = _settings.ThirstNeeds.StartingValue;
        Thirst.CriticalLevelThreshold = _settings.ThirstNeeds.CriticalLevelThreshold;
        
        Hunger = new Necessity();
        Hunger.MaxValue = _settings.HungerNeeds.MaxValue;
        Hunger.StartingValue = _settings.HungerNeeds.StartingValue;
        Hunger.CriticalLevelThreshold = _settings.HungerNeeds.CriticalLevelThreshold;

        Hunger.OnCritical += HungerCrit;
        Thirst.OnCritical += ThirstCrit;
    }

    private void Start()
    {
        PDA_State_GetTreeTarget getTreeTarget = new PDA_State_GetTreeTarget("Get Tree Target", ForestToCut);
        getTreeTarget.OnTreeAcquired += HarvestTree;
        _stateMachine.Add(getTreeTarget);
    }

    private void FixedUpdate()
    {
        _stateMachine.Process();
    }

    private void HarvestTree(Tree tree)
    {
        _target = tree;

        List<PDA_State> harvestTreeRoutine = new List<PDA_State>();

        PDA_State_MoveToTarget moveTowardsTree = new PDA_State_MoveToTarget("Move towards Tree", _agent, _target, 0.5f);
        PDA_State_AttackTarget cutTree = new PDA_State_AttackTarget("Attack Tree", _target, 2f, 3f, this, 30f, 30f);
        //Once the tree is cutted pick the drop
        cutTree.OnTargetDropped += PickUp;

        harvestTreeRoutine.Add(moveTowardsTree);
        harvestTreeRoutine.Add(cutTree);

        _stateMachine.Add(harvestTreeRoutine);
    }

    /// <summary>
    /// Reach IPickable and pick it up
    /// </summary>
    /// <param name="objectToPickUp"></param>
    private void PickUp(IPickable objectToPickUp)
    {
        List<PDA_State> pickUpDropRoutine = new List<PDA_State>();

        PDA_State_MoveToTarget moveTowardsDrop = new PDA_State_MoveToTarget("Move towards target", _agent, ref objectToPickUp, 0.15f);
        PDA_State_PickUpObject pickUpObject = new PDA_State_PickUpObject("Pick up target drop", ref objectToPickUp);
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
        if (_pickedObject.Transform.gameObject.TryGetComponent<Trunk>(out _))
            StoreTrunk(pickable, WoodStash);
    }

    /// <summary>
    /// Store IPickable inside a stash
    /// </summary>
    /// <param name="pickable"></param>
    /// <param name="stash"></param>
    private void StoreTrunk(IPickable pickable, IStash stash)
    {
        List<PDA_State> stashPickableRoutine = new List<PDA_State>();

        PDA_State_MoveToTarget moveTowardsStash = new PDA_State_MoveToTarget("Move towards target", _agent, ref stash, 3f);
        PDA_State_StoreToStash storeToStash = new PDA_State_StoreToStash("Store pickable to stash", pickable, stash);
        storeToStash.OnFinished += RecycleWorkRoutine;

        stashPickableRoutine.Add(moveTowardsStash);
        stashPickableRoutine.Add(storeToStash);

        _stateMachine.Add(stashPickableRoutine);
    }

    private void RecycleWorkRoutine()
    {
        PDA_State_GetTreeTarget getTreeTarget = new PDA_State_GetTreeTarget("Get Tree Target", ForestToCut);
        getTreeTarget.OnTreeAcquired += HarvestTree;
        _stateMachine.Add(getTreeTarget);
    }

    private void Rest()
    {
        List<PDA_State> restRoutine = new List<PDA_State>();
        
        PDA_State_MoveToTarget moveTowardsHome = new PDA_State_MoveToTarget("Move towards home", _agent, RestTarget, 0.5f);
        PDA_State_Idle waitUntillDay = new PDA_State_Idle("Wait day time", ref DayManager.OnDayStarted);

        restRoutine.Add(moveTowardsHome);
        restRoutine.Add(waitUntillDay);

        _stateMachine.Add(restRoutine);
    }

    private void HungerCrit()
    {

    }

    private void ThirstCrit()
    {

    }

    private void OnEnable()
    {
        DayManager.OnDayFinished += Rest;
    }

    private void OnDisable()
    {
        DayManager.OnDayFinished -= Rest;
    }
}
