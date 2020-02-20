using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SerializableUserObjectList
{
    public List<SerializableUserObject> Users;
}

[Serializable]
public struct SerializableUserObject
{
    public string UserName;
    public string Password;
    public string Data;

    public void SetData (string newData)
    {
        Data = newData;
    }
    public override string ToString()
    {
        return $"UserName = {UserName}; Password = {Password}; Data = {Data};";
    }
}
