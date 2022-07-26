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
        if(time > spawnTime)
        {
            int rand = Random.Range(0, 4);
            CreateMonster(0,rand);
            time = 0;
        }
    }

    void CreateMonster(int stage, int monster)
    {
        // 일 단 한 마 리 만
        Instantiate(MonsterPrefabs[monster], stageInfo[stage].StartPOS.transform);
    }

    
}
