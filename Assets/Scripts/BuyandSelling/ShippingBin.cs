using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShippingBin : InteractableObject
{
	public static int hourToShop = 18;
	public static List<ItemSlotdata> itemsToShip = new List<ItemSlotdata>();

	public override void Pickup()
	{
		Itemdata handSlotItem = Inventory.Instance.GetEquippedSlotItem(InventorySlot.Inventorytype.Item);

		if (handSlotItem == null) return;	

		UIManager.Instance.TriggerYesNoPrompt($"너는 {handSlotItem.name} 아이템을 팔기를 원하니?", placeItemsInShippingBin);
	}

	void placeItemsInShippingBin()
	{
		ItemSlotdata handslot = Inventory.Instance.GetEquippedSlot(InventorySlot.Inventorytype.Item);

		itemsToShip.Add(new ItemSlotdata(handslot));

		handslot.Empty();

		Inventory.Instance.RenderHand();

		foreach(ItemSlotdata item in itemsToShip)
		{
			Debug.Log("아이템" + item.itemdata.name + "을" + item.quantity + "개 만큼 팔았다");
		}
	}

	public static void ShipItem()
	{
		int moneyToReceive = TallyItem(itemsToShip);

		PlayerStats.Earn(moneyToReceive);

		itemsToShip.Clear();
	}

	static int TallyItem(List<ItemSlotdata> items)
	{
		int total = 0;

		foreach(ItemSlotdata item in items)
		{
			total += item.quantity * item.itemdata.cost;
		}

		return total;
	}

}
