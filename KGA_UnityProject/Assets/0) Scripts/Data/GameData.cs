using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : SingletonMonoBehaviour<GameData> // �̱��� �ٲٱ�
{
    public static PlayerData Player = null;

    void Awake()
    {
        LoadData();
    }

    public void SaveData()
    {
        // �÷��̾� ������ �����ϱ�
    }

    public void LoadData()
    {
        // �÷��̾� ������ �ҷ�����
        if(Player == null)
        {
            Player = new PlayerData();
            Player.Initialize();
        }

        // ���� ������ �ҷ�����
        OpenData();

        // UI ����
        RefreshUI();
    }

    public void OpenData()
    {
        
    }

    public void RefreshUI()
    {
        UIControl.Instance.RefreshGoldUI();
    }
}
