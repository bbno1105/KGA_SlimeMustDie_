using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public class PoolData
{
    public List<GameObject> PoolObject = new List<GameObject>();
    public int PoolCount = 0;
}

public class TrapCreator : MonoBehaviour
{
    [SerializeField] CameraAim camerAim;
    
    [SerializeField] GameObject SelectEffect;
    
    [SerializeField] GameObject[] TrapPrefabs;
    [SerializeField] GameObject[] selectTrapPrefabs;
    GameObject nowSelectTrap;

    Trap trap;
    float trapAngle = 0;
    int nowSelect = 0;

    [SerializeField] GameObject poolParent;
    [SerializeField] PoolData[] trapPool;
    [SerializeField] int startPoolCount;

    void Start()
    {
        // 기본 세팅
        PlayerControl.Instance.canAttack = true;
        if (nowSelectTrap)
        {
            nowSelectTrap.SetActive(false);
            nowSelectTrap = null;
        }

        // 트랩 풀링 세팅
        trapPool = new PoolData[TrapPrefabs.Length];
        for (int i = 0; i < trapPool.Length; i++)
        {
            trapPool[i] = new PoolData();

            trapPool[i].PoolCount = startPoolCount;

            for (int j = 0; j < trapPool[i].PoolCount; j++)
            {
                GameObject trapObj = Instantiate(TrapPrefabs[i]);
                trapObj.transform.parent = poolParent.transform;
                trapPool[i].PoolObject.Add(trapObj);
                trapPool[i].PoolObject[j].SetActive(false);
            }
        }
    }

    GameObject MakeTrap(int _trapIndex)
    {
        for (int i = 0; i < trapPool[_trapIndex].PoolCount; i++)
        {
            if(!trapPool[_trapIndex].PoolObject[i].activeSelf)
            {
                trapPool[_trapIndex].PoolObject[i].SetActive(true);
                return trapPool[_trapIndex].PoolObject[i];
            }
        }

        GameObject trapObj = Instantiate(TrapPrefabs[_trapIndex]);
        trapObj.transform.parent = poolParent.transform;
        trapPool[_trapIndex].PoolObject.Add(trapObj);
        trapPool[_trapIndex].PoolCount++;
        return trapPool[_trapIndex].PoolObject[trapPool[_trapIndex].PoolCount - 1];
    }


    void Update()
    {
        select();
        setTrap();
    }

    void select()
    {
        // TODO : 나중에 수정할 코드
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerControl.Instance.canAttack = true;
            if (nowSelectTrap)
            {
                nowSelectTrap.SetActive(false);
                nowSelectTrap = null;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            nowSelect = 0;
            ChangeSelectTrap();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            nowSelect = 1;
            ChangeSelectTrap();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            nowSelect = 2;
            ChangeSelectTrap();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            nowSelect = 3;
            ChangeSelectTrap();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            nowSelect = 4;
            ChangeSelectTrap();
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            nowSelect = 5;
            ChangeSelectTrap();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            trapAngle -= 90;
            trapAngle %= 360;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            trapAngle += 90;
            trapAngle %= 360;
        }

        if (nowSelectTrap == null) return;
        if (nowSelectTrap) nowSelectTrap.SetActive(camerAim.isTarget);

        nowSelectTrap.transform.position = this.SelectEffect.transform.position; 
        nowSelectTrap.transform.up = SelectEffect.transform.up;
        nowSelectTrap.transform.Rotate(0, trapAngle, 0);
        // nowSelectTrap.transform.localRotation = Quaternion.Euler(nowSelectTrap.transform.localRotation.x, trapAngle, nowSelectTrap.transform.localRotation.z);
        // UnityEngine.Debug.Log($"nowSelectTrap.transform.forward : {nowSelectTrap.transform.forward}");
    }

    void ChangeSelectTrap()
    {
        PlayerControl.Instance.canAttack = false;

        if (nowSelectTrap) nowSelectTrap.SetActive(false);
        nowSelectTrap = selectTrapPrefabs[nowSelect];
        nowSelectTrap.SetActive(true);
        trap = TrapPrefabs[nowSelect].GetComponent<Trap>();
    }

    void setTrap()
    {
        if (camerAim.isTarget && nowSelectTrap != null)
        {
            SelectEffect.SetActive(true);
            SelectEffect.transform.position = camerAim.hitPos;
            SelectEffect.transform.up = camerAim.hit.normal;

            SelectEffect.transform.position += camerAim.hit.normal;

            Block block = camerAim.hit.collider.gameObject.GetComponent<Block>();

            FindTileDir(SelectEffect.transform.position, camerAim.hit.collider.gameObject.transform.position, block);
            // SelectEffect.transform.position = new Vector3(camerAim.hitPos.x, SelectEffect.transform.position.y, camerAim.hitPos.z);
        }
        else
        {
            SelectEffect.SetActive(false);
        }
    }

    // Ceiling, Ground, Wall * 4
    int[] x = { 0, 0, 0, 0, 1, -1 };
    int[] y = { 1, -1, 0, 0, 0, 0 };
    int[] z = { 0, -0, 1, -1, 0, 0 };

    void FindTileDir(Vector3 _selectPoint, Vector3 _BlockPoint, Block _block)
    {
        Vector3 dirVector = (_BlockPoint - _selectPoint).normalized;

        for (int i = 0; i < 6; i++)
        {
            if(dirVector == new Vector3(x[i], y[i], z[i]))
            {
                bool canBuild = false;

                switch (i)
                {
                    case 0:
                        if (trap.CanBuildCeiling) canBuild = true;
                        break;
                    case 1:
                        if (trap.CanBuildGround) canBuild = true;
                        break;
                    default:
                        if (trap.CanBuildWall) canBuild = true;
                        break;
                }

                if (_block.IsTrapOn[i] || canBuild == false)
                {
                    SelectEffect.transform.GetChild(0).gameObject.SetActive(true);
                    SelectEffect.transform.GetChild(1).gameObject.SetActive(false);
                    camerAim.IsTrapOn = true;
                }
                else
                {
                    SelectEffect.transform.GetChild(0).gameObject.SetActive(false);
                    SelectEffect.transform.GetChild(1).gameObject.SetActive(true);
                    camerAim.IsTrapOn = false;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    Create(i);
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    Delete(i);
                }

            }
        }
    }

    void Create(int _trapIndex)
    {
        if (camerAim.isTarget)
        {
            Block targetBlock = camerAim.hitObject.GetComponent<Block>();
            if (!camerAim.IsTrapOn && targetBlock != null && nowSelectTrap != null)
            {
                if(trap.CanBuild())
                {
                    GameObject trapObj = MakeTrap(nowSelect);
                    trapObj.transform.position = SelectEffect.transform.position;
                    trapObj.transform.rotation = nowSelectTrap.transform.rotation;
                    targetBlock.SetTrap(_trapIndex, trapObj);
                }
                else
                {
                    // 돈이 없어요 메시지
                    MessageControl.Instance.RefreshMessage("돈이 부족합니다.");
                }
            }
            else
            {
                // 여기는 지을 수 없어요 메시지
                MessageControl.Instance.RefreshMessage("이곳에는 건설할 수 없습니다.");
            }
        }
    }

    void Delete(int _trapIndex)
    {
        if (camerAim.isTarget)
        {
            Block targetBlock = camerAim.hitObject.GetComponent<Block>();
            if (camerAim.IsTrapOn && targetBlock != null && nowSelectTrap != null)
            {
                GameObject targetTrap = targetBlock.ClearTrap(_trapIndex);
                targetTrap.SetActive(false);
            }
        }
    }

}
