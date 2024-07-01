using UnityEngine;

public interface IPickable : ITarget
{
    public void PickUp();
    public void PutDown(Vector3 dropPosition);
}
