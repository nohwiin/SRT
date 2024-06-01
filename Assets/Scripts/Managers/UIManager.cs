using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private GameObject authUI;
    [SerializeField]
    private GameObject searchUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        AuthManager.Instance.RegisterOnSignInSuccess(OnSignInEvent);
        ShowUI(authUI);
    }

    private void OnDestroy()
    {
        AuthManager.Instance.UnregisterOnSignInSuccess(OnSignInEvent);
    }

    /// <summary>
    /// 특정 UI를 활성화하고 나머지 UI를 비활성화합니다.
    /// </summary>
    /// <param name="uiToActivate">활성화할 UI</param>
    public void ShowUI(GameObject uiToActivate)
    {
        authUI.SetActive(false);
        searchUI.SetActive(false);

        uiToActivate.SetActive(true);
    }

    /// <summary>
    /// SignIn 성공 시 호출되는 메서드
    /// </summary>
    private void OnSignInEvent()
    {
        ShowUI(searchUI);
    }
}
