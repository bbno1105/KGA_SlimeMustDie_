using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : CharacterInfo
{
    // 플레이어만 가지고 있는 상태
    public bool IsRun { get; private set; }
    public void SetRunState(bool _isRun) { this.IsRun = _isRun; }

    public bool IsJump { get; private set; }
    public void SetJumpState(bool _isJump) { this.IsJump = _isJump; }
}
