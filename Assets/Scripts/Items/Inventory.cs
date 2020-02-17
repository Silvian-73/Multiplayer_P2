using UnityEngine;
using UnityEngine.Networking;

public class Inventory : NetworkBehaviour
{
    public Player Player;
    public int Space = 20;
    public event SyncList<Item>.SyncListChanged OnItemChanged;

    public SyncListItem Items = new SyncListItem();

    private UserData _data;

    public override void OnStartLocalPlayer()
    {
        Items.Callback += ItemChanged;
    }
    public void Load(UserData data)
    {
        _data = data;
        for (int i = 0; i < data.Inventory.Count; i++)
        {
            Items.Add(ItemBase.GetItem(data.Inventory[i]));
        }
    }
    private void ItemChanged(SyncList<Item>.Operation op, int itemIndex)
    {
        OnItemChanged(op, itemIndex);
    }

    public bool AddItem(Item item)
    {
        if (Items.Count < Space)
        {
            Items.Add(item);
            _data.Inventory.Add(ItemBase.GetItemId(item));
            return true;
        }
        else return false;
    }

    public void DropItem(Item item)
    {
        CmdDropItem(Items.IndexOf(item));
    }

    [Command]
    void CmdDropItem(int index)
    {
        if (Items[index] != null)
        {
            Drop(Items[index]);
            RemoveItem(Items[index]);
        }
    }

    private void Drop(Item item)
    {
        PickUpItem pickupItem = Instantiate(item.pickUpPrefab, Player.Character.transform.position, Quaternion.Euler(0, Random.Range(0, 360f), 0));
        pickupItem.Item = item;
        NetworkServer.Spawn(pickupItem.gameObject);
    }

    public void UseItem(Item item)
    {
        CmdUseItem(Items.IndexOf(item));
    }

    [Command]
    void CmdUseItem(int index)
    {
        if (Items[index] != null)
        {
            Items[index].Use(Player);
        }
    }

    public void RemoveItem(Item item)
    {
        Items.Remove(item);
        _data.Inventory.Remove(ItemBase.GetItemId(item));
    }
}
