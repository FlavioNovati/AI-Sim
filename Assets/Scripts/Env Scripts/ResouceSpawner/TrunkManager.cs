using System.Collections.Generic;
using UnityEngine;

public class TrunkManager : MonoBehaviour
{
    public static TrunkManager Instance;

    [SerializeField] private Trunk _trunkToSpawn;
    [SerializeField] private int _amount = 3;

    private List<Trunk> _trunkList = new List<Trunk>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }


        for(int i=0; i < _amount; i++)
        {
            _trunkList.Add(Instantiate(_trunkToSpawn, transform));
            _trunkList[^1].gameObject.SetActive(false);
        }
    }

    public Trunk GetTrunk(Vector3 pos)
    {
        for(int i=0; i < _trunkList.Count; i++)
        {
            if(!_trunkList[i].gameObject.activeInHierarchy)
            {
                _trunkList[i].transform.position = pos;
                _trunkList[i].gameObject.SetActive(true);
                return _trunkList[i];
            }
        }

        return null;
    }
}
