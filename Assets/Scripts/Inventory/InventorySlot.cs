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

	//해당 이미지 슬롯을 그림
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

	//인벤토리에 해당하는 아이템을 클릭할경우 장착
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		Inventory.Instance.InventoryGoHand(InventoryNum, inventorytype);
	}

	public void CheckSlotNum(int slotNum)
	{
		this.InventoryNum = slotNum;
	}

	//슬롯에 들어갈때 아이템 정보를 받음
	public void OnPointerEnter(PointerEventData eventData)
	{
		UIManager.Instance.DisplayItemInfo(ItemChaseImage);
	}


	//슬롯에서 나갈때 아이템정보 초기화
	public void OnPointerExit(PointerEventData eventData)
	{
		UIManager.Instance.DisplayItemInfo(null);
	}
}
