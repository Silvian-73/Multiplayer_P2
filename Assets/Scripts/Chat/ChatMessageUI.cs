using UnityEngine;
using UnityEngine.UI;

public class ChatMessageUI : MonoBehaviour
{

    [SerializeField] private Text _authorText;
    [SerializeField] private Text _messageText;
    [SerializeField] private Button _privateButton;

    private ChatMessage _msg;

    public void SetChatMessage(ChatMessage message)
    {
        _msg = message;
        _authorText.text = message.Author;
        _messageText.text = message.Message;
        _privateButton.onClick.AddListener(SetPrivateMessage);
    }

    public void SetPrivateMessage()
    {
        ChatUI.Instance.SetPrivateMessage(_msg);
    }
}
