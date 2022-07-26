using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAnimationEvent : MonoBehaviour
{
    float AttackPower;

    void Start()
    {
        AttackPower = this.transform.parent.GetComponent<PlayerInfo>().AttackPower;
    }

    void Attack1Event()
    {
        Attack(true);
    }

    void Attack2Event()
    {
        Attack(true);
    }

    void EndAttackEvent()
    {
        Attack(false);
    }

    public void Attack(bool _isAttack)
    {
        this.GetComponent<BoxCollider>().enabled = _isAttack;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            Monster monster = other.GetComponent<Monster>();
            if(monster.State != CharacterInfo.STATE.DIE)
            {
                monster.DamagedHP(AttackPower);
            }
        }
    }
}
