using UnityEngine;

public class Trunk : MonoBehaviour, IPickable
{
    public Transform Transform => transform;

    public void PickUp()
    {
        gameObject.SetActive(false);
    }

    public void PutDown(Vector3 dropPosition)
    {
        transform.position = dropPosition;
        gameObject.SetActive(true);
    }
}
