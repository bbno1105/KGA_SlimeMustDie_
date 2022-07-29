using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCreator : MonoBehaviour
{
    
    [SerializeField] CameraAim camerAim;
    
    [SerializeField] GameObject SelectEffect;
    
    [SerializeField] GameObject[] TrapPrefabs;
    [SerializeField] GameObject[] selectTrapPrefabs;
    GameObject nowSelectTrap;

    Trap trap;

    int nowSelect = 0;

    void Update()
    {
        select();
        SetSelectEffect();
    }

    void select()
    {
        // TODO : ���߿� ������ �ڵ�
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SelectEffect.transform.rotation *= Quaternion.Euler(0f, -90f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SelectEffect.transform.rotation *= Quaternion.Euler(0f, 90f, 0f);
        }

        if (nowSelectTrap == null) return;
        if (nowSelectTrap) nowSelectTrap.SetActive(camerAim.isTarget);

        nowSelectTrap.transform.position = new Vector3(this.SelectEffect.transform.position.x, this.SelectEffect.transform.position.y, this.SelectEffect.transform.position.z);
        nowSelectTrap.transform.rotation = SelectEffect.transform.rotation;
    }

    void ChangeSelectTrap()
    {
        if (nowSelectTrap) nowSelectTrap.SetActive(false);
        nowSelectTrap = selectTrapPrefabs[nowSelect];
        nowSelectTrap.SetActive(true);
        trap = TrapPrefabs[nowSelect].GetComponent<Trap>();
    }

    void SetSelectEffect()
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
        UnityEngine.Debug.Log($"dirVector : {dirVector}");

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
                UnityEngine.Debug.Log($"{trap}�� i�� {i} / Ground {trap.CanBuildGround} / Ceiling {trap.CanBuildCeiling} / Wall {trap.CanBuildWall} �׷��� {canBuild}");


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

                Create(i);
            }
        }
    }

    void Create(int _trapIndex)
    {
        if (Input.GetMouseButtonDown(0) && camerAim.isTarget)
        {
            Block targetBlock = camerAim.hitObject.GetComponent<Block>();
            if (!camerAim.IsTrapOn && targetBlock != null && nowSelectTrap != null)
            {
                Instantiate(TrapPrefabs[nowSelect], SelectEffect.transform.position, SelectEffect.transform.rotation); // �����̼��� ���߿� ���� ������ �� ��
                targetBlock.SetTrap(_trapIndex);
            }
        }
    }
}
