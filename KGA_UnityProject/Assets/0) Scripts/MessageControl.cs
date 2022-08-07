using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageControl : SingletonMonoBehaviour<MessageControl>
{
    [SerializeField] TextMeshProUGUI Gold;
    [SerializeField] TextMeshProUGUI Message;
    [SerializeField] TextMeshProUGUI StageMessage;
    [SerializeField] TextMeshProUGUI Stage;

    void Start()
    {
        RefreshMessage("", 0f);
    }

    public void RefreshGoldUI()
    {
        Gold.text = GameData.Instance.Player.gold.ToString();
    }

    public void RefreshMessage(string _Message, float _time = 3f, bool _isDistroy = true)
    {
        StartCoroutine(startMessage(_Message, _time, _isDistroy));
    }

    IEnumerator startMessage(string _Message, float _time, bool _isDistroy)
    {
        Message.gameObject.SetActive(true);
        Message.text = _Message;

        yield return new WaitForSeconds(_time);

        if(_isDistroy)
        {
            Message.gameObject.SetActive(false);
        }
    }

    public void RefreshStageMessage(string _Message, float _time = 3f, bool _isDistroy = true)
    {
        StartCoroutine(startStageMessage(_Message, _time, _isDistroy));
    }

    IEnumerator startStageMessage(string _Message, float _time, bool _isDistroy)
    {
        StageMessage.gameObject.SetActive(true);
        StageMessage.text = _Message;

        yield return new WaitForSeconds(_time);

        if (_isDistroy)
        {
            StageMessage.gameObject.SetActive(false);
        }
    }

    public void RefreshStageUI(string _stage)
    {
        Stage.text = _stage;
    }
}
