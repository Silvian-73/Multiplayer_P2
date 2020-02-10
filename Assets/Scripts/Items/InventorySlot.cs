using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    public Image Icon;
    public Button RemoveButton;
    public Inventory Inventory;

    private Item _item;

    public void SetItem(Item newItem)
    {
        _item = newItem;
        Icon.sprite = _item.icon;
        Icon.enabled = true;
        RemoveButton.interactable = true;
    }

    public void ClearSlot()
    {
        _item = null;
        Icon.sprite = null;
        Icon.enabled = false;
        RemoveButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        Inventory.DropItem(_item);
    }

    public void UseItem()
    {
        if (_item != null) Inventory.UseItem(_item);
    }
}