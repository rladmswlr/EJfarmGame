using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Itemdata ItemChaseImage;
	int quantity;

	public Image ItemImage;
	public Text quantityText;

	public enum Inventorytype
	{
		Item, Tool
	}

	public Inventorytype inventorytype;
	int InventoryNum;

	//�ش� �̹��� ������ �׸�
    public void Display(ItemSlotdata itemSlot)
	{
		ItemChaseImage = itemSlot.itemdata;
		quantity = itemSlot.quantity;

		quantityText.text = "";

		if(ItemChaseImage != null)
		{
			ItemImage.sprite = ItemChaseImage.Drawing;

			if(quantity > 1)
			{
				quantityText.text = quantity.ToString();
			}

			ItemImage.gameObject.SetActive(true);

			return;
		}

		ItemImage.gameObject.SetActive(false);

	}

	//�κ��丮�� �ش��ϴ� �������� Ŭ���Ұ�� ����
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		Inventory.Instance.InventoryGoHand(InventoryNum, inventorytype);
	}

	public void CheckSlotNum(int slotNum)
	{
		this.InventoryNum = slotNum;
	}

	//���Կ� ���� ������ ������ ����
	public void OnPointerEnter(PointerEventData eventData)
	{
		UIManager.Instance.DisplayItemInfo(ItemChaseImage);
	}


	//���Կ��� ������ ���������� �ʱ�ȭ
	public void OnPointerExit(PointerEventData eventData)
	{
		UIManager.Instance.DisplayItemInfo(null);
	}
}
