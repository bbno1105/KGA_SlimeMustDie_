using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBases_EffectList : MonoBehaviour
{
    public bool IsActive = true;

    public GameObject[] LevelPrefabs;
    public GameObject LevelUpdatePrefab;
    public GameObject InstantiatePoint;

    //public bool Level1;
    //public bool Level2;
    //public bool Level3;

    public int level = 0;

    private bool IsSpawned;
    private GameObject levelPrefabInstance;

    private void OnEnable()
    {
        level = 0;
        IsSpawned = false;
        //bool IsActive = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LevelUp()
    {
        var instance = Instantiate(LevelUpdatePrefab, InstantiatePoint.transform.position, InstantiatePoint.transform.rotation);
        Destroy(instance, 5);
        level += 1;
        if (level > 2)
        {
            level = 0;
        }
        IsSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive && !IsSpawned)
        {
            IsSpawned = true;
            if (levelPrefabInstance != null) Destroy(levelPrefabInstance);
            levelPrefabInstance = Instantiate(LevelPrefabs[level], InstantiatePoint.transform.position, InstantiatePoint.transform.rotation);
            //Debug.Log(level);
        }
    }
}
