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
    Material faceMaterial;

    void Start()
    {
        faceMaterial = this.transform.GetChild(1).GetComponent<Renderer>().materials[1];
        animator = this.transform.GetComponentInChildren<Animator>();
        SetEndPos(StageControl.Instance.stageInfo[GameData.Player.nowStage].GoalPOS.transform);
        this.SetState(STATE.MOVE);

        normalFaceNum = Random.Range(0, face.NormalFace.Length);

    }

    void Update()
    {
        switch (this.State)
        {
            case STATE.IDLE:
                Idle();
                break;
            case STATE.MOVE:
                Move();
                break;
            case STATE.DIE:
                break;
            default:
                break;
        }

        if(this.State != STATE.DIE)
        {
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

    void Die()
    {
        if(HP <= 0)
        {
            SetFace(face.damageFace);
            this.SetState(STATE.DIE);
        }
    }

    void SetFace(Texture _texture)
    {
        faceMaterial.SetTexture("_MainTex", _texture);
    }

    void SetAnimation()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Goal")
        {
            UnityEngine.Debug.Log("µé¾î¿È");
            Destroy(this.gameObject);
        }
    }
}
