using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerChat : NetworkBehaviour
{
    #region Singleton
    public static PlayerChat Instance;

    public override void OnStartClient()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of PlayerChat found!");
            return;
        }
        Instance = this;
    }
    #endregion

    public List<ChatChannel> Channels = new List<ChatChannel>(3);

    public event Action OnChangeChannels;
    public event Action <ChatMessage> OnReciveMessage;

    public void RegisterChannel(ChatChannel channel)
    {
        Channels.Add(channel);
        if (OnChangeChannels != null)
        {
            OnChangeChannels.Invoke();
        }
    }
    [Command]
    public void CmdSendFromChannel(GameObject channelGO, ChatMessage message)
    {
        message.Author = AccountManager.GetAccount(connectionToClient).Login;
        channelGO.GetComponent<ChatChannel>().SendFromChanel(message);
    }
    public void ReciveChatMessage(ChatMessage message)
    {
        OnReciveMessage.Invoke(message);
    }
}
