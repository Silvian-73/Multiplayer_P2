using System;
using UnityEngine.Networking;

[Serializable]
public struct ChatMessage
{
    public NetworkInstanceId SenderId;
    public NetworkInstanceId ReceiverId;
    public string Author;
    public string Message;

    public ChatMessage(NetworkInstanceId sender, NetworkInstanceId reciver, string author, string message)
    {
        SenderId = sender;
        ReceiverId = reciver;
        Author = author;
        Message = message;
    }
}
