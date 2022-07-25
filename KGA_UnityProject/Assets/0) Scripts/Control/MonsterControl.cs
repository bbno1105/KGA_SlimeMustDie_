using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControl : MonoBehaviour
{
    [SerializeField] MonsterInfo monsterInfo;

    void Move()
    {

    }

    void Die()
    {

    }



    void Spawn(Transform _stageSpawnPos)
    {
        monsterInfo.SetStartPos(_stageSpawnPos);
    }
}
