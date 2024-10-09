using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using UnityEngine;
using UnityEngine.Networking;

public enum SignInType
{
    MEMBERSHIP_ID = 1,
    EMAIL,
    PHONE_NUMBER
}

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance { get; private set; }

    private SignInType currentSignInType;
    private event Action OnSignInEvent = delegate { };
    private event Action OnSignOutEvent = delegate { };

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

    /// <summary>
    /// SignIn 성공 이벤트에 콜백을 등록합니다.
    /// </summary>
    /// <param name="callback">등록할 콜백 함수</param>
    public void RegisterOnSignInSuccess(Action callback)
    {
        OnSignInEvent += callback;
    }

    /// <summary>
    /// SignIn 성공 이벤트에서 콜백을 제거합니다.
    /// </summary>
    /// <param name="callback">제거할 콜백 함수</param>
    public void UnregisterOnSignInSuccess(Action callback)
    {
        OnSignInEvent -= callback;
    }

    /// <summary>
    /// SignOut 성공 이벤트에 콜백을 등록합니다.
    /// </summary>
    /// <param name="callback">등록할 콜백 함수</param>
    public void RegisterOnSignOutSuccess(Action callback)
    {
        OnSignOutEvent += callback;
    }

    /// <summary>
    /// SignOut 성공 이벤트에서 콜백을 제거합니다.
    /// </summary>
    /// <param name="callback">제거할 콜백 함수</param>
    public void UnregisterOnSignOutSuccess(Action callback)
    {
        OnSignOutEvent -= callback;
    }

    /// <summary>
    /// 사용자를 로그인합니다.
    /// </summary>
    /// <param name="user">사용자 객체 (ID 및 비밀번호 포함)</param>
    /// <param name="listener">로그인 결과를 수신할 델리게이트</param>
    public void SignIn(User user, ResultDelegate<bool, string> listener)
    {
        if (user == null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
        {
            listener.Invoke(false, "Invalid user credentials");
            return;
        }
        
        StartCoroutine(SignInAsync(user, listener));
    }

    /// <summary>
    /// 비동기 로그인 프로세스
    /// </summary>
    /// <param name="user">사용자 객체 (ID 및 비밀번호 포함)</param>
    /// <param name="listener">로그인 결과를 수신할 델리게이트</param>
    /// <returns>코루틴</returns>
    private IEnumerator SignInAsync(User user, ResultDelegate<bool, string> listener)
    {
        var signInRequestBody = new Dictionary<string, string>
        {
            { "auto", "Y" },
            { "check", "Y" },
            { "page", "menu" },
            { "deviceKey", "-" },
            { "customerYn", "" },
            { "login_referer", Constants.API_ENDPOINTS_MAIN },
            { "srchDvCd", ((int)currentSignInType).ToString() },
            { "srchDvNm", user.Username },
            { "hmpgPwdCphd", HttpUtility.UrlEncode(user.Password) }
        };

        using UnityWebRequest request = UnityWebRequest.Post(Constants.API_ENDPOINTS_LOGIN, signInRequestBody);
        request.SetRequestHeader("User-Agent", Constants.DEFAULT_HEADERS_USER_AGENT);
        request.SetRequestHeader("Accept", Constants.DEFAULT_HEADERS_ACCEPT);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            listener.Invoke(false, request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;

            if (responseText.Contains("존재하지않는 회원입니다") || responseText.Contains("비밀번호 오류"))
            {
                listener.Invoke(false, responseText);
            }
            else if (responseText.Contains("Your IP Address Blocked due to abnormal access."))
            {
                listener.Invoke(false, "Your IP Address Blocked due to abnormal access.");
            }
            else
            {
                OnSignInEvent?.Invoke();
                listener.Invoke(true, responseText);
            }
        }
    }

    /// <summary>
    /// 사용자를 로그아웃합니다.
    /// </summary>
    /// <param name="listener">로그아웃 결과를 수신할 델리게이트</param>
    public void SignOut(ResultDelegate<bool, string> listener)
    {
        StartCoroutine(SignOutAsync(listener));
    }

    /// <summary>
    /// 비동기 로그아웃 프로세스
    /// </summary>
    /// <param name="listener">로그아웃 결과를 수신할 델리게이트</param>
    /// <returns>코루틴</returns>
    private IEnumerator SignOutAsync(ResultDelegate<bool, string> listener)
    {
        var signOutRequestBody = new Dictionary<string, string>();

        using UnityWebRequest request = UnityWebRequest.Post(Constants.API_ENDPOINTS_LOGOUT, signOutRequestBody);
        request.SetRequestHeader("User-Agent", Constants.DEFAULT_HEADERS_USER_AGENT);
        request.SetRequestHeader("Accept", Constants.DEFAULT_HEADERS_ACCEPT);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;

            listener.Invoke(false, responseText);
            yield break;
        }

        ClearUserInfo();
        OnSignOutEvent?.Invoke();
        listener.Invoke(true, default);
    }

    /// <summary>
    /// 사용자 정보를 저장합니다.
    /// </summary>
    /// <param name="id">사용자 ID</param>
    /// <param name="pw">사용자 비밀번호</param>
    /// <param name="autoSignIn">자동 로그인 사용 여부</param>
    public void SaveUserInfo(string id, string pw, bool autoSignIn)
    {
        PlayerPrefs.SetInt("lastSignInType", (int)currentSignInType);
        PlayerPrefs.SetString($"{currentSignInType}_ID", id);
        PlayerPrefs.SetString($"{currentSignInType}_PW", pw);
        PlayerPrefs.SetString($"{currentSignInType}_AUTO_SIGNIN", autoSignIn.ToString());
        PlayerPrefs.Save();
    }

    private void ClearUserInfo()
    {
        var lastSignInTypeAsInt = TryGetLastSignInType();
        var lastSignInType = (SignInType)lastSignInTypeAsInt;

        PlayerPrefs.SetString($"{lastSignInType}_PW", string.Empty);
        PlayerPrefs.SetString($"{lastSignInType}_AUTO_SIGNIN", false.ToString());
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 저장된 ID를 가져옵니다.
    /// </summary>
    /// <param name="toggleText">토글 텍스트</param>
    /// <param name="rememberedId">저장된 ID 문자열</param>
    /// <returns>저장된 ID가 있는지 여부</returns>
    public bool TryGetRememberedId(string toggleText, out string rememberedId)
    {
        rememberedId = PlayerPrefs.GetString($"{currentSignInType}_ID", "");
        return !string.IsNullOrEmpty(rememberedId);
    }

    /// <summary>
    /// 마지막으로 사용된 SignInType을 가져옵니다.
    /// </summary>
    /// <returns>
    /// 마지막으로 사용된 SignInType의 정수 값.
    /// 기본값은 1 (SignInType.MEMBERSHIP_ID)에 해당합니다.
    /// </returns>
    public int TryGetLastSignInType()
    {
        var lastSignInType = PlayerPrefs.GetInt("lastSignInType", (int)SignInType.MEMBERSHIP_ID);
        currentSignInType = (SignInType)lastSignInType;

        return lastSignInType;
    }

    /// <summary>
    /// 저장된 사용자 정보를 가져옵니다.
    /// </summary>
    /// <param name="targetType">가져올 사용자 정보의 SignInType</param>
    /// <param name="username">가져온 사용자 이름</param>
    /// <param name="password">가져온 사용자 비밀번호</param>
    /// <returns>자동 로그인을 수행 필요 여부</returns>
    public bool TryGetSavedUserInfo(SignInType targetType, out string username, out string password)
    {
        username = PlayerPrefs.GetString($"{targetType}_ID", "");
        password = PlayerPrefs.GetString($"{targetType}_PW", "");
        if (bool.TryParse(PlayerPrefs.GetString($"{targetType}_AUTO_SIGNIN", "false"), out bool autoSignInEnabled) == false)
        {
            autoSignInEnabled = false;
        }

        return autoSignInEnabled && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
    }
}
