using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    #region Singleton
    public static ItemCollection Collection;
    private void Awake()
    {
        if (Collection != null)
        {
            if(collectionLink != Collection)
            {
                Debug.LogError("More then one Item Collection found!");
                return;
            }
        }
        Collection = collectionLink;
    }
    #endregion
    [SerializeField] ItemCollection collectionLink;

    public static int GetItemId(Item item)
    {
        for (int i=0; i<Collection.items.Length; i++)
        {
            if (item == Collection.items[i])
            {
                return i;
            }
        }
        if (item != null)
        {
            Debug.LogError("Item " + item.name + " not found in ItemBase!");
        }
        return -1;
    }

    public static Item GetItem (int id)
    {
        return id == -1 ? null : Collection.items[id];
    }
}
