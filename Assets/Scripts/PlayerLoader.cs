using UnityEngine;
using UnityEngine.Networking;

public class PlayerLoader : NetworkBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerController _controller;

    public Character CreateCharacter()
    {
        UserAccount acc = AccountManager.GetAccount(connectionToClient);
        GameObject unitPrefab = NetworkManager.singleton.spawnPrefabs.Find(x => x.GetComponent<NetworkIdentity>().assetId.Equals(acc.Data.CharacterHash));
        GameObject unit;
        Character tempCharacter;
        if (acc.Data.JustCreated)
        {
            unit = Instantiate(unitPrefab, transform.position, Quaternion.identity, transform);
            acc.Data.JustCreated = false;
            acc.Data.PosCharacter = transform.position;
            acc.Data.SpawnPosition = transform.position;
        }
        else
        {
            unit = Instantiate(unitPrefab, acc.Data.PosCharacter, Quaternion.identity, transform);
        }
        tempCharacter = unit.GetComponent<Character>();
        tempCharacter.SetRespawnPosition(acc.Data.SpawnPosition);
        tempCharacter.Player = _player;
        NetworkServer.Spawn(unit);
        TargetLinkCharacter(connectionToClient, unit.GetComponent<NetworkIdentity>());
        return tempCharacter;
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
