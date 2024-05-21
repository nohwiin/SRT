using System;

[Serializable]
public class User
{
    public string Username;
    public string Password;

    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }
}
