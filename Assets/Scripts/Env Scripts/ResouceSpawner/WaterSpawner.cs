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
        int waterIndex = UnityEngine.Random.Range(0, base._allowedObjectList.Count - 1);
        Water water = _allowedObjectList[waterIndex].GetComponent<Water>();
        _allowedObjectList.RemoveAt(waterIndex);
        return water;
    }
}
