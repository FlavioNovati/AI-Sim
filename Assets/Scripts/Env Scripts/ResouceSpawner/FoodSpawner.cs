using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : ResourceSpawner
{
    [Header("Food spawner Settings")]
    [SerializeField] private Food _foodToSpawn;
    [SerializeField] private Color color = Color.red;

    protected override void Awake()
    {
        base._obJectToSpawn = _foodToSpawn.gameObject;
        base.Awake();
    }

    private void OnValidate()
    {
        base._spawnAreaSizeColor = color;
    }

    /// <summary>
    /// Get a random Food and remove it from allowed foods
    /// </summary>
    /// <returns></returns>
    public Food GetFood()
    {
        int index = UnityEngine.Random.Range(0, base._allowedObjectList.Count - 1);
        Food food = _allowedObjectList[index].GetComponent<Food>();
        _allowedObjectList.RemoveAt(index);
        return food;
    }
}
