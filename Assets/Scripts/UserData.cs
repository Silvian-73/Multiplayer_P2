using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class UserData
{
    public Vector3 PosCharacter;
    public List<int> Inventory = new List<int>();
    public List<int> Equipment = new List<int>();

    public int Level;
    public int StatPoints;
    public float Exp;
    public float NextLevelExp;

    public int CurHealth;
    public int StatDamage;
    public int StatArmor;
    public int StatMoveSpeed;
}
