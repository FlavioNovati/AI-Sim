using UnityEngine;
using PushdownAutomata;
using System.Collections.Generic;
using UnityEngine.AI;

public class LumberjackController : MonoBehaviour, IEntity
{
    public delegate void Setupped();
    public event Setupped OnSetup;

    //Lumberjack parameters
    [SerializeField] private LumberjackData _settings = null;
    [HideInInspector] public Forest ForestToCut = null;
    [HideInInspector] public IStash WoodStash = null;
    [HideInInspector] public IHome Home = null;
    [HideInInspector] public FoodSpawner FoodSource = null;
    [HideInInspector] public WaterSpawner WaterSource = null;

    public PDA_Machine StateMachine { get; private set; } = new PDA_Machine();
    private NavMeshAgent _agent = null;

    //IEntity variables
    public Necessity Thirst { get; set; }
    public Necessity Hunger { get; set; }
    public NavMeshAgent Agent { get => _agent; }
    public LumberjackData Data => _settings;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        //Create new Hunger Necessity
        Hunger = new Necessity();
        Hunger.MaxValue = _settings.HungerNeeds.MaxValue;
        Hunger.StartingValue = _settings.HungerNeeds.StartingValue;
        Hunger.CriticalLevelThreshold = _settings.HungerNeeds.CriticalLevelThreshold;

        //Create new Thirst Necessity
        Thirst = new Necessity();
        Thirst.MaxValue = _settings.ThirstNeeds.MaxValue;
        Thirst.StartingValue = _settings.ThirstNeeds.StartingValue;
        Thirst.CriticalLevelThreshold = _settings.ThirstNeeds.CriticalLevelThreshold;

        //Connect critical events
        Hunger.OnCritical += HungerCrit;
        Thirst.OnCritical += ThirstCrit;
    }

    private void Start()
    {
        RecycleWorkRoutine();
        //Set up Finished event
        OnSetup?.Invoke();
    }

    private void FixedUpdate()
    {
        StateMachine.Process();
    }

    //Move towards a tree and cut it
    private void HarvestTree(ITree tree)
    {
        if (tree == null)
            return;

        //Create new state
        PDA_State_AttackDamageable cutTree = new PDA_State_AttackDamageable("Cut tree", this, tree);
        //Once the tree is cutted pick the drop
        cutTree.OnTargetDropped += PickUp;
        //Apply routine to state machine
        StateMachine.Add(cutTree);
    }

    /// <summary>
    /// Reach IPickable and pick it up
    /// </summary>
    /// <param name="objectToPickUp"></param>
    private void PickUp(IPickable objectToPickUp, bool urgent)
    {
        PDA_State_PickUpObject pickUpObject = new PDA_State_PickUpObject("Pick up " +objectToPickUp.PickableName, objectToPickUp, this);
        //When object is picked up execute PickUpObject
        pickUpObject.OnPickUp += AnalizePickable;

        //Apply routine to state machine
        if (urgent)
            StateMachine.AddUrgent(pickUpObject);
        else
            StateMachine.Add(pickUpObject);
    }

    /// <summary>
    /// if Trunk stash it, if Feedable feed lumberjack
    /// </summary>
    /// <param name="pickable"></param>
    private void AnalizePickable(IPickable pickable)
    {
        //if is a trunk -> save it in the stash
        if (pickable.Transform.gameObject.TryGetComponent<Trunk>(out _))
        {
            StorePickable(pickable, WoodStash);
            return;
        }
       
        //if feedable -> feed lumberjack
        if (pickable.Transform.gameObject.TryGetComponent<IFeedable>(out IFeedable feedable))
            feedable.FeedEntity(this);
    }

    /// <summary>
    /// Store IPickable inside a stash
    /// </summary>
    /// <param name="pickable"></param>
    /// <param name="stash"></param>
    private void StorePickable(IPickable pickable, IStash stash)
    {
        PDA_State_MoveToTarget moveTowardsStash = new PDA_State_MoveToTarget("Move to wood stash", this, stash);
        PDA_State_StoreToStash storeToStash = new PDA_State_StoreToStash("Store trunk to stash", pickable, stash);
        storeToStash.OnFinished += RecycleWorkRoutine;

        //Setup routine
        List<PDA_State> stashPickableRoutine = new List<PDA_State>{ moveTowardsStash, storeToStash };
        //Apply routine to state machine
        StateMachine.Add(stashPickableRoutine);
    }

    private void RecycleWorkRoutine()
    {
        ITree tree = ForestToCut.GetTree();
        HarvestTree(tree);
    }

    private void Rest()
    {
        PDA_State_MoveToTarget moveTowardsHome = new PDA_State_MoveToTarget("Move to home", this, Home);
        PDA_State_Idle waitUntillDay = new PDA_State_Idle("Wait day time", ref DayManager.OnDayStarted);

        //Setup routine
        List<PDA_State> restRoutine = new List<PDA_State>{ moveTowardsHome, waitUntillDay };
        //Apply routine to state machine
        StateMachine.Add(restRoutine);
    }

    private void HungerCrit()
    {
        IFeedable foodTarget = FoodSource.GetFood();
        PickUp(foodTarget, true);
    }

    private void ThirstCrit()
    {
        IFeedable waterTarget = WaterSource.GetWater();
        PickUp(waterTarget, true);
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
