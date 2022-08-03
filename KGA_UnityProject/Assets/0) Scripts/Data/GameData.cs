using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : SingletonMonoBehaviour<GameData> // 싱글톤 바꾸기
{
    public PlayerData Player = null;

    void Awake()
    {
        LoadData();
    }

    public void SaveData()
    {
        // 플레이어 데이터 저장하기
    }

    public void LoadData()
    {
        // 플레이어 데이터 불러오기
        if(Player == null)
        {
            UnityEngine.Debug.Log("들어옴???");
            Player = new PlayerData();
            Player.Initialize();
        }

        // 게임 데이터 불러오기
        OpenData();

        // UI 갱신
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
