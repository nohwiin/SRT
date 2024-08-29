using UnityEngine;

public class AuthController : MonoBehaviour
{
    private AuthManager authManager;

    private void Awake()
    {
        authManager = ManagerFactory.GetAuthManager();
    }

    /// <summary>
    /// 로그인을 시도합니다.
    /// </summary>
    public void SignIn(string username, string password, bool rememberId, bool autoSignIn)
    {
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
                Debug.Log("SignIn successful");
                Debug.Log(result);
                Debug.Log(response);

                if (rememberId || autoSignIn)
                {
                    authManager.SaveUserInfo(username, password, autoSignIn);
                }
            }
            else
            {
                Debug.LogError("SignIn failed");
                Debug.Log(result);
                Debug.Log(response);
            }
        });
    }

    /// <summary>
    /// 로그아웃을 시도합니다.
    /// </summary>

    public void SignOut()
    {
        authManager.SignOut((result, response) =>
        {
            if (result)
            {
                Debug.Log("SignOut successful");
                Debug.Log(result);
                Debug.Log(response);
            }
            else
            {
                Debug.LogError("SignOut failed");
                Debug.Log(result);
                Debug.Log(response);
            }
        });
    }

    /// <summary>
    /// 자동 로그인을 시도합니다.
    /// </summary>
    public void AutoSignInIfNeeded()
    {
        var signInType = authManager.TryGetLastSignInType();
        var isAutoSignInNeeded = authManager.TryGetSavedUserInfo((SignInType)signInType, out var username, out var password);
        if (isAutoSignInNeeded)
        {
            User user = new(username, password);
            authManager.SignIn(user, (result, response) =>
            {
                if (result)
                {
                    Debug.Log("Login successful");
                    Debug.Log(result);
                    Debug.Log(response);
                }
                else
                {
                    Debug.LogError("Login failed");
                    Debug.Log(result);
                    Debug.Log(response);
                }
            });
        }
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
