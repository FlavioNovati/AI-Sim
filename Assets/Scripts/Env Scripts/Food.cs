using UnityEngine;

public class Food : MonoBehaviour, IFeedable
{ 
    [SerializeField] private float _feedAmount;
    [SerializeField] private float _stoppingDistance = 0.2f;

    //ITarget
    public Transform Transform => transform;
    public float StoppingDistance => _stoppingDistance;

    //IEatable
    public float FeedAmount => _feedAmount;
    public void FeedEntity(IEntity entityToFeed)
    {
        entityToFeed.Hunger.Increase(FeedAmount);
    }

    //IPickable
    public string PickableName => "food";
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
