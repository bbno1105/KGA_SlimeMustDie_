using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : CharacterInfo
{
    // 플레이어만 가지고 있는 상태
    [field: SerializeField] public bool IsRun { get; private set; }
    public void SetRunState(bool _isRun) { this.IsRun = _isRun; }

    [field : SerializeField] public bool IsJump { get; private set; }
    public void SetJumpState(bool _isJump) { this.IsJump = _isJump; }

    public Rigidbody rigid;
    public Animator anim;

    public float walkSpeed;
    public float runSpeed;
    public float jumpPower;
    public float moveblendSpeed;
    public float jumpblendSpeed;

    public void DamagedHP(float _damage)
    {
        this.SetHP(this.HP - (int)_damage);
        this.anim.SetTrigger(AnimString.Damaged);

        if (this.HP <= 0)
        {
            this.State = STATE.DIE;
            anim.SetTrigger(AnimString.Dead);
        }
    }

    void Start()
    {
        rigid = this.gameObject.GetComponent<Rigidbody>();
        anim = this.gameObject.GetComponentInChildren<Animator>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            this.SetJumpState(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dead")
        {
            PlayerControl.Instance.Die();
        }
    }
}
