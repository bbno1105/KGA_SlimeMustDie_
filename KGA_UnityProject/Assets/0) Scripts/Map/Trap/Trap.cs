using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Trap : MonoBehaviour
{
    Monster monster;

    [Header("설치 가능한 장소")]
    public bool CanBuildGround;
    public bool CanBuildWall;
    public bool CanBuildCeiling;

    public float CoolTime;
    float nowCoolTime = 0;

    public int Cost;

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
    [SerializeField] bool isOneTimeAnimation;

    Animator anim;
    [SerializeField] bool isParticle;
    [SerializeField] bool isOneTimeParticle;
    ParticleSystem[] particle;


    public bool isTrapOn;
    public bool isTrapActive;

    // Material material; // 테스트용 Material
    // Color defaultColor;

    [SerializeField] List<GameObject> target = new List<GameObject>();


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
                    if (!target[i].gameObject.activeSelf)
                    {
                        RemoveMonster(target[i].gameObject);
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

                if (isParticle && isOneTimeParticle == false)
                {
                    for (int i = 0; i < particle.Length; i++)
                    {
                        particle[i].Stop();
                    }
                }

                if(isAnimation && isOneTimeAnimation == false)
                {
                    anim.SetBool("isActive", false);
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

    void OnTriggerEnter(Collider _other)
    {
        if (_other.tag == "Monster" || _other.tag == "DamagedMonster")
        {
            AddMonster(_other.gameObject);

            // 지속 함정
            if (isContinuousTrap) TrapPassive(true, _other.gameObject);
        }
    }

    void OnTriggerExit(Collider _other)
    {
        if (_other.tag == "Monster" || _other.tag == "DamagedMonster")
        {
            RemoveMonster(_other.gameObject);

            // 지속 함정
            if(isContinuousTrap) TrapPassive(false, _other.gameObject);
        }
    }

    void TrapActivate(GameObject _target)
    {
        //if (_target.GetComponent<Monster>().State == CharacterInfo.STATE.DIE)
        //{
        //    RemoveMonster(_target);
        //    return;
        //}

        if (isDamageTrap) DamageTrap(_target);
        if (isFreezeTrap) ActiveFreezeTrap(_target);
        if (isRigidTrap) ActiveJumpTrap(_target);

        if(isAnimation)
        {
            if(isOneTimeAnimation)
            {
                anim.SetTrigger("isActive");
            }
            else
            {
                anim.SetBool("isActive", true);
            }
        }
        if(isParticle)
        {
            for (int i = 0; i < particle.Length; i++)
            {
                particle[i].Play();
            }
        }
    }

    void TrapPassive(bool _isOn, GameObject _target)
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

    public bool CanBuild()
    {
        if(GameData.Instance.Player.UseGold(Cost))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddMonster(GameObject _Monster)
    {
        target.Add(_Monster);
        _Monster.GetComponent<Monster>().AddTrap(this.gameObject);
    }

    public void RemoveMonster(GameObject _Monster)
    {
        target.Remove(_Monster);
        _Monster.GetComponent<Monster>().RemoveTrap(this.gameObject);
    }

    // -----------------------------------------------------------------------------------[데미지 함정]
    public void DamageTrap(GameObject _other)
    {
        monster = _other.GetComponent<Monster>();
        monster.DamagedHP(Damage);
    }

    // -----------------------------------------------------------------------------------[빙결 함정]
    public void ActiveFreezeTrap(GameObject _other)
    {
        monster = _other.GetComponent<Monster>();
        monster.Freeze(FreezeTime);
    }

    // -----------------------------------------------------------------------------------[점프 함정]
    public void ActiveJumpTrap(GameObject _other)
    {
        monster = _other.GetComponent<Monster>();
        Vector3 jumpVector = (this.transform.forward * addFource + this.transform.up * 1.5f * addFource);
        monster.Jump(jumpVector);
    }

    // -----------------------------------------------------------------------------------[이속감소 함정]
    public void PassiveSlowTrap(bool _isOn, GameObject _other)
    {
        monster = _other.GetComponent<Monster>();
        monster.Slow(_isOn,SlowSpeed);
    }
}
