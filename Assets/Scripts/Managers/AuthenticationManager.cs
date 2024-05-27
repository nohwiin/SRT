using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AuthenticationManager : MonoBehaviour
{
    private static AuthenticationManager _instance;
    public static AuthenticationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AuthenticationManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(AuthenticationManager).Name;
                    _instance = obj.AddComponent<AuthenticationManager>();
                }
            }
            return _instance;
        }
    }

    private bool verbose = true;

    private UnityWebRequest CreateRequest(string url, WWWForm form)
    {
        var request = UnityWebRequest.Post(url, form);
        request.SetRequestHeader(Constants.HEADER_USER_AGENT, Constants.DEFAULT_USER_AGENT);
        request.SetRequestHeader(Constants.HEADER_ACCEPT, Constants.HEADER_ACCEPT);
        return request;
    }

    private void Log(string message)
    {
        if (verbose)
        {
            Debug.Log("[*] " + message);
        }
    }

    public IEnumerator Login(User user, Action<bool, string> callback)
    {
        string loginType;
        if (Constants.EMAIL_REGEX.IsMatch(user.Username))
        {
            loginType = Constants.LOGIN_TYPES_EMAIL;
        }
        else if (Constants.PHONE_NUMBER_REGEX.IsMatch(user.Username))
        {
            loginType = Constants.LOGIN_TYPES_PHONE_NUMBER;
            user.Username = user.Username.Replace("-", "");
        }
        else
        {
            loginType = Constants.LOGIN_TYPES_MEMBERSHIP_ID;
        }

        WWWForm form = new WWWForm();
        form.AddField("auto", "Y");
        form.AddField("check", "Y");
        form.AddField("page", "menu");
        form.AddField("deviceKey", "-");
        form.AddField("customerYn", "");
        form.AddField("login_referer", Constants.API_ENDPOINTS_MAIN);
        form.AddField("srchDvCd", loginType);
        form.AddField("srchDvNm", user.Username);
        form.AddField("hmpgPwdCphd", user.Password);

        yield return NetworkManager.PostRequest(Constants.API_ENDPOINTS_LOGIN, form, (UnityWebRequest request) =>
        {
            if (request.result != UnityWebRequest.Result.Success)
            {
                Log(request.error);
                callback(false, request.error);
            }
            else
            {
                string responseText = request.downloadHandler.text;
                Log(responseText);

                // Handle specific error messages
                if (responseText.Contains("존재하지않는 회원입니다") || responseText.Contains("비밀번호 오류"))
                {
                    callback(false, responseText);
                }
                else if (responseText.Contains("Your IP Address Blocked due to abnormal access."))
                {
                    callback(false, "Your IP Address Blocked due to abnormal access.");
                }
                else
                {
                    callback(true, responseText);
                }
            }
        });
    }
}
