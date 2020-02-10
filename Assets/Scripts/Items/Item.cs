using UnityEngine;

[CreateAssetMenu (fileName ="New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public PickUpItem pickUpPrefab;

    public virtual void Use(Player player)
    {
        Debug.Log("Using " + name);
    }
}
