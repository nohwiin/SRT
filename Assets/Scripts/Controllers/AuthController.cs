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
        authManager = ManagerFactory.GetAuthManager();
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

        if (ValidateCredentials(username, password) == false)
        {
            Debug.LogError("Invalid username or password format.");
            return;
        }

        User user = new(username, password);

        authManager.SignIn(user, (result, response) =>
        {
            if (result)
            {
                // 로그인 성공 처리
                Debug.Log("Login successful");
                Debug.Log(response);
                
                if (rememberIdToggle.isOn)
                {
                    authManager.SaveUserInfo(username, password); // 비밀번호 저장은 해싱 또는 암호화를 사용하는 것이 좋습니다.
                }
            }
            else
            {
                // 로그인 실패 처리
                Debug.LogError("Login failed");
            }
        });
    }

    /// <summary>
    /// 사용자 자격 증명을 검증합니다.
    /// </summary>
    /// <param name="username">사용자 이름</param>
    /// <param name="password">사용자 비밀번호</param>
    /// <returns>자격 증명이 유효한지 여부</returns>
    private bool ValidateCredentials(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return false;
        }

        return true;
    }
}
