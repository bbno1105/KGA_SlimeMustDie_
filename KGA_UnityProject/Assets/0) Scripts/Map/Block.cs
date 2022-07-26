using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [field : SerializeField] public bool IsTrapOn { get; private set; }

    public void SetTrap()
    {
        IsTrapOn = true;
    }

    public void DelTrap()
    {
        IsTrapOn = false;
    }
}
