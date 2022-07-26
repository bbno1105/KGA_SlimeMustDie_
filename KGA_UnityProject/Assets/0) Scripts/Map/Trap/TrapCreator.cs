using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCreator : MonoBehaviour
{
    
    [SerializeField] CameraAim camerAim;
    
    [SerializeField] GameObject SelectEffect;
    
    GameObject PlayerTraps;
    [SerializeField] GameObject[] TrapPrefabs;

    int nowSelect = 0;

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
            nowSelect = 0;
            if (PlayerTraps) PlayerTraps.SetActive(false);
            PlayerTraps = TrapPrefabs[nowSelect];
            PlayerTraps.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            nowSelect = 1;
            if (PlayerTraps) PlayerTraps.SetActive(false);
            PlayerTraps = TrapPrefabs[nowSelect];
            PlayerTraps.SetActive(true);
        }

        if (PlayerTraps == null) return;
        if (PlayerTraps) PlayerTraps.SetActive(camerAim.isTarget);

        PlayerTraps.transform.position = new Vector3(this.SelectEffect.transform.position.x, PlayerTraps.transform.position.y, this.SelectEffect.transform.position.z);
    }

    void SetSelectEffect()
    {
        if (camerAim.isTarget && PlayerTraps != null)
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
            if (!camerAim.IsTrapOn && targetBlock != null && PlayerTraps != null)
            {
                Instantiate(PlayerTraps);
                targetBlock.SetTrap();
            }
        }
    }
}
