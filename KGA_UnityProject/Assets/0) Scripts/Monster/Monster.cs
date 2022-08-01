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

    NavMeshAgent navAgent;
    Rigidbody rigid;
    Animator anim;

    SphereCollider sphereCollider;

    Material faceMaterial;
    Material bodyMaterial;

    GameObject HPBar;

    bool isAttack;
    bool isFly;

    GameObject target;
    Color defaultColor;

    void Start()
    {
        navAgent = this.transform.GetComponent<NavMeshAgent>();
        anim = this.transform.GetComponent<Animator>();
        sphereCollider = this.transform.GetComponent<SphereCollider>();
        rigid = this.transform.GetComponent<Rigidbody>();
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
        if (this.State == STATE.MOVE && !isFly)
        {
            Move();
        }
    }

    // ----------------------------------------------------------------[�̵�]
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

    // ----------------------------------------------------------------[������ ����]
    public void DamagedHP(float _damage)
    {
        this.tag = "DamagedMonster";
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
        // ������ ��Ÿ���� ����� ���� (�������ڵ�)
        this.tag = "Monster";

        yield return new WaitForSeconds(1.5f);

        HPBar.SetActive(false);
    }

    // ----------------------------------------------------------------[�̵��ӵ� ����]
    int slowTrapCount = 0;
    public void Slow(bool _isOn, float _slowSpeed)
    {
        if (_isOn)
        {
            ++slowTrapCount;
            bodyMaterial.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
            this.navAgent.speed = _slowSpeed;
            this.anim.speed = _slowSpeed;
        }
        else
        {
            --slowTrapCount;
        }

        if(slowTrapCount <= 0)
        {
            bodyMaterial.color = defaultColor;
            this.navAgent.speed = 1;
            this.anim.speed = 1;
            slowTrapCount = 0;
        }
    }

    // ----------------------------------------------------------------[����]
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

    // ----------------------------------------------------------------[����]
    public void Jump(Vector3 _jumpVector)
    {
        if (isFly && rigid.velocity.y > 0) return;
        isFly = true;

        navAgent.enabled = false;
        rigid.isKinematic = false;

        rigid.velocity = Vector3.zero;
        rigid.velocity = _jumpVector;
    }

    // ----------------------------------------------------------------[����]
    void Die()
    {
        this.anim.speed = 1;
        bodyMaterial.color = defaultColor;

        SetFace(face.damageFace);
        this.SetState(STATE.DIE);
        HPBar.SetActive(false);

        this.navAgent.speed = 0;
        this.sphereCollider.enabled = false;

        anim.SetTrigger(AnimString.Dead);
    }

    void SetFace(Texture _texture)
    {
        faceMaterial.SetTexture("_MainTex", _texture);
    }

    public void DestroySlime()
    {
        this.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
        {
            this.gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isFly && collision.gameObject.tag == "Ground")
        {
            isFly = false;
            navAgent.enabled = true;
            rigid.isKinematic = false;
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
