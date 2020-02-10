using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(StatsManager), typeof(NetworkIdentity), typeof(PlayerProgress))]
public class Player : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Equipment _equipment;
    [SerializeField] private PlayerProgress _progress;

    [SerializeField] private StatsManager _statsManager;

    public Character Character 
    { 
        get 
        { 
            return _character; 
        } 
    }
    public Inventory Inventory 
    { 
        get 
        { 
            return _inventory; 
        } 
    }
    public Equipment Equipment 
    { 
        get 
        { 
            return _equipment; 
        } 
    }
    public PlayerProgress Progress 
    { 
        get 
        { 
            return _progress;
        }
    }

    public void Setup(Character character, Inventory inventory, Equipment equipment, bool isLocalPlayer)
    {
        _statsManager = GetComponent<StatsManager>();
        _progress = GetComponent<PlayerProgress>();
        _character = character;
        _inventory = inventory;
        _equipment = equipment;
        _character.Player = this;
        _inventory.Player = this;
        _equipment.Player = this;
        _statsManager.Player = this;

        if (GetComponent<NetworkIdentity>().isServer)
        {
            _character.Stats.Manager = _statsManager;
            _progress.Manager = _statsManager;
        }

        if (isLocalPlayer)
        {
            InventoryUI.Instance.SetInventory(_inventory);
            EquipmentUI.Instance.SetEquipment(_equipment);
            StatsUI.Instance.SetManager(_statsManager);
        }
    }
}
