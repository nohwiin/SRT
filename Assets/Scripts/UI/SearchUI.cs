using UnityEngine;
using UnityEngine.UI;

public class SearchUI : MonoBehaviour
{
    [SerializeField]
    private Button signOutButton;

    private AuthController authController;

    private void Awake()
    {
        authController = FindObjectOfType<AuthController>();
    }

    private void Start()
    {
        signOutButton.onClick.AddListener(OnSignOutButtonClicked);
    }

    /// <summary>
    /// 로그아웃 버튼 클릭 이벤트를 처리합니다.
    /// </summary>
    private void OnSignOutButtonClicked()
    {
        authController.SignOut();
    }
}
