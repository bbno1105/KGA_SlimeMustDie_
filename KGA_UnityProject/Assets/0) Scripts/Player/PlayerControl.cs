using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : SingletonMonoBehaviour<PlayerControl>
{
    [SerializeField] PlayerInfo PInfo;

    [Header("캐릭터")]
    [SerializeField] Transform player;
    [SerializeField] Transform character;

    [Header("카메라")]
    [SerializeField] private Transform camArm;

    Vector2 moveInput;

    public bool canAttack;

    public void Initalize()
    {
        PInfo.gameObject.transform.position = StageControl.Instance.stageInfo[GameData.Instance.Player.nowStage].PlayerStartPOS.transform.position;
        PInfo.rigid.isKinematic = false;
        PInfo.anim.Rebind();

        PInfo.SetHP(PInfo.MaxHP);
        PInfo.SetState(CharacterInfo.STATE.IDLE);

    }

    void Update()
    {
        if(PInfo.State == CharacterInfo.STATE.DIE)
        {
            // 게임오버? 부활?
        }
        else
        {
            InputKey();
            SetState();
            SetAnimation();
        }
    }

    void InputKey()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetMouseButton(0) && canAttack)
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
            float moveSpeed = PInfo.walkSpeed;
            if(PInfo.IsRun)
            {
                moveSpeed = PInfo.runSpeed;
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
        PInfo.rigid.velocity = Vector3.up * PInfo.jumpPower;
        PInfo.anim.SetTrigger(AnimString.Jump);
    }


    void SetAnimation()
    {
        if (PInfo.IsJump)
        {
            JumpBlendAnition();
            PInfo.anim.SetFloat(AnimString.JumpBlend, PInfo.jumpblendSpeed);
        }
        else
        {
            MoveBlendAnition();
            PInfo.anim.SetFloat(AnimString.MoveBlend, PInfo.moveblendSpeed);
        }

        PInfo.anim.SetBool(AnimString.isJump, PInfo.IsJump);
        PInfo.anim.SetBool(AnimString.isAttack, PInfo.IsAttack);
    }

    void MoveBlendAnition()
    {
        switch (PInfo.State)
        {
            case CharacterInfo.STATE.IDLE:
                {
                    if (PInfo.moveblendSpeed > 0)
                    {
                        PInfo.moveblendSpeed -= 2f * Time.deltaTime;
                    }
                    else
                    {
                        PInfo.moveblendSpeed = 0;
                    }
                }
                break;
            case CharacterInfo.STATE.MOVE:
                if (PInfo.IsRun)
                {
                    if (PInfo.moveblendSpeed < 1f)
                    {
                        PInfo.moveblendSpeed += Time.deltaTime;
                    }
                    else
                    {
                        PInfo.moveblendSpeed = 1f;
                    }
                }
                else
                {
                    if (PInfo.moveblendSpeed < 0.4f)
                    {
                        PInfo.moveblendSpeed += Time.deltaTime;
                    }
                    else if (PInfo.moveblendSpeed > 0.6f)
                    {
                        PInfo.moveblendSpeed -= Time.deltaTime;
                    }
                    else
                    {
                        PInfo.moveblendSpeed = 0.5f;
                    }
                }
                break;
            default:
                break;
        }
    }

    void JumpBlendAnition()
    {
        if (PInfo.rigid.velocity.y > 1)
        {
            if (PInfo.jumpblendSpeed < -0.5f)
            {
                PInfo.jumpblendSpeed += Time.deltaTime;
            }
            else
            {
                PInfo.jumpblendSpeed = -0.5f;
            }
        }
        else if (PInfo.rigid.velocity.y > 0)
        {
            if (PInfo.jumpblendSpeed < 0)
            {
                PInfo.jumpblendSpeed += Time.deltaTime;
            }
            else
            {
                PInfo.jumpblendSpeed = 0;
            }
        }
        else if (PInfo.rigid.velocity.y < -1)
        {
            if (PInfo.jumpblendSpeed < 1f)
            {
                PInfo.jumpblendSpeed += Time.deltaTime;
            }
            else
            {
                PInfo.jumpblendSpeed = 1f;
            }
        }
    }

    public void Die()
    {
        PInfo.anim.SetTrigger(AnimString.Dead);
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        PInfo.SetState(CharacterInfo.STATE.DIE);
        PInfo.rigid.isKinematic = true;

        yield return new WaitForSeconds(5);

        Initalize();
    }
}
