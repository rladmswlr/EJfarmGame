using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : InteractableObject
{
    public List<Itemdata> shopItems;

    public static void Purchase(Itemdata item, int quantity)
	{
		int totalcost = item.cost * quantity;

		if(PlayerStats.Money >= totalcost)
		{
			PlayerStats.Spend(totalcost);

			ItemSlotdata purchaseItem = new ItemSlotdata(item, quantity);

			Inventory.Instance.ShopToInventory(purchaseItem);
		}
	}

	public override void Pickup()
	{
		UIManager.Instance.OpenShop(shopItems);
	}
}
