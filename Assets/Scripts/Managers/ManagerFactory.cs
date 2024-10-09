using UnityEngine;

public static class ManagerFactory
{
    private static AuthManager authManager;
    private static UIManager uiManager;
    private static SearchManager searchManager;

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

    public static SearchManager GetSearchManager()
    {
        if (searchManager == null)
        {
            searchManager = Object.FindObjectOfType<SearchManager>();
            if (searchManager == null)
            {
                var searchManagerGameObject = new GameObject("SearchManager");
                searchManager = searchManagerGameObject.AddComponent<SearchManager>();
            }
        }
        return searchManager;
    }
}
