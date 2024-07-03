using UnityEngine;
public interface ITarget
{
    public Transform Transform { get; }
    public float StoppingDistance { get;}
}
