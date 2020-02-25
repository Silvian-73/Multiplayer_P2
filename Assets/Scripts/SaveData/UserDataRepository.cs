using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class UserDataRepository : Singleton<UserDataRepository>
{
    private IData<SerializableUserObjectList> _data = new JsonData<SerializableUserObjectList>();
    private SerializableUserObjectList _users = new SerializableUserObjectList();

    private string _folderName = "DataSave";
    private string _fileName = "data.dat";
    private string _path;

    const string SUCCESS = "Success";
    const string USERERROR = "UserError";
    const string PASSERROR = "PassError";

    private void Awake()
    {
        Load();
    }
    private void Save()
    {
        if (!Directory.Exists(Path.Combine(_path)))
        {
            Directory.CreateDirectory(_path);
        }
        if (_users.Users.Count > 0)
        {
            _data.Save(_users, Path.Combine(_path, _fileName));
        }
    }
    private void Load()
    {
        _path = Path.Combine(Application.dataPath, _folderName);
        if (!Directory.Exists(Path.Combine(_path)))
        {
            Directory.CreateDirectory(_path);
        }
        var file = Path.Combine(_path, _fileName);
        if (!File.Exists(file))
        {
            _users.Users = new List<SerializableUserObject>();
        }
        else
        {
            _users = _data.Load(file);
        }
        
    }
    public IEnumerator<string> Login (string userName, string password)
    {
        Load();
        if (_users.Users.Count == 0)
        {
            yield return USERERROR;
        }

        bool noUser = true;
        bool wrongPassword = false;

        foreach (SerializableUserObject user in _users.Users)
        {
            if (user.UserName == userName)
            {
                noUser = false;
                if (user.Password != password)
                {
                    wrongPassword = true;
                }
            }
        }
        if ((!noUser && !wrongPassword))
        {
            yield return SUCCESS;
        }
        if (noUser)
        {
           yield return USERERROR;
        }
        if (wrongPassword)
        {
            yield return PASSERROR;
        }
    }

    public IEnumerator<string> RegisterUser (string userName, string password, string data)
    {
        Load();
        bool userExist = false;
        foreach (SerializableUserObject user in _users.Users)
        {
            if (user.UserName == userName)
            {
                userExist = true;
            }
        }
        if (userExist)
        {
            yield return USERERROR;
        }
        else
        {
            var tempUser = new SerializableUserObject();
            tempUser.UserName = userName;
            tempUser.Password = password;
            tempUser.Data = data;
            _users.Users.Add(tempUser);
            Save();
            yield return SUCCESS;
        }
    }

    public IEnumerator<string> SetUserData(string userName, string password, string data)
    {
        bool userExist = false;
        int targetUser = 0;

        foreach (SerializableUserObject user in _users.Users)
        {
            if (user.UserName == userName)
            {
                userExist = true;
                targetUser =_users.Users.IndexOf(user);
            }
        }
        if (!userExist)
        {
            yield return USERERROR;
        }
        else
        {
            _users.Users.RemoveAt(targetUser);
            SerializableUserObject tempUser = new SerializableUserObject { UserName = userName, Password = password, Data = data };
            _users.Users.Add(tempUser);
            Save();
            yield return SUCCESS;
        }
    }

    public IEnumerator<string> GetUserData(string userName, string password)
    {
        bool userExist = false;
        foreach (SerializableUserObject user in _users.Users)
        {
            if (user.UserName == userName)
            {
                userExist = true;
                yield return user.Data;
            }
        }
        if (!userExist)
        {
            yield return USERERROR;
        }
    }
}
