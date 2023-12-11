using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CropSaveState
{
    public int landID;

    public string seedToGrow;
    public CropBehavior.CropState cropState;
    public int growth;

    public CropSaveState(int landID, string seedToGrow, CropBehavior.CropState cropState, int growth)
	{
        this.landID = landID;
        this.seedToGrow = seedToGrow;
        this.cropState = cropState;
        this.growth = growth;
        
	}

	public void Grow()
	{
		//��� �۹��� �����Ų��.
		growth++;

		CropManage seedInfo = (CropManage)Inventory.Instance.ItemIndex.GetItemFromString(seedToGrow);

		int maxgrowth = TimeManage.HourToMinute(TimeManage.DaysToHours(seedInfo.GrowTime));


		//�ִ� �۹� �ڶ�½ð��� 50%�� �����Ұ�� seedling �ܰ迡 ����
		if (growth >= maxgrowth / 2 && cropState == CropBehavior.CropState.Seed)
		{
			cropState = CropBehavior.CropState.Seedling;
			//LandManager.Instance.OnCropStateChange(landID, cropState, growth);
		}
		//�ְ�ܰ迡 �����Ұ�� growth�Ϸ�
		if (growth >= maxgrowth && cropState == CropBehavior.CropState.Seedling)
		{
			cropState = CropBehavior.CropState.Crop;

			//LandManager.Instance.DeregisterCrop(this.landID);

			//LandManager.Instance.DeregisterCrop(landID);
		}

	}
}
