using UnityEngine;

public class Water : MonoBehaviour, IFeedable
{
    [SerializeField] private float _feedAmount = 15f;
    [SerializeField] private float _stoppingDistance = 0.25f;

    //ITarget
    public Transform Transform => transform;
    public float StoppingDistance => _stoppingDistance;

    //IEatable implementation
    public float FeedAmount => _feedAmount;
    public void FeedEntity(IEntity entityToFeed)
    {
        entityToFeed.Thirst.Increase(FeedAmount);
    }

    //IPicable implementation
    public string PickableName => "water";
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
