using UnityEngine.Networking;
using System.Collections;
using UnityEngine;

public static class NetworkManager
{
    public static IEnumerator PostRequest(string url, WWWForm form, System.Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();
            callback(www);
        }
    }
}
