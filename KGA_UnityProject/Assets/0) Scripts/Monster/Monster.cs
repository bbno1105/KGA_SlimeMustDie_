using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : CharacterInfo
{
    public Transform StartPos { get; private set; }
    public void SetStartPos(Transform _Pos)
    {
        this.StartPos = _Pos;
    }

    public Transform EndPos { get; private set; }
    public void SetEndPos(Transform _Pos)
    {
        this.EndPos = _Pos;
    }

    [SerializeField] Face face;
    int normalFaceNum;
    [SerializeField] NavMeshAgent navAgent;

    Animator anim;
    Rigidbody rigidbody;
    SphereCollider sphereCollider;
    Material faceMaterial;
    Material bodyMaterial;
    GameObject HPBar;

    bool isAttack;
    GameObject target;
    Color defaultColor;

    void Start()
    {
        anim = this.transform.GetComponent<Animator>();
        rigidbody = this.transform.GetComponent<Rigidbody>();
        sphereCollider = this.transform.GetComponent<SphereCollider>();

        bodyMaterial = this.transform.GetChild(1).GetComponent<Renderer>().materials[0];
        faceMaterial = this.transform.GetChild(1).GetComponent<Renderer>().materials[1];
        HPBar = this.transform.GetChild(2).gameObject;

        Initialized();
    }

    private void Initialized()
    {
        this.HP = this.MaxHP;
        SetEndPos(StageControl.Instance.stageInfo[GameData.Player.nowStage].GoalPOS.transform);
        this.SetState(STATE.MOVE);
        normalFaceNum = Random.Range(0, face.NormalFace.Length);
        defaultColor = bodyMaterial.color;
    }

    void Update()
    {
        if(this.State == STATE.MOVE)
        {
            Move();
        }
    }

    // ----------------------------------------------------------------[이동]
    void Move()
    {
        navAgent.isStopped = false;
        navAgent.updateRotation = true;

        navAgent.SetDestination(this.EndPos.position);
        SetFace(face.NormalFace[normalFaceNum]);
    }

    void OnAnimatorMove()
    {
        Vector3 position = anim.rootPosition;
        position.y = navAgent.nextPosition.y;
        transform.position = position;
        navAgent.nextPosition = transform.position;
    }

    // ----------------------------------------------------------------[데미지 받음]
    public void DamagedHP(float _damage)
    {
        this.tag = "Untagged";
        this.HP -= (int)_damage;
        anim.SetTrigger(AnimString.Damaged);
        HPBar.SetActive(true);


        if (this.State == STATE.MOVE)
        {
            StartCoroutine(DamageEffectCoroutine());
        }
        StartCoroutine(HPBarUICoroutine());
        
        if (this.HP <= 0)
        {
            Die();
        }
    }

    IEnumerator DamageEffectCoroutine()
    {
        bodyMaterial.color = new Color(1, 0.2f, 0.2f, 0.8f);

        yield return new WaitForSeconds(0.1f);

        bodyMaterial.color = defaultColor;
    }

    IEnumerator HPBarUICoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        // 때리는 쿨타임을 여기다 넣음 (개날림코드)
        this.tag = "Monster";

        yield return new WaitForSeconds(1.5f);

        HPBar.SetActive(false);
    }
    // ----------------------------------------------------------------[이동속도 감소]
    int trapCount = 0;
    public void Slow(bool _isOn, float _slowSpeed)
    {
        if (_isOn)
        {
            ++trapCount;
            bodyMaterial.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
            this.navAgent.speed = _slowSpeed;
            this.anim.speed = _slowSpeed;
        }
        else
        {
            --trapCount;
        }

        if(trapCount <= 0)
        {
            bodyMaterial.color = defaultColor;
            this.navAgent.speed = 1;
            this.anim.speed = 1;
        }
    }

    // ----------------------------------------------------------------[얼음]
    public void Freeze(float _freezeTime)
    {
        StartCoroutine(FreezeCoroutine(_freezeTime));
    }

    IEnumerator FreezeCoroutine(float _freezeTime)
    {
        bodyMaterial.color = new Color(0f, 0.9f, 1f, 0.5f);

        this.navAgent.speed = 0;
        this.anim.speed = 0;
        this.SetState(STATE.FREEZE);

        yield return new WaitForSeconds(_freezeTime);

        bodyMaterial.color = defaultColor;
        this.navAgent.speed = 1;
        this.anim.speed = 1;
        this.SetState(STATE.MOVE);
    }

    // ----------------------------------------------------------------[죽음]
    void Die()
    {
        this.anim.speed = 1;
        bodyMaterial.color = defaultColor;

        SetFace(face.damageFace);
        this.SetState(STATE.DIE);
        HPBar.SetActive(false);

        this.navAgent.speed = 0;
        this.rigidbody.velocity = Vector3.zero;
        this.rigidbody.useGravity = false;
        this.sphereCollider.enabled = false;

        anim.SetTrigger(AnimString.Dead);
    }

    void SetFace(Texture _texture)
    {
        faceMaterial.SetTexture("_MainTex", _texture);
    }

    public void DestroySlime()
    {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
        {
            DestroySlime();
        }
    }

    //public void Attack()
    //{
    //    target.GetComponent<PlayerInfo>().DamagedHP(this.AttackPower);
    //}

    //    if(other.tag == "Player")
    //    {
    //        SetFace(face.attackFace);

    //        isAttack = true;
    //        this.SetState(STATE.Attack);
    //        target = other.gameObject;

    //        navAgent.isStopped = true;
    //        navAgent.updateRotation = false;

    //        anim.SetBool(AnimString.isAttack, true);
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        SetFace(face.NormalFace[normalFaceNum]);

    //        isAttack = false;
    //        this.SetState(STATE.MOVE);

    //        navAgent.isStopped = false;
    //        navAgent.updateRotation = true;

    //        anim.SetBool(AnimString.isAttack, false);
    //    }
    //}
}
