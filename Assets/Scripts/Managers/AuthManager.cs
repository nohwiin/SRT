using System;
using System.Collections;
using UnityEngine;

public delegate void ResultDelegate<Result>(Result result);

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
    /// <param name="id">사용자 ID</param>
    /// <param name="pw">사용자 비밀번호</param>
    /// <param name="listener">로그인 결과를 수신할 델리게이트</param>
    public void SignIn(string id, string pw, ResultDelegate<bool> listener)
    {
        StartCoroutine(SignInAsync(id, pw, listener));
    }

    /// <summary>
    /// 비동기 로그인 프로세스
    /// </summary>
    /// <param name="id">사용자 ID</param>
    /// <param name="pw">사용자 비밀번호</param>
    /// <param name="listener">로그인 결과를 수신할 델리게이트</param>
    /// <returns>코루틴</returns>
    private IEnumerator SignInAsync(string id, string pw, ResultDelegate<bool> listener)
    {
        // 실제 인증 로직으로 대체해야 합니다.
        bool result = true;
        listener.Invoke(result);
        yield break;
    }

    /// <summary>
    /// 사용자 정보를 저장합니다.
    /// </summary>
    /// <param name="id">사용자 ID</param>
    /// <param name="pw">사용자 비밀번호</param>
    public void SaveUserInfo(string id, string pw)
    {
        PlayerPrefs.SetString($"{currentSignInType}_ID", id);
        PlayerPrefs.SetString($"{currentSignInType}_PW", pw); // 비밀번호는 해싱 등을 통해 저장해야 합니다.
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
        currentSignInType = ConvertTextToSignInType(toggleText);
        rememberedId = PlayerPrefs.GetString($"{currentSignInType}_ID", "");
        return !string.IsNullOrEmpty(rememberedId);
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
