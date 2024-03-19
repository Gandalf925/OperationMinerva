using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCanvasController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Button cancelButton;
    NetworkRunnerController networkRunnerController;

    private void Start()
    {
        networkRunnerController = GlobalManager.Instance.networkRunnerController;
        networkRunnerController.OnStartedRunnerConnection += OnStartedRunnerConnection;
        networkRunnerController.OnPlayerJoinedSucessfully += OnPlayerJoinedSucessfully;


        cancelButton.onClick.AddListener(networkRunnerController.ShutDownRunner);
        this.gameObject.SetActive(false);
    }

    private void OnStartedRunnerConnection()
    {
        this.gameObject.SetActive(true);
        const string CLIP_Name = "in";
        StartCoroutine(Utils.PlayAnimAndSetStateWhenFinished(this.gameObject, animator, CLIP_Name));
    }

    private void OnPlayerJoinedSucessfully()
    {
        const string CLIP_Name = "out";
        StartCoroutine(Utils.PlayAnimAndSetStateWhenFinished(this.gameObject, animator, CLIP_Name, false));
    }

    private void CancelRequest()
    {
        throw new NotImplementedException();
    }

    private void OnDestroy()
    {
        networkRunnerController.OnStartedRunnerConnection -= OnStartedRunnerConnection;
        networkRunnerController.OnPlayerJoinedSucessfully -= OnPlayerJoinedSucessfully;
    }
}
