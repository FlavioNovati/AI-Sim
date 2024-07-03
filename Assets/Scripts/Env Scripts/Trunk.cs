using UnityEngine;

public class Trunk : MonoBehaviour, IPickable
{
    [SerializeField] private float _stoppingDistance = 0.25f;

    //ITarget
    public Transform Transform => transform;
    public float StoppingDistance => _stoppingDistance;
    
    //IPickable
    public string PickableName => "trunk";
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
