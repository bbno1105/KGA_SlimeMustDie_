using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Trap : MonoBehaviour
{
    Monster monster;

    public float CoolTime;
    float nowTime = 0;

    public float Cost;

    public bool isDamageTrap;
    public float Damage;

    public bool isSlowTrap;
    public float Slow;

    public bool isFreezeTrap;
    public float FreezeTime;

    public bool isTrapOn;

    bool isRigidTrap;

    Material material;
    Color defaultColor;

    List<Collider> target = new List<Collider>();

    void Start()
    {
        material = this.GetComponent<Renderer>().material;
        Initialized();
    }

    void Initialized()
    {
        defaultColor = material.color;
    }

    void Update()
    {
        if(isTrapOn && target.Count > 0)
        {
            for (int i = 0; i < target.Count; i++)
            {
                if (target[i] == null)
                {
                    target.RemoveAt(i);
                    continue;
                }
                ActiveTrap(target[i]);
            }
            isTrapOn = false;
            nowTime = 0;
            TrapEffectOn(false);
        }
        else
        { 
            nowTime += 1 * Time.deltaTime;
            if(CoolTime <= nowTime)
            {
                isTrapOn = true;
                TrapEffectOn(true);
            }
        }
    }

    void TrapEffectOn(bool _isTrapOn)
    {
        if(_isTrapOn)
        {
            this.material.color = Color.white;
        }
        else
        {
            this.material.color = defaultColor;
        }
    }


    void ActiveTrap(Collider _target)
    {
        if (_target.GetComponent<Monster>().State == CharacterInfo.STATE.DIE)
        {
            target.Remove(_target);
            return;
        }

        if (isDamageTrap) DamageTrap(_target);
        if (isSlowTrap) SlowTrap(_target);
        if (isFreezeTrap) FreezeTrap(_target);

        if (isRigidTrap)
        {
            Rigidbody rigid = _target.GetComponent<Rigidbody>();
            //rigid.AddForce(.back * 1000f + Vector3.up * 300f);
        }

    }

    public void DamageTrap(Collider _other)
    {
        monster = _other.GetComponent<Monster>();
        monster.DamagedHP(Damage);
    }

    public void SlowTrap(Collider _other)
    {
        //anim = _other.GetComponent<Animator>();
        //agent = _other.GetComponent<NavMeshAgent>();
    }

    public void FreezeTrap(Collider _other)
    {
        StartCoroutine(Freeze(_other));
    }

    IEnumerator Freeze(Collider _other)
    {
        NavMeshAgent agent = _other.GetComponent<NavMeshAgent>();
        Animator anim = _other.GetComponent<Animator>();

        agent.speed = 0;
        anim.speed = 0;
        _other.GetComponent<Monster>().SetState(CharacterInfo.STATE.IDLE);

        yield return new WaitForSeconds(FreezeTime);

        agent.speed = 1;
        anim.speed = 1;
        _other.GetComponent<Monster>().SetState(CharacterInfo.STATE.MOVE);

    }



    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Monster")
        {
            target.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Monster")
        {
            target.Remove(other);
        }
    }


}
