using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // �÷��̾� ������
    public int nowStage = 0;

    int _glod;
    public int gold { get; private set; }

    public bool UseGold(int _useGold)
    {
        if(gold < _useGold)
        {
            return false;
        }

        gold -= _useGold;
        UIControl.Instance.RefreshGoldUI();
        return true;
    }

    public void AddGold(int _addGold)
    {
        gold += _addGold;
        UIControl.Instance.RefreshGoldUI();
    }

    public void SetGold(int _Gold)
    {
        gold += _Gold;
        UIControl.Instance.RefreshGoldUI();
    }

    // �ʱ�ȭ
    public void Initialize()
    {
        nowStage = 0;
        gold = 2000;
    }

    // �� ����
}
