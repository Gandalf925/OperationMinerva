using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance { get; private set; }

    [SerializeField] GameObject parentObject;
    [field: SerializeField] public NetworkRunnerController networkRunnerController { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン遷移で破棄されないようにする
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject); // すでにインスタンスが存在する場合は、この新しいインスタンスを破棄
            }
        }
    }
}
