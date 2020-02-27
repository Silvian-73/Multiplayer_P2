using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerVisibility : NetworkBehaviour
{
    [SerializeField] private float _visRange = 10f;
    [SerializeField] private float _visUpdateInterval = 1f;
    [SerializeField] private LayerMask _visMask;

    private Transform _transform;
    private float _visUpdateTime;

    private Collider[] _bufferColliders = new Collider[128];
    private int _targetColliders;

    public override void OnStartServer()
    {
        _transform = transform;
    }
    void Update()
    {
        if (isServer)
        {
            if (Time.time - _visUpdateTime > _visUpdateInterval)
            {
                GetComponent<NetworkIdentity>().RebuildObservers(false);
                _visUpdateTime = Time.time;
            }
        }

    }
    public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize)
    {
        _targetColliders = Physics.OverlapSphereNonAlloc(_transform.position, _visRange, _bufferColliders, _visMask);
        for (int i=0; i < _targetColliders; i++)
        {
            Character character = _bufferColliders[i].GetComponent<Character>();
            if (character != null && character.Player != null)
            {
                NetworkIdentity identity = character.Player.GetComponent<NetworkIdentity>();
                if (identity != null && identity.connectionToClient != null)
                {
                    observers.Add(identity.connectionToClient);
                }
            }
        }
        Character m_character = GetComponent<Character>();
        if (m_character != null && !observers.Contains(m_character.Player.Conn))
        {
            observers.Add(m_character.Player.Conn);
        }
        return true;
    }
    public override bool OnCheckObserver(NetworkConnection connection)
    {
        Character character = GetComponent<Character>();
        if (character != null && connection == character.Player.Conn)
        {
            return true;
        }
        Player player = null;
        foreach (UnityEngine.Networking.PlayerController controller in connection.playerControllers)
        {
            if (controller != null)
            {
                player = controller.gameObject.GetComponent<Player>();
                if (player != null) break;
            }
        }
        if (player != null && player.Character != null)
        {
            return (player.Character.transform.position - _transform.position).magnitude < _visRange;
        }
        else
        {
            return false;
        }
    }
}
