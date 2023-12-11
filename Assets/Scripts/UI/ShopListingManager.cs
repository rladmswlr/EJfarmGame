using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopListingManager : MonoBehaviour
{
    public GameObject shopListing;
    public Transform listingGrid;

	Itemdata itemToBuy;
	int quantity;

	[Header("Confirmation Screen")]
	public GameObject confirmationScreen;
	public Text confirmationPrompt;
	public Text quantityText;
	public Text costCalculationText;
	public Button purchaseButton;


    public void RenderShop(List<Itemdata> shopItems)
	{
        if(listingGrid.childCount > 0)
		{
			foreach(Transform child in listingGrid)
			{
				Destroy(child.gameObject);
			}
		}
		foreach (Itemdata shopItem in shopItems)
		{
			GameObject listingGameObject = Instantiate(shopListing, listingGrid);

			listingGameObject.GetComponent<ShopList>().Display(shopItem);
		}	
	}

	public void OpenConfirmationScreen(Itemdata item)
	{
		itemToBuy = item;
		quantity = 1;
		RenderConfirmationScreen();
	}

	public void RenderConfirmationScreen()
	{
		confirmationScreen.SetActive(true);

		confirmationPrompt.text = "�ʴ� " + itemToBuy.name + "�� ����̴�?";

		quantityText.text = "x " + quantity;

		int cost = itemToBuy.cost * quantity;

		int playerMoneyLeft = PlayerStats.Money - cost;

		if (playerMoneyLeft < 0)
		{
			costCalculationText.text = "�ڱ��� ������� �ʽ��ϴ�.";
			purchaseButton.interactable = false;
			return;
		}

		purchaseButton.interactable = true;

		costCalculationText.text = PlayerStats.Money + " > " + playerMoneyLeft;
	}

	public void AddQuantity()
	{
		quantity++;
		RenderConfirmationScreen();
	}

	public void SubtractQuantity()
	{
		if(quantity <  1)
		{
			quantity--;
		}

		RenderConfirmationScreen();
	}

	public void ConfirmPurchase()
	{
		Shop.Purchase(itemToBuy, quantity);
		confirmationScreen.SetActive(false);
	}

	public void CancelPurchase()
	{
		confirmationScreen.SetActive(false);
	}
}
