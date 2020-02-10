using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public Image icon;
    public Button unequipButton;
    public Equipment equipment;

    private Item _item;

    public void SetItem(Item newItem)
    {
        _item = newItem;
        icon.sprite = _item.icon;
        icon.enabled = true;
        unequipButton.interactable = true;
    }

    public void ClearSlot()
    {
        _item = null;
        icon.sprite = null;
        icon.enabled = false;
        unequipButton.interactable = false;
    }

    public void Unequip()
    {
        equipment.UnequipItem(_item);
    }
}
