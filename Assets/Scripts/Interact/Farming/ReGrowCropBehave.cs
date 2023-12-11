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
		//�÷��̾� ������ ������ ���� �������� �ְ�
		Inventory.Instance.EquipHandSlot(item);

		//�տ� ����������� �����
		Inventory.Instance.RenderHand();

		upcrop.Regrow();
	}
}
