using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Trap : MonoBehaviour
{
    Monster monster;
    Animator anim;
    NavMeshAgent agent;

    public float MoveSpeed;
    public float Damage;

    float nowTime = 0;
    public float CoolTime;
    public float Cost;

    public bool isTrapOn;
    bool isRigidTrap;

    Material material;
    Color defaultColor;

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
        if (!isTrapOn)
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

    public void DamageTrap(Collider _other)
    {
        monster = _other.GetComponent<Monster>();
        monster.DamagedHP(Damage);
    }

    public void MovementTrap(Collider _other)
    {
        anim = _other.GetComponent<Animator>();
        agent = _other.GetComponent<NavMeshAgent>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Monster")
        {
            if(isTrapOn)
            {
                isTrapOn = false;
                nowTime = 0;
                TrapEffectOn(false);

                DamageTrap(other);
                MovementTrap(other);
        
                if(isRigidTrap)
                {
                    Rigidbody rigid = other.GetComponent<Rigidbody>();
                    //rigid.AddForce(.back * 1000f + Vector3.up * 300f);
                }
            }
        }
    }
}
