using UnityEngine;

[CreateAssetMenu(fileName = "New Lumberjack Data", menuName = "Settings/Lumberjack")]
public class LumberjackData : ScriptableObject
{
    [Header("Needs Settings")]
    [SerializeField] public Necessity HungerNeeds = new Necessity();
    [SerializeField] public Necessity ThirstNeeds = new Necessity();
    [Header("Tasks Consumption")]
    [SerializeField] public TaskConsumption AttackingConsumption = new TaskConsumption(1.7f, 2f);
    [SerializeField] public TaskConsumption WalkingConsumption = new TaskConsumption(0.2f, 0.25f);
    [Header("Attacking Settings")]
    [SerializeField] public float Damage = 2f;
    [SerializeField] public float AttackDelay = 1.5f;
    [Header("Stopping Distances")]
    [SerializeField] public float TreeStoppingDistance = 1f;
    [SerializeField] public float DropStoppingDistance = 0.5f;
    [SerializeField] public float StashStoppingDistance = 3f;
    [SerializeField] public float HouseStoppingDistance = 1.5f;
}
