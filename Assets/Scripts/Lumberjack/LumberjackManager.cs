using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LumberjackManager : MonoBehaviour
{
    public static LumberjackManager Instance;

    private List<LumberjackController> _lumberjackList;
    private Forest _forestToCut = null;
    private WoodStash _woodStash = null;
    private IHome _home = null;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }

        //Get All lumberjacks
        _lumberjackList = new List<LumberjackController>();
        _lumberjackList = FindObjectsOfType<LumberjackController>().ToList();

        //Get Forest
        _forestToCut = FindObjectOfType<Forest>();

        //Get Wood Stash
        _woodStash = FindObjectOfType<WoodStash>();

        //Get Home
        _home = FindObjectOfType<Home>();

        //Inject data to lumberjacks
        for (int i=0;  i< _lumberjackList.Count; i++)
        {
            _lumberjackList[i].ForestToCut = _forestToCut;
            _lumberjackList[i].WoodStash = _woodStash;
            _lumberjackList[i].RestTarget = _home;
        }
    }



}
