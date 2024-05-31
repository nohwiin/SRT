using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuthController : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField usernameInputField;
    [SerializeField]
    private TMP_InputField passwordInputField;

    [SerializeField]
    private Toggle rememberIdToggle;
    [SerializeField]
    private Toggle autoSignInToggle;

    [SerializeField]
    private Button signInButton;

    private AuthManager authManager;

    private void Awake()
    {
        authManager = AuthManager.Instance;
    }

    private void Start()
    {
        signInButton.onClick.AddListener(OnSignInButtonClicked);
    }

    /// <summary>
    /// 로그인 버튼 클릭 이벤트를 처리합니다.
    /// </summary>
    private void OnSignInButtonClicked()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;
        
        authManager.SignIn(username, password, result =>
        {
            if (result)
            {
                // 로그인 성공 처리
                Debug.Log("Login successful");
                if (rememberIdToggle.isOn)
                {
                    authManager.SaveUserInfo(username, password); // 비밀번호 저장은 해싱 또는 암호화를 사용하는 것이 좋습니다.
                }
            }
            else
            {
                // 로그인 실패 처리
                Debug.Log("Login failed");
            }
        });
    }
}
