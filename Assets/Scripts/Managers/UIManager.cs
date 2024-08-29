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
        ManagerFactory.GetAuthManager().RegisterOnSignInSuccess(OnSignInEvent);
        ActiveUI(authUI);
    }

    private void OnDestroy()
    {
        ManagerFactory.GetAuthManager().UnregisterOnSignInSuccess(OnSignInEvent);
    }

    /// <summary>
    /// 특정 UI를 활성화하고 나머지 UI를 비활성화합니다.
    /// </summary>
    /// <param name="uiToActivate">활성화할 UI</param>
    public void ActiveUI(GameObject uiToActivate)
    {
        uiToActivate.SetActive(true);
    }

    /// <summary>
    /// SignIn 성공 시 호출되는 메서드
    /// </summary>
    private void OnSignInEvent()
    {
        authUI.GetComponent<IActiveUI>().SetActive(false);

        ActiveUI(searchUI);
    }
}
