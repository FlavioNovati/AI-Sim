using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Forest : MonoBehaviour
{
    [SerializeField] private Tree _treeToSpawn;
    [SerializeField] private float _startingTreeCount = 15f;
    [SerializeField] private float _forestSize = 15f;

    private List<Tree> _treesList = new List<Tree>();
    private List<Tree> _allowedTreesList = new List<Tree>();

    private void Awake()
    {
        //Instancitate all trees
        for (int i=0; i < _startingTreeCount; i++)
        {
            _treesList.Add(Instantiate(_treeToSpawn, transform));
        }

        //Arrange all trees
        for (int i = 0; i < _startingTreeCount; i++)
        {
            Vector3 pos = UnityEngine.Random.insideUnitCircle * _forestSize;
            pos.z = pos.y;
            pos.y = 0f;

            _treesList[i].transform.position = transform.position + pos;
        }
        _allowedTreesList = _treesList;
    }

    //TODO: Regrow trees
    public void Regrow() { }

    /// <summary>
    /// Get a random tree that is active in hierarchy
    /// </summary>
    /// <returns></returns>
    public Tree GetTree()
    {
        int treeIndex = UnityEngine.Random.Range(0, _allowedTreesList.Count - 1);
        Tree tree = _treesList[treeIndex];
        _treesList.RemoveAt(treeIndex);
        return tree; 
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, _forestSize);
    }
#endif
}
