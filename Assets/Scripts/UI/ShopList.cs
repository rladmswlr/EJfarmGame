using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopList : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image itemThumbnail;
    public Text nameText;
    public Text costText;

    Itemdata itemData;

    public void Display(Itemdata itemData)
	{
        this.itemData = itemData;
        itemThumbnail.sprite = itemData.Drawing;
        nameText.text = itemData.name;
        costText.text = itemData.cost + PlayerStats.CURRENCY;
	}
    public void OnPointerClick(PointerEventData eventData)
	{
        UIManager.Instance.shopListingManager.OpenConfirmationScreen(itemData);
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(itemData);
    }

    //슬롯에서 나갈때 아이템정보 초기화
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(null);
    }
}
