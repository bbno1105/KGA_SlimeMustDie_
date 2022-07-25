using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo : MonoBehaviour
{
    [field: SerializeField] public GameObject StartPOS { get; private set; }
    [field: SerializeField] public GameObject GoalPOS { get; private set; }
}
