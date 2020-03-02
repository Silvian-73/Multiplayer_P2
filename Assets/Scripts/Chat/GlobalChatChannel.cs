using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalChatChannel : ChatChannel
{

    #region Singleton
    public static ChatChannel Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of GlobalChat found!");
            return;
        }
        Instance = this;
    }
    #endregion
}
