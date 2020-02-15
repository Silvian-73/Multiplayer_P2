using System;
using System.Collections.Generic;
using UnityEngine;

//[Serializable]
//public struct SerializableUserObjectArray
//{
//    public int Count;
//    public SerializableUserObject[] Users;
//}

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

    public override string ToString()
    {
        return $"UserName = {UserName}; Password = {Password}; Data = {Data};";
    }
}
