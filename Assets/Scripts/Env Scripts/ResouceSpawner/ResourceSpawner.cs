using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [Header("Resource Spawner Settings")]
    [HideInInspector] protected GameObject _obJectToSpawn;
    [SerializeField] private float _startingObjectCount = 15f;
    [SerializeField] private float _spawnAreaSize = 15f;
    [SerializeField] private float _maxSpawnCount = 50f;
    protected Color _spawnAreaSizeColor = Color.white;

    protected List<GameObject> _spawnList = new List<GameObject>();
    protected List<GameObject> _allowedObjectList = new List<GameObject>();

    [Header("Respawn Settings")]
    [SerializeField] private int _minNew = 3;
    [SerializeField] private int _maxNew = 7;

    protected virtual void Awake()
    {
        //Instancitate all objects
        for (int i = 0; i < _startingObjectCount; i++)
            _spawnList.Add(Instantiate(_obJectToSpawn, transform));

        //Arrange all trees
        for (int i = 0; i < _startingObjectCount; i++)
            _spawnList[i].transform.position = transform.position + GetNewPos();

        //All intanciated object are allowed
        _allowedObjectList = _spawnList;

        DayManager.OnDayStarted += Respawn;
    }

    protected virtual void Respawn()
    {
        if (_spawnList.Count >= _maxSpawnCount)
            return;

        int newAmount = UnityEngine.Random.Range(_minNew, _maxNew);
        int objectRespawned = 0;

        //Pool old Tree
        for (int i = 0; i < newAmount; i++)
        {
            if (!_spawnList[i].gameObject.activeInHierarchy)
            {
                //enable object
                _spawnList[i].gameObject.SetActive(true);
                //Move object
                _spawnList[i].transform.position = transform.position + GetNewPos();
                //add object to allowed object to return
                _allowedObjectList.Add(_spawnList[i]);
                //increase object respawned
                objectRespawned++;
            }
        }
        //Enough objects are in map
        if (objectRespawned == newAmount)
            return;

        //Intanciate new object
        for (int i = 0; i < newAmount; i++)
        {
            //Instanciate
            _spawnList.Add(Instantiate(_obJectToSpawn, transform));
            //add to allowed object
            _allowedObjectList.Add(_spawnList[^1]);
            if (_spawnList.Count >= _maxSpawnCount)
                return;
            objectRespawned++;
        }
    }

    private Vector3 GetNewPos()
    {
        Vector3 pos = UnityEngine.Random.insideUnitCircle * _spawnAreaSize;
        pos.z = pos.y;
        pos.y = 0f;
        return pos;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = _spawnAreaSizeColor;
        Handles.DrawWireDisc(transform.position, Vector3.up, _spawnAreaSize);
    }
#endif
}
