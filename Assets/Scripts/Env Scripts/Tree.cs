using UnityEngine;

public class Tree : MonoBehaviour, ITree, IDamageable, IDroppable
{
    [SerializeField] private float _healtPoints = 5f;
    [SerializeField] private float _stoppingDistance = 0.25f;
    [SerializeField] private Trunk _objectToDrop;

    //IDamageable
    public float HP { get; set; }
    public event IDamageable.Death OnDeath;
    //IDroppable
    public event IDroppable.Drop OnDrop;
    //ITarget
    public Transform Transform => transform;
    public float StoppingDistance => _stoppingDistance;

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
        //Get random positon near tree
        Vector3 pos = transform.position;
        Vector2 randomPos = UnityEngine.Random.insideUnitCircle * 1f;
        pos.x += randomPos.x;
        pos.z += randomPos.y;
        //Instanciate trunk
        Trunk temp = Instantiate(_objectToDrop, pos, Quaternion.identity);
        temp.gameObject.SetActive(true);
        //Invoke drop event
        OnDrop?.Invoke(temp);
    }
}
