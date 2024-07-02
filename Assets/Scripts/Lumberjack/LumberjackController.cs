using UnityEngine;
using PushdownAutomata;
using System.Collections.Generic;
using UnityEngine.AI;

public class LumberjackController : MonoBehaviour, IEntity
{
    public delegate void Setupped();
    public event Setupped OnSetup;

    [SerializeField] private LumberjackData _settings = null;

    [HideInInspector] public Forest ForestToCut = null;
    [HideInInspector] public IStash WoodStash = null;
    [HideInInspector] public ITarget RestTarget = null;

    private IPickable _pickedObject = null;
    private ITarget _target = null;
    
    public PDA_Machine StateMachine { get; private set; } = new PDA_Machine();
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
        PDA_State_GetTreeTarget getTreeTarget = new PDA_State_GetTreeTarget("Get tree target", ForestToCut);
        getTreeTarget.OnTreeAcquired += HarvestTree;
        StateMachine.Add(getTreeTarget);
        //Set up Finished event
        OnSetup?.Invoke();
    }

    private void FixedUpdate()
    {
        StateMachine.Process();
    }

    private void HarvestTree(Tree tree)
    {
        _target = tree;

        PDA_State_MoveToTarget moveTowardsTree = new PDA_State_MoveToTarget("Move to tree", _agent, _target, _settings.TreeStoppingDistance, this, _settings.WalkingConsumption);
        PDA_State_AttackTarget cutTree = new PDA_State_AttackTarget("Cut tree", _target, _settings.Damage, _settings.AttackDelay, this, _settings.AttackingConsumption);
        //Once the tree is cutted pick the drop
        cutTree.OnTargetDropped += PickUp;

        //Setup routine
        List<PDA_State> harvestTreeRoutine = new List<PDA_State>{ moveTowardsTree, cutTree };
        //Apply routine to state machine
        StateMachine.Add(harvestTreeRoutine);
    }

    /// <summary>
    /// Reach IPickable and pick it up
    /// </summary>
    /// <param name="objectToPickUp"></param>
    private void PickUp(IPickable objectToPickUp)
    {
        PDA_State_MoveToTarget moveTowardsDrop = new PDA_State_MoveToTarget("Move towards pickable", _agent, objectToPickUp, _settings.DropStoppingDistance, this, _settings.WalkingConsumption);
        PDA_State_PickUpObject pickUpObject = new PDA_State_PickUpObject("Pick up pickable", ref objectToPickUp);
        //When object is picked up execute PickUpObject
        pickUpObject.OnPickUp += PickUpObject;

        //Setup routine
        List<PDA_State> pickUpDropRoutine = new List<PDA_State>{ moveTowardsDrop, pickUpObject };
        //Apply routine to state machine
        StateMachine.Add(pickUpDropRoutine);
    }

    /// <summary>
    /// Store IPickable inside a local variable, if Trunk stash it, if consumable cosume it
    /// </summary>
    /// <param name="pickable"></param>
    private void PickUpObject(IPickable pickable)
    {
        _pickedObject = pickable;

        //if is a trunk -> save it in the stash
        if (_pickedObject.Transform.gameObject.TryGetComponent<Trunk>(out _))
            StoreTrunk(pickable, WoodStash);

        //TODO: Consume
    }

    /// <summary>
    /// Store IPickable inside a stash
    /// </summary>
    /// <param name="pickable"></param>
    /// <param name="stash"></param>
    private void StoreTrunk(IPickable pickable, IStash stash)
    {

        PDA_State_MoveToTarget moveTowardsStash = new PDA_State_MoveToTarget("Move to wood stash", _agent, stash, _settings.StashStoppingDistance, this, _settings.WalkingConsumption);
        PDA_State_StoreToStash storeToStash = new PDA_State_StoreToStash("Store trunk to stash", pickable, stash);
        storeToStash.OnFinished += RecycleWorkRoutine;

        //Setup routine
        List<PDA_State> stashPickableRoutine = new List<PDA_State>{ moveTowardsStash, storeToStash };
        //Apply routine to state machine
        StateMachine.Add(stashPickableRoutine);
    }

    private void RecycleWorkRoutine()
    {
        PDA_State_GetTreeTarget getTreeTarget = new PDA_State_GetTreeTarget("Get tree target", ForestToCut);
        //When recive tree target -> start harvest tree routine
        getTreeTarget.OnTreeAcquired += HarvestTree;
        //Apply routine to state machine
        StateMachine.Add(getTreeTarget);
    }

    private void Rest()
    {
        PDA_State_MoveToTarget moveTowardsHome = new PDA_State_MoveToTarget("Move to home", _agent, RestTarget, _settings.HouseStoppingDistance, this, _settings.WalkingConsumption);
        PDA_State_Idle waitUntillDay = new PDA_State_Idle("Wait day time", ref DayManager.OnDayStarted);

        //Setup routine
        List<PDA_State> restRoutine = new List<PDA_State>{ moveTowardsHome, waitUntillDay };
        //Apply routine to state machine
        StateMachine.Add(restRoutine);
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
        OnSetup -= OnSetup;
    }
}
