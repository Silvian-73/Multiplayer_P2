using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class UserAccount : MonoBehaviour
{
    public string Login; 
    public string Pass;
    public UserData Data;
    public NetworkConnection Conn;
    
    private XmlSerializer _xmlSerializer = new XmlSerializer(typeof(UserData));
    private StringWriter _dataWriter = new StringWriter();
    private UserDataRepository _repository = UserDataRepository.Instance;

    public UserAccount(NetworkConnection conn)
    {
        this.Conn = conn;
    }

    public IEnumerator LoginUser(string login, string pass)
    {
        IEnumerator eLogin = _repository.Login(login, pass);
        while (eLogin.MoveNext())
        {
            yield return eLogin.Current;
        }
        string response = eLogin.Current as string;

        if (response == "Success")
        {
            Debug.Log("server login success");
            Login = login;
            Pass = pass;
            if (AccountManager.AddAccount(this))
            {
                IEnumerator eLoad = LoadData();
                while (eLoad.MoveNext())
                {
                    yield return eLoad.Current;
                }
                response = eLoad.Current as string;
                if (response == "Error")
                {
                    yield return eLoad.Current;
                }
                else
                {
                    yield return eLogin.Current;
                }
            }
            else
            {
                Debug.Log("account already use");
                yield return "Already use";
            }
        }
        else
        {
            Debug.Log("server login fail");
            yield return eLogin.Current;
        }
    }

    IEnumerator LoadData()
    {
        IEnumerator e = _repository.GetUserData(Login, Pass);
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string;
        if (response == "Error")
        {
            Debug.LogError("UserData for user " + Login + " load error with code: " + response);
        }
        else
        {
            Debug.Log("UserData for user " + Login + " completely load.");
            Debug.Log(response);
            if (response != "")
            {
                Data = (UserData)_xmlSerializer.Deserialize(new StringReader(response));
            }
            else
            {
                Data = new UserData();
            }
        }
    }

    IEnumerator SaveData()
    {
        _xmlSerializer.Serialize(_dataWriter, Data);
        IEnumerator e = _repository.SetUserData(Login, Pass, _dataWriter.ToString());
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string;
        if (response == "Success")
        {
            Debug.Log("UserData for user " + Login + " completely save.");
        }
        else
        {
            Debug.LogError("UserData for user " + Login + " save error with code: " + response);
        }
    }
    public IEnumerator Quit()
    {
        IEnumerator eSave = SaveData();
        while (eSave.MoveNext())
        {
            yield return eSave.Current;
        }
        AccountManager.RemoveAccount(this);
    }
}
