using UnityEngine;

public static class ManagerFactory
{
    private static AuthManager authManager;
    private static UIManager uiManager;

    public static AuthManager GetAuthManager()
    {
        if (authManager == null)
        {
            authManager = Object.FindObjectOfType<AuthManager>();
            if (authManager == null)
            {
                var authManagerGameObject = new GameObject("AuthManager");
                authManager = authManagerGameObject.AddComponent<AuthManager>();
            }
        }
        return authManager;
    }

    public static UIManager GetUIManager()
    {
        if (uiManager == null)
        {
            uiManager = Object.FindObjectOfType<UIManager>();
            if (uiManager == null)
            {
                var uiManagerGameObject = new GameObject("UIManager");
                uiManager = uiManagerGameObject.AddComponent<UIManager>();
            }
        }
        return uiManager;
    }
}
