using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : SingletonMonoBehaviour<GameData> // �̱��� �ٲٱ�
{
    public static PlayerData Player = null;

    void Start()
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
        }

        // ���� ������ �ҷ�����
        OpenData();
    }

    public void OpenData()
    {

    }
}
