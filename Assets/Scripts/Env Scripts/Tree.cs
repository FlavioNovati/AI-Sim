using UnityEngine;

public class Tree : MonoBehaviour, ITree, IDamageable, IDroppable
{
    public event IDamageable.Death OnDeath;
    public event IDroppable.Drop OnDrop;

    public Transform Transform => transform;
    [SerializeField] private Trunk _objectToDrop;

    public float HP { get; set; }
    [SerializeField] private float _healtPoints = 5f;

    private void Awake()
    {
        HP = _healtPoints;
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;

        if (HP < 0)
            Die();
    }

    private void Die()
    {
        SpawnTrunk();
        OnDeath?.Invoke();
        this.gameObject.SetActive(false);
    }

    //TODO: Replace With Pooling
    private void SpawnTrunk()
    {
        Vector3 pos = transform.position;
        Vector2 randomPos = UnityEngine.Random.insideUnitCircle * 1f;
        pos.x += randomPos.x;
        pos.z += randomPos.y;
        _objectToDrop = Instantiate(_objectToDrop, pos, Quaternion.identity);
        OnDrop?.Invoke(_objectToDrop);
    }
}
