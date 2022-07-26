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
    SphereCollider collider;
    Material faceMaterial;
    Material bodyMaterial;
    GameObject HPBar;

    bool isAttack;
    GameObject target;

    public float speed;

    void Start()
    {
        anim = this.transform.GetComponent<Animator>();
        rigidbody = this.transform.GetComponent<Rigidbody>();
        collider = this.transform.GetComponent<SphereCollider>();
        bodyMaterial = this.transform.GetChild(1).GetComponent<Renderer>().materials[0];
        faceMaterial = this.transform.GetChild(1).GetComponent<Renderer>().materials[1];
        HPBar = this.transform.GetChild(2).gameObject;

        Initialized();

        speed = anim.speed;
    }

    private void Initialized()
    {
        this.HP = this.MaxHP;
        SetEndPos(StageControl.Instance.stageInfo[GameData.Player.nowStage].GoalPOS.transform);
        this.SetState(STATE.MOVE);
        normalFaceNum = Random.Range(0, face.NormalFace.Length);
    }

    void Update()
    {
        if(this.State != STATE.DIE)
        {
            Move();
            anim.speed = speed;
        }
    }

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

    public void DamagedHP(float _damage)
    {
        this.tag = "Untagged";
        this.HP -= (int)_damage;
        anim.SetTrigger(AnimString.Damaged);
        HPBar.SetActive(true);

        StartCoroutine(DamageEffect());
        StartCoroutine(HPBarEffect());

        if (this.HP <= 0)
        {
            Die();
        }
    }

    IEnumerator DamageEffect()
    {
        Color defaultColor = bodyMaterial.color;
        bodyMaterial.color = new Color(1, 0, 0, 0.8f);


        yield return new WaitForSeconds(0.2f);

        bodyMaterial.color = defaultColor;
        this.tag = "Monster";
    }

    IEnumerator HPBarEffect()
    {
        yield return new WaitForSeconds(1.5f);
        HPBar.SetActive(false);
    }

    void Die()
    {
        SetFace(face.damageFace);
        this.SetState(STATE.DIE);
        HPBar.SetActive(false);

        this.navAgent.Stop();
        this.rigidbody.velocity = Vector3.zero;
        this.rigidbody.useGravity = false;
        this.collider.enabled = false;

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
