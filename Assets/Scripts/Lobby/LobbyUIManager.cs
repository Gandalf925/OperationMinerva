using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] LobbyPanelBase[] lobbyPanels;

    void Start()
    {
        foreach (var lobby in lobbyPanels)
        {
            lobby.InitPanel(uIManager: this);
        }
    }

    public void ShowPanel(LobbyPanelBase.LobbyPanelType type)
    {
        foreach (var lobby in lobbyPanels)
        {
            if (lobby.panelType == type)
            {
                lobby.ShowPanel();
            }
        }
    }
}
