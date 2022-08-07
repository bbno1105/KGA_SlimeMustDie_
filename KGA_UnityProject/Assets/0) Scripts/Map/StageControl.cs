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

    public bool isStageStart;
    public bool isStageLoading;

    //테스트를 위한 몬스터 스테이지 변수
    int stage;
    int testStage;
    float testStageTime;

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
        testStage = 1;

        Initialize();
    }

    void Initialize()
    {
        isStageStart = false;
        isStageLoading = false;
        testStageTime = 30f;
        MessageControl.Instance.RefreshStageMessage("F키를 누르면 스테이지가 시작됩니다.",1f,false);
        MessageControl.Instance.RefreshStageUI($"{testStage} STAGE");
    }

    void Update()
    {
        StartStage();
    }

    void StartStage()
    {
        if(isStageStart)
        {
            testStageTime -= Time.deltaTime;
            if(testStageTime < 0f)
            {
                if(testStage <= MonsterPrefabs.Length)
                {
                    testStage++;
                    isStageStart = false;
                    StartCoroutine(NextStage());
                }
                testStageTime = 0;
            }

            time += Time.deltaTime;
            // TODO : 나중에 스테이지에 따라 나오게 해야할 듯
            if (time > spawnTime)
            {
                int rand = Random.Range(0, testStage);
                CreateMonster(GameData.Instance.Player.nowStage, rand);
                time = 0;
            }

        }
        else if(!isStageLoading)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(StageLoading());
            }
        }
    }

    IEnumerator NextStage()
    {
        MessageControl.Instance.RefreshStageMessage($"{testStage} 스테이지 클리어", 1f, false);

        yield return new WaitForSeconds(3);

        Initialize();
    }

    IEnumerator StageLoading()
    {
        isStageLoading = true;

        for (int i = 0; i < 5; i++)
        {
            int count = 5 - i;
            MessageControl.Instance.RefreshStageMessage(count.ToString(),1,false);
            yield return new WaitForSeconds(1);
        }
        MessageControl.Instance.RefreshStageMessage("몬스터가 등장합니다.",1,false);
        isStageStart = true;

        yield return new WaitForSeconds(3);

        while (testStageTime > 0)
        {
            MessageControl.Instance.RefreshStageMessage($"남은 시간 : {(int)testStageTime}초", 1f, false);
            yield return new WaitForSeconds(1);
        }
    }

    void CreateMonster(int _stage, int _monster)
    {
        GameObject monster = MakeMonster(_monster);
        monster.transform.position = stageInfo[_stage].MonsterStartPOS.transform.position;
        monster.transform.rotation = stageInfo[_stage].MonsterStartPOS.transform.rotation;
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
