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

    int nowSelect = 0;

    void Start()
    {
        
    }

    void Update()
    {
        select();
        SetSelectEffect();
        Create();
    }

    void select()
    {
        // TODO : 나중에 수정할 코드
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
            if (nowSelectTrap) nowSelectTrap.SetActive(false);
            nowSelectTrap = selectTrapPrefabs[nowSelect];
            nowSelectTrap.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            nowSelect = 1;
            if (nowSelectTrap) nowSelectTrap.SetActive(false);
            nowSelectTrap = selectTrapPrefabs[nowSelect];
            nowSelectTrap.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            nowSelect = 2;
            if (nowSelectTrap) nowSelectTrap.SetActive(false);
            nowSelectTrap = selectTrapPrefabs[nowSelect];
            nowSelectTrap.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            nowSelect = 3;
            if (nowSelectTrap) nowSelectTrap.SetActive(false);
            nowSelectTrap = selectTrapPrefabs[nowSelect];
            nowSelectTrap.SetActive(true);
        }

        if (nowSelectTrap == null) return;
        if (nowSelectTrap) nowSelectTrap.SetActive(camerAim.isTarget);

        nowSelectTrap.transform.position = new Vector3(this.SelectEffect.transform.position.x, nowSelectTrap.transform.position.y, this.SelectEffect.transform.position.z);
    }

    void SetSelectEffect()
    {
        if (camerAim.isTarget && nowSelectTrap != null)
        {
            SelectEffect.SetActive(true);
            SelectEffect.transform.position = new Vector3(camerAim.hitPos.x, SelectEffect.transform.position.y, camerAim.hitPos.z);
            if (camerAim.hit.collider.gameObject.GetComponent<Block>().IsTrapOn)
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
        }
        else
        {
            SelectEffect.SetActive(false);
        }
    }

    void Create()
    {
        if (Input.GetMouseButtonDown(0) && camerAim.isTarget)
        {
            Block targetBlock = camerAim.hitObject.GetComponent<Block>();
            if (!camerAim.IsTrapOn && targetBlock != null && nowSelectTrap != null)
            {
                Instantiate(TrapPrefabs[nowSelect], SelectEffect.transform.position, SelectEffect.transform.rotation); // 로테이션이 나중에 함정 방향이 될 것
                targetBlock.SetTrap();
            }
        }
    }
}
