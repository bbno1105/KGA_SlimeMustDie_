using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private PlayerInfo PInfo;

    [Header("캐릭터")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform character;
    Animator anim;
    // TODO : 나중에 Animator.StringToHash 로 최적화하기


    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    float blendSpeed;

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

    void SetAnimation()
    {
        switch (PInfo.State)
        {
            case CharacterInfo.STATE.IDLE:
                if (blendSpeed > 0)
                {
                    blendSpeed -= 2f * Time.deltaTime;
                }
                else
                {
                    blendSpeed = 0;
                }
                break;
            case CharacterInfo.STATE.MOVE:
                if(PInfo.IsRun)
                {
                    if (blendSpeed < 1f)
                    {
                        blendSpeed += Time.deltaTime;
                    }
                    else
                    {
                        blendSpeed = 1f;
                    }
                }
                else
                {
                    if (blendSpeed < 0.4f)
                    {
                        blendSpeed += Time.deltaTime;
                    }
                    else if (blendSpeed > 0.6f)
                    {
                        blendSpeed -= Time.deltaTime;
                    }
                    else
                    {
                        blendSpeed = 0.5f;
                    }
                }
                break;
            case CharacterInfo.STATE.DIE:
                break;
            default:
                break;
        }
        anim.SetFloat("Blend", blendSpeed);
        anim.SetBool("isAttack", PInfo.IsAttack);
    }
}
