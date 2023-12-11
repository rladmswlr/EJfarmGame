using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReGrowCropBehave : InteractableObject
{
	CropBehavior upcrop;

	public void setParent(CropBehavior upcrop)
	{
		this.upcrop = upcrop;
	}

	// Start is called before the first frame update
	public override void Pickup()
	{
		//플레이어 아이템 장착에 현재 아이템을 넣고
		Inventory.Instance.EquipHandSlot(item);

		//손에 현재아이템을 쥐어줌
		Inventory.Instance.RenderHand();

		upcrop.Regrow();
	}
}
