using UnityEngine;

public class PickUpItem : Interactable
{
    public Item Item;
    public float Lifetime;

    private void Update()
    {
        if (isServer)
        {
            Lifetime -= Time.deltaTime;
            if (Lifetime <= 0) Destroy(gameObject);
        }
    }

    public override bool Interact(GameObject user)
    {
        return PickUp(user);
    }

    public bool PickUp (GameObject user)
    {
        Character character = user.GetComponent<Character>();
        if (character != null && character.Player.Inventory.AddItem(Item))
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
