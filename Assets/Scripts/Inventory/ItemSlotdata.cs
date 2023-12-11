using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlotdata
{
    public Itemdata itemdata;
    public int quantity;        //æ∆¿Ã≈€∞πºˆ

    public ItemSlotdata(Itemdata itemdata, int quantity)
	{
        this.itemdata = itemdata;
        this.quantity = quantity;
        ValidataQuantity();
	}

    public ItemSlotdata(Itemdata itemData)
	{
        this.itemdata = itemData;
        quantity = 1;
        ValidataQuantity();
    }

    public ItemSlotdata(ItemSlotdata slotToClone)
	{
        itemdata = slotToClone.itemdata;
        quantity = slotToClone.quantity;
	}

    public void AddQuantity()
	{
        AddQuantity(1);
	}

    public void AddQuantity(int amout)
	{
        quantity += amout;
	}

    public void Remove()
	{
        quantity--;
        ValidataQuantity();

    }

    public bool Stackable(ItemSlotdata slotToCompare)
	{
        return slotToCompare.itemdata == itemdata;
	}

    public void ValidataQuantity()
	{
        if(quantity <= 0 || itemdata == null)
		{
            Empty();
		}
	}

    public void Empty()
	{
        itemdata = null;
        quantity = 0;
	}

    public bool IsEmpty()
	{
        return itemdata == null;
	}

    public static ItemSlotSaveData SerializeData(ItemSlotdata itemSlot)
	{
        return new ItemSlotSaveData(itemSlot);

    }

    public static ItemSlotdata DeserializeData(ItemSlotSaveData itemSlot)
    {
        Itemdata item = Inventory.Instance.ItemIndex.GetItemFromString(itemSlot.itemID);

        return new ItemSlotdata(item, itemSlot.quantity);

    }

    public static ItemSlotSaveData[] SerializeArray(ItemSlotdata[] array)
	{
        return Array.ConvertAll(array, new Converter<ItemSlotdata, ItemSlotSaveData>(SerializeData));
	}

    public static ItemSlotdata[] DeserializeArray(ItemSlotSaveData[] array)
    {
        return Array.ConvertAll(array, new Converter<ItemSlotSaveData, ItemSlotdata>(DeserializeData));
    }
}
