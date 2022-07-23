using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : SingletonMonoBehaviour<GameData> // 싱글톤 바꾸기
{
    public static PlayerData Player = null;

    void Start()
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
            Player = new PlayerData();
        }

        // 게임 데이터 불러오기
        OpenData();
    }

    public void OpenData()
    {

    }
}
