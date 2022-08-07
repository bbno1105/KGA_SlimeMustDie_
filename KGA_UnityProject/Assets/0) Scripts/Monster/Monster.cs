using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : CharacterInfo
{
    public int Reward;

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

    void Awake()
    {
        navAgent = this.transform.GetComponent<NavMeshAgent>();
        anim = this.transform.GetComponent<Animator>();
        sphereCollider = this.transform.GetComponent<SphereCollider>();
        rigid = this.transform.GetComponent<Rigidbody>();
        bodyMaterial = this.transform.GetChild(1).GetComponent<Renderer>().materials[0];
        faceMaterial = this.transform.GetChild(1).GetComponent<Renderer>().materials[1];
        HPBar = this.transform.GetChild(2).gameObject;
        defaultColor = bodyMaterial.color;

        Initialized();
    }

    private void Initialized()
    {
        this.HP = this.MaxHP;
        normalFaceNum = Random.Range(0, face.NormalFace.Length);
        SetFace(face.NormalFace[normalFaceNum]);

        SetEndPos(StageControl.Instance.stageInfo[GameData.Instance.Player.nowStage].MonsterGoalPOS.transform);
        this.transform.position = StageControl.Instance.stageInfo[GameData.Instance.Player.nowStage].MonsterStartPOS.transform.position;

        this.SetState(STATE.MOVE);
        isFly = false;

        this.rigid.isKinematic = true;
        this.sphereCollider.enabled = true;
        this.anim.speed = 1;

        this.navAgent.enabled = true;
        this.navAgent.speed = 1;

        bodyMaterial.color = defaultColor;
        HPBar.SetActive(true);
    }

    void OnEnable()
    {
        Initialized();
    }

    void Update()
    {
        if (this.State == STATE.MOVE && !isFly)
        {
            Move();
        }
    }

    void OnDisable()
    {
        this.transform.position = StageControl.Instance.stageInfo[GameData.Instance.Player.nowStage].MonsterStartPOS.transform.position;
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
            DestroySlime();
        }
        else if (other.tag == "Dead")
        {
            Die();
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
        StartCoroutine(DamageCoroutine(_damage));
        StartCoroutine(HPBarUICoroutine());
    }

    IEnumerator DamageCoroutine(float _damage)
    {
        this.tag = "DamagedMonster";
        this.HP -= (int)_damage;
        anim.SetTrigger(AnimString.Damaged);

        if (this.State == STATE.MOVE)
        {
            bodyMaterial.color = new Color(1, 0.2f, 0.2f, 0.8f);
        }

        yield return new WaitForSeconds(0.1f);

        this.tag = "Monster";
        bodyMaterial.color = defaultColor;

        if (this.HP <= 0)
        {
            Die();
        }
    }

    IEnumerator HPBarUICoroutine()
    {
        HPBar.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        HPBar.SetActive(false);
    }

    // ----------------------------------------------------------------[이동속도 감소]
    int slowTrapCount = 0; // trap의 trigger가 겹치는 경우가 발생하여 추가
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

        if(this.State != STATE.DIE)
        {
            this.SetState(STATE.MOVE);
        }
    }

    // ----------------------------------------------------------------[날아감]
    public void Jump(Vector3 _jumpVector)
    {
        if (isFly && rigid.velocity.y > 0) return;
        isFly = true;

        // rigid와 navi를 같이 사용할 수 없음
        navAgent.enabled = false;
        rigid.isKinematic = false;

        rigid.velocity = Vector3.zero;
        rigid.velocity = _jumpVector;
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

    // ----------------------------------------------------------------[죽음]
    void Die()
    {
        GameData.Instance.Player.AddGold(Reward);

        this.anim.speed = 1;
        bodyMaterial.color = defaultColor;

        SetFace(face.damageFace);
        this.SetState(STATE.DIE);
        HPBar.SetActive(false);

        this.navAgent.speed = 0;
        this.navAgent.enabled = false;
        this.sphereCollider.enabled = false;

        anim.SetTrigger(AnimString.Dead);
    }
}
