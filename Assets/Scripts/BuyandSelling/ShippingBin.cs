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

		UIManager.Instance.TriggerYesNoPrompt($"�ʴ� {handSlotItem.name} �������� �ȱ⸦ ���ϴ�?", placeItemsInShippingBin);
	}

	void placeItemsInShippingBin()
	{
		ItemSlotdata handslot = Inventory.Instance.GetEquippedSlot(InventorySlot.Inventorytype.Item);

		itemsToShip.Add(new ItemSlotdata(handslot));

		handslot.Empty();

		Inventory.Instance.RenderHand();

		foreach(ItemSlotdata item in itemsToShip)
		{
			Debug.Log("������" + item.itemdata.name + "��" + item.quantity + "�� ��ŭ �ȾҴ�");
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
