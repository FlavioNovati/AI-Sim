using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WoodStash : MonoBehaviour, IStash
{
    private List<IPickable> _storedObject = new List<IPickable>();

    //Text For Debugging
    [SerializeField] private TMP_Text m_Text;

    public Transform Transform => transform;

    public void StorePickable(IPickable pickable)
    {
        _storedObject.Add(pickable);
        m_Text.text = _storedObject.Count.ToString();
    }
}
