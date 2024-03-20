using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CreateNicknamePanel : LobbyPanelBase
{
    [Header("CreateNicknamePanel Vars")]
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button createNicknameButton;
    const int MAX_CHAR_FORNICKNAME = 2;

    public override void InitPanel(LobbyUIManager uIManager)
    {
        base.InitPanel(uIManager);

        createNicknameButton.interactable = false;
        createNicknameButton.onClick.AddListener(OnCreateNicknameButtonClicked);
        inputField.onValueChanged.AddListener(OnInputValueChanged);
    }

    private void OnInputValueChanged(string arg0)
    {
        createNicknameButton.interactable = arg0.Length >= MAX_CHAR_FORNICKNAME;
    }

    private void OnCreateNicknameButtonClicked()
    {
        string nickName = inputField.text;
        if (nickName.Length >= MAX_CHAR_FORNICKNAME)
        {
            GlobalManager.Instance.networkRunnerController.SetPlayerNickName(nickName);

            base.ClosePanel();
            lobbyUIManager.ShowPanel(LobbyPanelType.MiddleSectionPanel);
        }
    }
}
