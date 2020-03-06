using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PrivateChatChannel : ChatChannel
{

    private NetworkInstanceId _receiver;

    public void SetReciverMessage(ChatMessage message)
    {
        _receiver = message.SenderId;
        name = message.Author;
    }

    public override void SendFromPlayerChat(string text)
    {
        ChatMessage msg = new ChatMessage(PlayerChat.Instance.netId, _receiver, PlayerChat.Instance.name, text);
        PlayerChat.Instance.CmdSendFromChannel(gameObject, msg);
    }

    public override void SendFromChanel(ChatMessage message)
    {
        TargetSendFromChanel(NetworkServer.objects[message.ReceiverId].connectionToClient, message);
    }

    [TargetRpc]
    protected void TargetSendFromChanel(NetworkConnection target, ChatMessage message)
    {
        PlayerChat.Instance.ReciveChatMessage(message);
    }
}
