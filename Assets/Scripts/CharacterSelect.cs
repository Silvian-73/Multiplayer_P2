using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterSelect : MonoBehaviour
{
    #region Singleton
    public static CharacterSelect Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
        _manager.serverRegisterHandler += RegisterServerHandler;
        _manager.clientRegisterHandler += RegisterClientHandler;
    }
    #endregion

    [SerializeField] private MyNetworkManager _manager;

    void RegisterServerHandler()
    {
        NetworkServer.RegisterHandler(MsgType.Highest + 1 + (short)NetMsgType.SelectCharacter, OnSelectCharacter);
    }

    void RegisterClientHandler(NetworkClient client)
    {
        client.RegisterHandler(MsgType.Highest + 1 + (short)NetMsgType.SelectCharacter, OnOpenSelectUI);
    }
    void OnSelectCharacter(NetworkMessage netMsg)
    {
        NetworkHash128 hash = netMsg.reader.ReadNetworkHash128();
        if (hash.IsValid())
        {
            UserAccount account = AccountManager.GetAccount(netMsg.conn);
            account.Data.CharacterHash = hash;
            _manager.AccountEnter(account);
        }
    }

    void OnOpenSelectUI(NetworkMessage netMsg)
    {
       CharacterSelectUI.Instance.OpenPanel();
    }

    public void SelectCharacter(NetworkHash128 characterHash)
    {
        if (characterHash.IsValid())
        {
            _manager.client.Send(MsgType.Highest + 1 + (short)NetMsgType.SelectCharacter, new HashMessage(characterHash));
        }
    }
}
