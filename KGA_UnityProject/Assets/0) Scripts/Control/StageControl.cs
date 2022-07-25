using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControl : SingletonMonoBehaviour<StageControl>
{
    public StageInfo[] stageInfo;

    [SerializeField] GameObject[] MonsterPrefabs;

    float time;

    void Update()
    {
        time += Time.deltaTime;
        if(time > 1f)
        {
            CreateMonster();
            time = 0;
        }
    }

    void CreateMonster()
    {
        // �� �� �� �� �� ��
        Instantiate(MonsterPrefabs[0], stageInfo[0].StartPOS.transform);
    }

    
}
