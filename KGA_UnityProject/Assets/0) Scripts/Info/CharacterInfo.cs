using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [field : SerializeField] public int HP { get; private set; }
    public void SetHP(int _HP) { this.HP = _HP; }
    public void DamagedHP(float _damage)
    {
        this.HP -= (int)_damage;
        if(this.HP <= 0)
        {
            this.SetState(STATE.DIE);
        }
    }

    [field: SerializeField] public float AttackPower { get; private set; }
    public void SetAttackPower(float _AttackPower) { this.AttackPower = _AttackPower; }

    [field: SerializeField] public float MoveSpeed { get; private set; }
    public void SetMoveSpeed(float _MoveSpeed) { this.MoveSpeed = _MoveSpeed;  }

    public enum STATE
    {
        IDLE,
        MOVE,
        DIE
    }
    public STATE State { get; private set; }
    public void SetState(STATE _state) { this.State = _state; }

    public bool IsAttack { get; private set; }
    public void SetAttackState(bool _isAttack) { this.IsAttack = _isAttack; }
}
