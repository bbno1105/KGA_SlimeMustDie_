using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControl : SingletonMonoBehaviour<StageControl>
{
    public StageInfo[] stageInfo;
    
    [SerializeField] GameObject[] MonsterPrefabs;

    [SerializeField] float spawnTime;
    float time;

    

    void Update()
    {
        time += Time.deltaTime;
        // TODO : 나중에 스테이지에 따라 나오게 해야할 듯
        if(time > spawnTime)
        {
            int rand = Random.Range(0, 4);
            CreateMonster(0,rand);
            time = 0;
        }
    }

    void CreateMonster(int stage, int monster)
    {
        Instantiate(MonsterPrefabs[monster], stageInfo[stage].StartPOS.transform);
    }

    
}
