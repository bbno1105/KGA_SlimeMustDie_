using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : SingletonMonoBehaviour<GameData> // �̱��� �ٲٱ�
{
    public PlayerData Player = null;

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
            UnityEngine.Debug.Log("����???");
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
        Player.SetGold(StageControl.Instance.stageInfo[Player.nowStage].StartGold);
    }

    public void RefreshUI()
    {
        UIControl.Instance.RefreshGoldUI();
    }
}
