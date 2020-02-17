using UnityEngine;
using UnityEngine.Networking;

public class PlayerLoader : NetworkBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private PlayerController _controller;

    public Character CreateCharacter()
    {
        UserAccount acc = AccountManager.GetAccount(connectionToClient);
        GameObject unit = Instantiate(_unitPrefab, acc.Data.PosCharacter, Quaternion.identity);
        NetworkServer.Spawn(unit);
        TargetLinkCharacter(connectionToClient, unit.GetComponent<NetworkIdentity>());
        return unit.GetComponent<Character>();
    }

    public override void OnStartAuthority()
    {
        CmdCreatePlayer();  
    }
    [Command]
    public void CmdCreatePlayer()
    {
        Character character = CreateCharacter();
        _player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), isLocalPlayer);
        _controller.SetCharacter(character, isLocalPlayer);
    }

    [TargetRpc]
    void TargetLinkCharacter(NetworkConnection target, NetworkIdentity unit)
    {
        Character character = unit.GetComponent<Character>();
        _player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), true);
        _controller.SetCharacter(character, true);
    }
    public override bool OnCheckObserver(NetworkConnection connection)
    {
        return false;
    }

    private void OnDestroy()
    {
        if (isServer && _player.Character != null)
        {
            UserAccount acc = AccountManager.GetAccount(connectionToClient);
            acc.Data.PosCharacter = _player.Character.transform.position;
            Destroy(_player.Character.gameObject);
            NetworkManager.singleton.StartCoroutine(acc.Quit());
        }
    }
}
