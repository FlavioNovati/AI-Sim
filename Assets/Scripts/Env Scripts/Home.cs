using UnityEngine;

public class Home : MonoBehaviour, IHome
{
    [SerializeField] private float _stoppingDistance = 0.01f;

    //ITarget implementation
    public Transform Transform => transform;
    public float StoppingDistance => _stoppingDistance;
}
