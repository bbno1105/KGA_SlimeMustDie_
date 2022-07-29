using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [field : SerializeField] public bool[] IsTrapOn { get; private set; }

    void Start()
    {
        IsTrapOn = new bool[6];
    }
    //public enum TRAPINDEX
    //{
    //    UP,
    //    DOWN,
    //    FRONT,
    //    BACK,
    //    LEFT,
    //    RIGHT
    //}
    //public TRAPINDEX trapIndex;

    public void SetTrap(int _trapIndex)
    {
        IsTrapOn[(int)_trapIndex] = true;
    }

    public void ClearTrap(int _trapIndex)
    {
        IsTrapOn[(int)_trapIndex] = false;
    }
}
