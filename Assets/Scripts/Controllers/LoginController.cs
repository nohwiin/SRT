using UnityEngine;

public class LoginController : MonoBehaviour
{
    public void AttemptLogin(string username, string password)
    {
        if (ValidateCredentials(username, password))
        {
            User user = new User(username, password);
            StartCoroutine(AuthenticationManager.Instance.Login(user, OnLoginResponse));
        }
        else
        {
            Debug.LogError("Invalid username or password format.");
            // Handle invalid input (e.g., show error message)
        }
    }

    private bool ValidateCredentials(string username, string password)
    {
        // Add your validation logic here (e.g., check for empty strings, valid email format, etc.)
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return false;
        }

        // Example validation for email
        if (!Constants.EMAIL_REGEX.IsMatch(username) && !Constants.PHONE_NUMBER_REGEX.IsMatch(username))
        {
            return false;
        }

        return true;
    }

    private void OnLoginResponse(bool success, string response)
    {
        if (success)
        {
            HandleLoginSuccess(response);
        }
        else
        {
            HandleLoginFailure(response);
        }
    }

    private void HandleLoginSuccess(string response)
    {
        Debug.Log("Login successful!");
        // Handle successful login (e.g., transition to another scene)
    }

    private void HandleLoginFailure(string response)
    {
        Debug.LogError("Login failed: " + response);
        // Handle login failure (e.g., show error message)
    }
}
