using UnityEngine.AI;

public interface IEntity
{
    public Necessity Thirst { get; set; }
    public Necessity Hunger { get; set; }
    public NavMeshAgent Agent { get; }
    public LumberjackData Data { get; }
}
