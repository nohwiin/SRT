using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    public InputField usernameInputField;
    public InputField passwordInputField;

    private LoginController loginController;

    private void Start()
    {
        // LoginController 스크립트를 가져옴
        loginController = FindObjectOfType<LoginController>();

        // '로그인' 버튼에 클릭 이벤트 추가
        Button loginButton = GetComponent<Button>();
        loginButton.onClick.AddListener(OnLoginButtonClick);
    }

    private void OnLoginButtonClick()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        loginController.AttemptLogin(username, password);
    }
}
