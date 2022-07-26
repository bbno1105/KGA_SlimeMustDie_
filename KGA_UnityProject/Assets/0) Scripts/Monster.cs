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

    Animator animator;
    Rigidbody rigidbody;
    SphereCollider collider;
    Material faceMaterial;
    Material bodyMaterial;

    void Start()
    {
        animator = this.transform.GetComponent<Animator>();
        rigidbody = this.transform.GetComponent<Rigidbody>();
        collider = this.transform.GetComponent<SphereCollider>();
        bodyMaterial = this.transform.GetChild(1).GetComponent<Renderer>().materials[0];
        faceMaterial = this.transform.GetChild(1).GetComponent<Renderer>().materials[1];

        Initialized();
    }

    private void Initialized()
    {
        SetEndPos(StageControl.Instance.stageInfo[GameData.Player.nowStage].GoalPOS.transform);
        this.SetState(STATE.MOVE);
        normalFaceNum = Random.Range(0, face.NormalFace.Length);
    }

    void Update()
    {
        if(this.State != STATE.DIE)
        {
            Move();
            // Attack();
        }
    }

    void Idle()
    {
        navAgent.isStopped = true;
        navAgent.updateRotation = false;

        animator.SetFloat("MoveBlend", 0);
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
        Vector3 position = animator.rootPosition;
        position.y = navAgent.nextPosition.y;
        transform.position = position;
        navAgent.nextPosition = transform.position;
    }

    void Attack()
    {
        SetFace(face.attackFace);
    }

    public void DamagedHP(float _damage)
    {
        this.HP -= (int)_damage;
        animator.SetTrigger("Damaged");

        StartCoroutine(DamageEffect());

        if (this.HP <= 0)
        {
            Die();
        }
    }

    IEnumerator DamageEffect()
    {
        Color defaultColor = bodyMaterial.color;
        bodyMaterial.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        bodyMaterial.color = defaultColor;
    }

    void Die()
    {
        SetFace(face.damageFace);
        this.SetState(STATE.DIE);

        this.rigidbody.velocity = Vector3.zero;
        this.navAgent.Stop();
        this.rigidbody.useGravity = false;
        this.collider.enabled = false;

        this.gameObject.tag = "Untagged";

        animator.SetTrigger("Dead");
    }

    void SetFace(Texture _texture)
    {
        faceMaterial.SetTexture("_MainTex", _texture);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Goal")
        {
            DestroySlime();
        }
    }

    public void DestroySlime()
    {
        Destroy(this.gameObject);
    }
}
