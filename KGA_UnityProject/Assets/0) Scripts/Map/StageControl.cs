using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControl : SingletonMonoBehaviour<StageControl>
{
    public StageInfo[] stageInfo;
    
    [SerializeField] GameObject[] MonsterPrefabs;

    [SerializeField] float spawnTime;
    float time;

    [SerializeField] GameObject poolParent;
    [SerializeField] PoolData[] monsterPool;
    [SerializeField] int startPoolCount;

    void Start()
    {
        // 몬스터 풀링 세팅
        monsterPool = new PoolData[MonsterPrefabs.Length];
        for (int i = 0; i < monsterPool.Length; i++)
        {
            monsterPool[i] = new PoolData();

            monsterPool[i].PoolCount = startPoolCount;

            for (int j = 0; j < monsterPool[i].PoolCount; j++)
            {
                GameObject mobObj = Instantiate(MonsterPrefabs[i]);
                mobObj.transform.parent = poolParent.transform;
                monsterPool[i].PoolObject.Add(mobObj);
                monsterPool[i].PoolObject[j].SetActive(false);
            }
        }
    }

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

    void CreateMonster(int _stage, int _monster)
    {
        GameObject monster = MakeMonster(_monster);
        monster.transform.position = stageInfo[_stage].StartPOS.transform.position;
        monster.transform.rotation = stageInfo[_stage].StartPOS.transform.rotation;
    }

    GameObject MakeMonster(int _monsterIndex)
    {
        for (int i = 0; i < monsterPool[_monsterIndex].PoolCount; i++)
        {
            if (!monsterPool[_monsterIndex].PoolObject[i].activeSelf)
            {
                monsterPool[_monsterIndex].PoolObject[i].SetActive(true);
                return monsterPool[_monsterIndex].PoolObject[i];
            }
        }

        GameObject mobObj = Instantiate(MonsterPrefabs[_monsterIndex]);
        mobObj.transform.parent = poolParent.transform;
        monsterPool[_monsterIndex].PoolObject.Add(mobObj);
        monsterPool[_monsterIndex].PoolCount++;
        return monsterPool[_monsterIndex].PoolObject[monsterPool[_monsterIndex].PoolCount - 1];
    }
}
