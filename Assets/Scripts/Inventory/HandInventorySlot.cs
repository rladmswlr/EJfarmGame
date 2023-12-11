using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandInventorySlot : InventorySlot
{
	//인벤토리에 해당하는 아이템을 클릭할경우 장착
	public override void OnPointerClick(PointerEventData eventData)
	{
		Inventory.Instance.HandGoInventory(inventorytype);
	}
}
