using UnityEngine;

public class LoginController : MonoBehaviour
{
    public void AttemptLogin(string username, string password)
    {
        User user = new User(username, password);
        StartCoroutine(AuthenticationManager.Instance.Login(user, OnLoginResponse));
    }

    private void OnLoginResponse(bool success, string response)
    {
        if (success)
        {
            Debug.Log("Login successful!");
            // Handle successful login (e.g., transition to another scene)
        }
        else
        {
            Debug.LogError("Login failed: " + response);
            // Handle login failure (e.g., show error message)
        }
    }
}
