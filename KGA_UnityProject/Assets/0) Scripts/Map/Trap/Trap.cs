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

    public bool isContinuousTrap; // 무한 지속인가
    public float CountinueTime; // 얼마나 지속할 것인가
    float nowCountinueTime;



    [Header("데미지 함정")]
    public bool isDamageTrap;
    public float Damage;
    public float DamageCooltime; // 사실 SPD : 몇 초에 1대 때릴 것인가
    float nowDamageCooltime;

    [Header("이속감소 함정")]
    public bool isSlowTrap;
    public float SlowSpeed;

    [Header("빙결 함정")]
    public bool isFreezeTrap;
    public float FreezeTime;

    [Header("점프 함정")]
    public bool isRigidTrap;
    public float addFource; // 점프 힘

    [Header("이펙트")]
    [SerializeField] bool isAnimation;
    Animator anim;
    [SerializeField] bool isParticle;
    ParticleSystem[] particle;


    public bool isTrapOn;
    public bool isTrapActive;

    // Material material; // 테스트용 Material
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

        // TrapOFF : 함정 켜짐 + 무한 지속이 아니라면 들어옴
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
        if(_isTrapOn) // 켜진상태 애니메이션
        {
            // this.material.color = Color.white;
        }
        else // 꺼진 상태 애니메이션
        {
            // this.material.color = defaultColor;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            target.Add(other);

            // 지속 함정
            if (isContinuousTrap) TrapPassive(true, other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Monster" || other.tag == "DamagedMonster")
        {
            target.Remove(other);

            // 지속 함정
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



    // -----------------------------------------------------------------------------------[데미지 함정]
    public void DamageTrap(Collider _other)
    {
        monster = _other.GetComponent<Monster>();
        monster.DamagedHP(Damage);
    }

    // -----------------------------------------------------------------------------------[빙결 함정]
    public void ActiveFreezeTrap(Collider _other)
    {
        monster = _other.GetComponent<Monster>();
        monster.Freeze(FreezeTime);
    }

    // -----------------------------------------------------------------------------------[점프 함정]
    public void ActiveJumpTrap(Collider _other)
    {
        monster = _other.GetComponent<Monster>();
        Vector3 jumpVector = (this.transform.forward * addFource + this.transform.up * 1.5f * addFource);
        monster.Jump(jumpVector);
        

    }

    // -----------------------------------------------------------------------------------[이속감소 함정]
    public void PassiveSlowTrap(bool _isOn, Collider _other)
    {
        monster = _other.GetComponent<Monster>();
        monster.Slow(_isOn,SlowSpeed);
    }
}
