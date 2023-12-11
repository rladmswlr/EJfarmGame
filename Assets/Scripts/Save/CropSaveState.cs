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
		//계속 작물을 성장시킨다.
		growth++;

		CropManage seedInfo = (CropManage)Inventory.Instance.ItemIndex.GetItemFromString(seedToGrow);

		int maxgrowth = TimeManage.HourToMinute(TimeManage.DaysToHours(seedInfo.GrowTime));


		//최대 작물 자라는시간의 50%에 도달할경우 seedling 단계에 진입
		if (growth >= maxgrowth / 2 && cropState == CropBehavior.CropState.Seed)
		{
			cropState = CropBehavior.CropState.Seedling;
			//LandManager.Instance.OnCropStateChange(landID, cropState, growth);
		}
		//최고단계에 도착할경우 growth완료
		if (growth >= maxgrowth && cropState == CropBehavior.CropState.Seedling)
		{
			cropState = CropBehavior.CropState.Crop;

			//LandManager.Instance.DeregisterCrop(this.landID);

			//LandManager.Instance.DeregisterCrop(landID);
		}

	}
}
