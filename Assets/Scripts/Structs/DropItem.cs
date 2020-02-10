using UnityEngine;

[System.Serializable]
struct DropItem
{
    public Item Item;
    [Range(0, 100f)]
    public float Rate;
}
