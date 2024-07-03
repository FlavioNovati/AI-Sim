using UnityEngine;

public class Forest : ResourceSpawner
{
    [Header("Forest Settings")]
    [SerializeField] private Tree _treeToSpawn;
    [SerializeField] private Color color = Color.yellow;

    protected override void Awake()
    {
        base._obJectToSpawn = _treeToSpawn.gameObject;
        base.Awake();
    }

    private void OnValidate()
    {
        base._spawnAreaSizeColor = color;
    }

    /// <summary>
    /// Get a random tree and remove it from allowed trees
    /// </summary>
    /// <returns></returns>
    public Tree GetTree()
    {
        int treeIndex = UnityEngine.Random.Range(0, base._allowedObjectList.Count - 1);

        if (treeIndex < 0)
            return null;

        Tree tree = _allowedObjectList[treeIndex].GetComponent<Tree>();
        _allowedObjectList.RemoveAt(treeIndex);
        return tree; 
    }
}
