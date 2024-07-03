using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WoodStash : MonoBehaviour, IStash
{
    private List<IPickable> _storedObject = new List<IPickable>();
    
    [SerializeField] private float _stoppingDistance = 3f;
    [SerializeField, Tooltip("Text to show stored amount")] private TMP_Text _text;

    //ITarget
    public Transform Transform => transform;
    public float StoppingDistance => _stoppingDistance;

    //IStash
    public void StorePickable(IPickable pickable)
    {
        _storedObject.Add(pickable);
        _text.text = _storedObject.Count.ToString();
    }
}
