using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPanelBase : MonoBehaviour
{
    [field: SerializeField, Header("LobbyPanelBase Vars")]
    public LobbyPanelType panelType { get; private set; }
    [SerializeField] Animator panelAnimator;
    protected LobbyUIManager lobbyUIManager;

    public enum LobbyPanelType
    {
        None,
        CreateNicknamePanel,
        MiddleSectionPanel,
    }

    public virtual void InitPanel(LobbyUIManager uIManager)
    {
        lobbyUIManager = uIManager;
    }

    public void ShowPanel()
    {
        this.gameObject.SetActive(true);
        const string POP_IN_CLIP_NAME = "In";
        panelAnimator.Play(POP_IN_CLIP_NAME);
    }

    public void ClosePanel()
    {
        const string POP_OUT_CLIP_NAME = "Out";
        StartCoroutine(Utils.PlayAnimAndSetStateWhenFinished(this.gameObject, panelAnimator, POP_OUT_CLIP_NAME, false));
    }
}
