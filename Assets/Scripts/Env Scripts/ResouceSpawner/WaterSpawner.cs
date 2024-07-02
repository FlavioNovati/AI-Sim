using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpawner : ResourceSpawner
{
    [Header("Food spawner Settings")]
    [SerializeField] private Water _waterToSpawn;
    [SerializeField] private Color color = Color.blue;

    protected override void Awake()
    {
        base._obJectToSpawn = _waterToSpawn.gameObject;
        base.Awake();
    }

    private void OnValidate()
    {
        base._spawnAreaSizeColor = color;
    }

    /// <summary>
    /// Get a random water and remove it from allowed water
    /// </summary>
    /// <returns></returns>
    public Water GetWater()
    {
        int index = UnityEngine.Random.Range(0, base._allowedObjectList.Count - 1);
        Water food = _allowedObjectList[index].GetComponent<Water>();
        _allowedObjectList.RemoveAt(index);
        return food;
    }
}
