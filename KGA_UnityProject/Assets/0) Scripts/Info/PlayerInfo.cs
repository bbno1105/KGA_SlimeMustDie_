using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : CharacterInfo
{
    // �÷��̾ ������ �ִ� ����
    public bool IsRun { get; private set; }
    public void SetRunState(bool _isRun) { this.IsRun = _isRun; }

    public bool IsJump { get; private set; }
    public void SetJumpState(bool _isJump) { this.IsJump = _isJump; }
}
