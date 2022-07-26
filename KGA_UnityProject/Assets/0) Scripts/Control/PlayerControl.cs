using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private PlayerInfo PInfo;

    [Header("캐릭터")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform character;
    Rigidbody rigid;
    Animator anim;
    // TODO : 나중에 Animator.StringToHash 로 최적화하기


    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpPower;
    float moveblendSpeed;
    float jumpblendSpeed;

    [Header("카메라")]
    [SerializeField] private Transform camArm;

    Vector2 moveInput;

    public void InitalizeInfo(int _HP, float _AttackPower, float _MoveSpeed)
    {
        PInfo.SetHP(_HP);
        PInfo.SetAttackPower(_AttackPower);
        PInfo.SetMoveSpeed(_MoveSpeed);
    }

    void Start()
    {
        rigid = player.gameObject.GetComponent<Rigidbody>();
        anim = character.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        InputKey();
        SetState();
        SetAnimation();
    }

    void InputKey()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetMouseButton(0))
        {
            PInfo.SetAttackState(true);
        }
        else
        {
            PInfo.SetAttackState(false);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            PInfo.SetRunState(true);
        }
        else
        {
            PInfo.SetRunState(false);
        }

        if(Input.GetKeyDown(KeyCode.Space) && !PInfo.IsJump)
        {
            PInfo.SetJumpState(true);
            Jump();
        }
    }

    void SetState()
    {
        PInfo.SetState(CharacterInfo.STATE.IDLE);
        if (moveInput.magnitude != 0)
        {
            PInfo.SetState(CharacterInfo.STATE.MOVE);
            float moveSpeed = this.walkSpeed;
            if(PInfo.IsRun)
            {
                moveSpeed = this.runSpeed;
            }
            PInfo.SetMoveSpeed(moveSpeed);
            Move();
        }
    }

    void Move()
    {
        Vector3 lookForward = new Vector3(camArm.forward.x, 0f, camArm.forward.z).normalized;
        Vector3 lookRight = new Vector3(camArm.right.x, 0f, camArm.right.z).normalized;
        Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

        character.forward = lookForward;
        player.transform.position += moveDir * Time.deltaTime * PInfo.MoveSpeed;
    }

    void Jump()
    {
        rigid.velocity = Vector3.up * jumpPower;
        anim.SetTrigger("Jump");
    }



    void SetAnimation()
    {
        if (PInfo.IsJump)
        {
            JumpBlendAnition();
            anim.SetFloat("JumpBlend", jumpblendSpeed);
        }
        else
        {
            MoveBlendAnition();
            anim.SetFloat("MoveBlend", moveblendSpeed);
        }

        anim.SetBool("isJump", PInfo.IsJump);
        anim.SetBool("isAttack", PInfo.IsAttack);
    }

    void MoveBlendAnition()
    {
        switch (PInfo.State)
        {
            case CharacterInfo.STATE.IDLE:
                {
                    if (moveblendSpeed > 0)
                    {
                        moveblendSpeed -= 2f * Time.deltaTime;
                    }
                    else
                    {
                        moveblendSpeed = 0;
                    }
                }
                break;
            case CharacterInfo.STATE.MOVE:
                if (PInfo.IsRun)
                {
                    if (moveblendSpeed < 1f)
                    {
                        moveblendSpeed += Time.deltaTime;
                    }
                    else
                    {
                        moveblendSpeed = 1f;
                    }
                }
                else
                {
                    if (moveblendSpeed < 0.4f)
                    {
                        moveblendSpeed += Time.deltaTime;
                    }
                    else if (moveblendSpeed > 0.6f)
                    {
                        moveblendSpeed -= Time.deltaTime;
                    }
                    else
                    {
                        moveblendSpeed = 0.5f;
                    }
                }
                break;
            default:
                break;
        }
    }

    void JumpBlendAnition()
    {
        if (rigid.velocity.y > 1)
        {
            if (jumpblendSpeed < -0.5f)
            {
                jumpblendSpeed += Time.deltaTime;
            }
            else
            {
                jumpblendSpeed = -0.5f;
            }
        }
        else if (rigid.velocity.y > 0)
        {
            if (jumpblendSpeed < 0)
            {
                jumpblendSpeed += Time.deltaTime;
            }
            else
            {
                jumpblendSpeed = 0;
            }
        }
        else if (rigid.velocity.y < -1)
        {
            if (jumpblendSpeed < 1f)
            {
                jumpblendSpeed += Time.deltaTime;
            }
            else
            {
                jumpblendSpeed = 1f;
            }
        }
    }
}
