using UnityEngine;

public class Water : MonoBehaviour, IEatable
{
    [SerializeField] private float _feedAmount;

    public Transform Transform => transform;

    public float FeedAmount { get; set; }

    private void Awake()
    {
        FeedAmount = _feedAmount;
    }

    public void FeedMe(IEntity entityToFeed)
    {
        entityToFeed.Thirst.Increase(FeedAmount);
    }

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
