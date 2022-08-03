using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo : MonoBehaviour
{
    [field: SerializeField] public int Level { get; private set; }

    [field: SerializeField] public GameObject PlayerStartPOS { get; private set; }

    [field: SerializeField] public GameObject MonsterStartPOS { get; private set; }
    [field: SerializeField] public GameObject MonsterGoalPOS { get; private set; }

    [field: SerializeField] public int StartGold { get; private set; }

}
