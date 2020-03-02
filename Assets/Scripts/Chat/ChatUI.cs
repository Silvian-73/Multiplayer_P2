using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ChatUI : MonoBehaviour
{

    #region Singleton
    public static ChatUI Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of ChatUI found!");
            return;
        }
        Instance = this;
    }
    #endregion
    [SerializeField] private Dropdown _channelsDropdown;
    [SerializeField] private InputField _messageInput;
    [SerializeField] private PrivateChatChannel _privateChannel;
    [SerializeField] private Transform _messageContaner;
    [SerializeField] private ChatMessageUI _messagePrefab;

    private PlayerChat _playerChat;
    private void RefreshChanels()
    {
        _channelsDropdown.ClearOptions();
        _channelsDropdown.AddOptions(_playerChat.Channels.ConvertAll(x => x.name));
    }

    public void ReciveChatMessage(ChatMessage message)
    {
        ChatMessageUI newMessage = Instantiate(_messagePrefab, _messageContaner);
        newMessage.SetChatMessage(message);
    }
    public void SetPlayerChat(PlayerChat chat)
    {
        _playerChat = chat;
        RefreshChanels();
        _playerChat.OnChangeChannels += RefreshChanels;
        _playerChat.OnReciveMessage += ReciveChatMessage;
    }

    public void SendChatMessage(string text)
    {
        if (_playerChat != null)
        {
            _playerChat.Channels[_channelsDropdown.value].SendFromPlayerChat(text);
        }
    }
    public void SetPrivateMessage(ChatMessage message)
    {
        _privateChannel.SetReciverMessage(message);
        if (_playerChat.Channels.Contains(_privateChannel))
        {
            RefreshChanels();
        }
        else
        {
            _playerChat.RegisterChannel(_privateChannel);
        }
        _channelsDropdown.value = _channelsDropdown.options.Count - 1;
    }
    private void Update()
    {
        if (_messageInput.isFocused && !_messageInput.text.Equals("") && Input.GetKey(KeyCode.Return))
        {
            SendChatMessage(_messageInput.text);
            _messageInput.text = String.Empty;
        }
    }
}
