using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Trap : MonoBehaviour
{
    Monster monster;

    public float CoolTime;
    float nowCoolTime = 0;

    public float Cost;

    public bool isContinuousTrap; // ���� �����ΰ�
    public float CountinueTime; // �󸶳� ������ ���ΰ�
    float nowCountinueTime;



    [Header("������ ����")]
    public bool isDamageTrap;
    public float Damage;
    public float DamageCooltime; // ��� SPD : �� �ʿ� 1�� ���� ���ΰ�
    float nowDamageCooltime;

    [Header("�̼Ӱ��� ����")]
    public bool isSlowTrap;
    public float SlowSpeed;

    [Header("���� ����")]
    public bool isFreezeTrap;
    public float FreezeTime;

    [Header("���� ����")]
    public bool isRigidTrap;
    public float addFource; // ���� ��

    [Header("����Ʈ")]
    [SerializeField] bool isAnimation;
    Animator anim;
    [SerializeField] bool isParticle;
    ParticleSystem[] particle;


    public bool isTrapOn;
    public bool isTrapActive;

    // Material material; // �׽�Ʈ�� Material
    // Color defaultColor;

    [SerializeField] List<Collider> target = new List<Collider>();


    void Start()
    {
        anim = this.GetComponent<Animator>();
        if (anim != null) isAnimation = true;
        particle = this.GetComponentsInChildren<ParticleSystem>();
        if (particle.Length > 0) isParticle = true;

        // material = this.GetComponent<Renderer>().material;
        Initialized();
    }

    void Initialized()
    {
        // defaultColor = material.color;
    }

    void Update()
    {
        // TrapON & TrapActivate
        if(isTrapOn && target.Count > 0)
        {
            nowDamageCooltime += Time.deltaTime;
            if(DamageCooltime <= nowDamageCooltime)
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] == null)
                    {
                        target.RemoveAt(i);
                        continue;
                    }
                    TrapActivate(target[i]);
                }
                nowDamageCooltime = 0;
            }

            isTrapActive = true;
        }
        else
        { 
            nowCoolTime += Time.deltaTime;
            if(CoolTime <= nowCoolTime)
            {
                isTrapOn = true;
                nowCoolTime = 0;
                nowDamageCooltime = 0;
            }
        }

        // TrapOFF : ���� ���� + ���� ������ �ƴ϶�� ����
        if (isTrapActive && isContinuousTrap == false)
        {
            nowCountinueTime += 1 * Time.deltaTime;
            if (CountinueTime <= nowCountinueTime)
            {
                nowCountinueTime = 0;
                isTrapOn = false;
                isTrapActive = false;

                if (isParticle)
                {
                    for (int i = 0; i < particle.Length; i++)
                    {
                        particle[i].Stop();
                    }
                }
            }
        }

        TrapRenderer(isTrapOn);
    }

    void TrapRenderer(bool _isTrapOn)
    {
        if(_isTrapOn) // �������� �ִϸ��̼�
        {
            // this.material.color = Color.white;
        }
        else // ���� ���� �ִϸ��̼�
        {
            // this.material.color = defaultColor;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            target.Add(other);

            // ���� ����
            if (isContinuousTrap) TrapPassive(true, other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Monster" || other.tag == "DamagedMonster")
        {
            target.Remove(other);

            // ���� ����
            if(isContinuousTrap) TrapPassive(false, other);
        }
    }

    void TrapActivate(Collider _target)
    {
        if (_target.GetComponent<Monster>().State == CharacterInfo.STATE.DIE)
        {
            target.Remove(_target);
            return;
        }

        if (isDamageTrap) DamageTrap(_target);
        if (isFreezeTrap) ActiveFreezeTrap(_target);
        if (isRigidTrap) ActiveJumpTrap(_target);

        if(isAnimation) anim.SetTrigger("isActive");
        if(isParticle)
        {
            for (int i = 0; i < particle.Length; i++)
            {
                particle[i].Play();
            }
        }
    }

    void TrapPassive(bool _isOn, Collider _target)
    {
        if(_isOn)
        {
            PassiveSlowTrap(_isOn, _target);
        }
        else
        {
            PassiveSlowTrap(_isOn, _target);
        }
    }



    // -----------------------------------------------------------------------------------[������ ����]
    public void DamageTrap(Collider _other)
    {
        monster = _other.GetComponent<Monster>();
        monster.DamagedHP(Damage);
    }

    // -----------------------------------------------------------------------------------[���� ����]
    public void ActiveFreezeTrap(Collider _other)
    {
        monster = _other.GetComponent<Monster>();
        monster.Freeze(FreezeTime);
    }

    // -----------------------------------------------------------------------------------[���� ����]
    public void ActiveJumpTrap(Collider _other)
    {
        monster = _other.GetComponent<Monster>();
        Vector3 jumpVector = (this.transform.forward * addFource + this.transform.up * 1.5f * addFource);
        monster.Jump(jumpVector);
        

    }

    // -----------------------------------------------------------------------------------[�̼Ӱ��� ����]
    public void PassiveSlowTrap(bool _isOn, Collider _other)
    {
        monster = _other.GetComponent<Monster>();
        monster.Slow(_isOn,SlowSpeed);
    }
}
