using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;

public class MiddleSectionPanel : LobbyPanelBase
{
    [Header("MiddleSectionPanel vars")]
    [SerializeField] Button joinRandomRoomButton;
    [SerializeField] Button joinRoomByArgButton;
    [SerializeField] Button createRoomButton;

    [SerializeField] TMP_InputField joinRoomByArgInputField;
    [SerializeField] TMP_InputField createRoomInputField;
    NetworkRunnerController networkRunnerController;

    public override void InitPanel(LobbyUIManager uIManager)
    {
        base.InitPanel(uIManager);


        networkRunnerController = GlobalManager.Instance.networkRunnerController;
        joinRandomRoomButton.onClick.AddListener(JoinRandomRoom);
        joinRoomByArgButton.onClick.AddListener(() => CreateRoom(GameMode.Client, joinRoomByArgInputField.text));
        createRoomButton.onClick.AddListener(() => CreateRoom(GameMode.Host, createRoomInputField.text));
    }

    private void CreateRoom(GameMode mode, string field)
    {
        if (field.Length >= 2)
        {
            Debug.Log("CreateRoom: -----------------" + mode + " -----------------");
            networkRunnerController.StartGame(mode, field);
        }
    }

    private void JoinRandomRoom()
    {
        Debug.Log("CreateRoom: -----------------JoinRandomRoom -----------------");
        networkRunnerController.StartGame(GameMode.AutoHostOrClient, string.Empty);
    }
}
