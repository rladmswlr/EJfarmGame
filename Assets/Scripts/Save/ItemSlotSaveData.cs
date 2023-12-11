using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlotSaveData
{
    public string itemID;
    public int quantity;

    public ItemSlotSaveData(ItemSlotdata data)
	{
        if (data.IsEmpty())
        {
            itemID = null;
            quantity = 0;
            return;
        }
        itemID = data.itemdata.name;
        quantity = data.quantity;
    }
}
