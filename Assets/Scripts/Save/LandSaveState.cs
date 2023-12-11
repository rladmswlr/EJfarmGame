using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LandSaveState
{
	public Ground.Grounded landStatus;
	public TimeManage lastWatered;

	public LandSaveState(Ground.Grounded landStatus, TimeManage lastWatered)
	{
		this.landStatus = landStatus;
		this.lastWatered = lastWatered;
	}

	public void ClockUpdate(TimeManage timemanage)
	{
		if (landStatus == Ground.Grounded.Mud)
		{
			int CheckTime = TimeManage.CompareTimeManage(lastWatered, timemanage);

			//24�ð��� �����ٸ� ���붥�� ����������
			if (CheckTime > 24)
			{
				landStatus = Ground.Grounded.Soil; 
			}
		}

	}
}
