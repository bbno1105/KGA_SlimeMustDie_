using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIControl : SingletonMonoBehaviour<UIControl>
{
    [SerializeField] TextMeshProUGUI Gold;

    public void RefreshGoldUI()
    {
        Gold.text = GameData.Instance.Player.gold.ToString();
    }
}
