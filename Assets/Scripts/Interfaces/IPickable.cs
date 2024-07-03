using UnityEngine;

public interface IPickable : ITarget
{
    public string PickableName { get; }
    public void PickUp();
    public void PutDown(Vector3 dropPosition);
}
