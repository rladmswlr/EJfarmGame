using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandInventorySlot : InventorySlot
{
	//�κ��丮�� �ش��ϴ� �������� Ŭ���Ұ�� ����
	public override void OnPointerClick(PointerEventData eventData)
	{
		Inventory.Instance.HandGoInventory(inventorytype);
	}
}
