using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [field : SerializeField] public bool[] IsTrapOn { get; private set; }
    GameObject[] trap;

    void Start()
    {
        IsTrapOn = new bool[6];
        trap = new GameObject[6];
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

    public void SetTrap(int _trapIndex, GameObject gameObject)
    {
        IsTrapOn[(int)_trapIndex] = true;
        trap[_trapIndex] = gameObject;
    }

    public GameObject ClearTrap(int _trapIndex)
    {
        IsTrapOn[(int)_trapIndex] = false;
        return trap[_trapIndex];
    }
}
