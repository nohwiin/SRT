using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public delegate void ResultDelegate<Result>(Result result);
public delegate void ResultDelegate<Result, Response>(Result result, Response response);

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
        WWWForm form = new();
        form.AddField("auto", "Y");
        form.AddField("check", "Y");
        form.AddField("page", "menu");
        form.AddField("deviceKey", "-");
        form.AddField("customerYn", "");
        form.AddField("login_referer", Constants.API_ENDPOINTS_MAIN);
        form.AddField("srchDvCd", (int)currentSignInType);
        form.AddField("srchDvNm", user.Username);
        form.AddField("hmpgPwdCphd", user.Password);

        using (UnityWebRequest request = UnityWebRequest.Post(Constants.API_ENDPOINTS_LOGIN, form))
        {
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
                    listener.Invoke(true, responseText);
                }
            }
        }
    }

    /// <summary>
    /// 사용자 정보를 저장합니다.
    /// </summary>
    /// <param name="id">사용자 ID</param>
    /// <param name="pw">사용자 비밀번호</param>
    public void SaveUserInfo(string id, string pw)
    {
        PlayerPrefs.SetInt("lastSignInType", (int)currentSignInType);
        PlayerPrefs.SetString($"{currentSignInType}_ID", id);
        PlayerPrefs.SetString($"{currentSignInType}_PW", pw);
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
        SetCurrentSignInType(toggleText);
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
        return PlayerPrefs.GetInt("lastSignInType", (int)SignInType.MEMBERSHIP_ID);
    }

    /// <summary>
    /// 현재 SignInType을 설정합니다.
    /// </summary>
    /// <param name="toggleText">토글 텍스트를 기반으로 SignInType을 설정합니다.</param>
    public void SetCurrentSignInType(string toggleText)
    {
        currentSignInType = ConvertTextToSignInType(toggleText);
    }

    /// <summary>
    /// 텍스트를 SignInType으로 변환합니다.
    /// </summary>
    /// <param name="toggleText">토글 텍스트</param>
    /// <returns>SignInType 열거형 값</returns>
    private SignInType ConvertTextToSignInType(string toggleText)
    {
        return toggleText switch
        {
            "회원번호" => SignInType.MEMBERSHIP_ID,
            "이메일주소" => SignInType.EMAIL,
            "휴대폰번호" => SignInType.PHONE_NUMBER,
            _ => throw new ArgumentException($"Invalid toggle text: {toggleText}")
        };
    }
}
