using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterInfo : CharacterInfo
{
    [field : SerializeField] public Transform StartPos { get; private set; }
    public void SetStartPos(Transform _Pos)
    {
        this.StartPos = _Pos;
    }

    [field : SerializeField] public Transform EndPos { get; private set; }
    public void SetEndPos(Transform _Pos)
    {
        this.EndPos = _Pos;
    }
}
