using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehavior : MonoBehaviour
{
	int landID;

	public static CropBehavior Instance { get; private set; }

	//�۹��� �󸶳� �ڶ���� �˷��ֱ����� Ŭ����
	CropManage CropGrow;

	[Header("Stages of Life")]
	public GameObject seed;
	private GameObject seedling;
	private GameObject crop;

	public enum CropState
	{
		Seed, Seedling, Crop
	}
	public CropState cropState;

	int growth;
	int maxgrowth;

	private void Awake()
	{
		//�̰� �̿ܿ� �Ѱ� �� ������� ������Ʈ ����
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}

		else
		{
			Instance = this;
		}
	}

	//�̴ϼȶ���¡ �۾�
	public void Plant(int landID, CropManage CropGrow)
	{
		this.landID = landID;

		//�ش� �۹��� ������ ����
		this.CropGrow = CropGrow;

		//seed = Instantiate(CropGrow.seed, transform);

		//�������� �۹��� �ν��Ͻ�ȭ
		seedling = Instantiate(CropGrow.seedling, transform);

		// ������� ������Ʈ �ν��Ͻ�ȭ
		Itemdata cropToDay = CropGrow.Output;

		//crop = Instantiate(cropToDay.Object, transform);

		crop = Instantiate(CropGrow.Harvested, transform);

		//�ִ� �ڶ�°� ��ð��� �ɸ����� ���
		int hoursToGrow = TimeManage.DaysToHours(CropGrow.GrowTime);
		// ������� �ڶ���� ���
		maxgrowth = TimeManage.HourToMinute(hoursToGrow);

		if (CropGrow.regrowble)
		{
			ReGrowCropBehave regrowcrop = crop.GetComponent<ReGrowCropBehave>();

			//�ش��۹��� �������� �����Ѵ�.
			regrowcrop.setParent(this);
		}

		//�ʱ� �Ĺ��� ������ Seed�� �ذ���
		SwitchState(cropState);
		LandManager.Instance.RegisterCrop(landID, CropGrow, cropState, growth);
	}

	public void LoadCrop(int landID, CropManage seedToGrow, CropBehavior.CropState cropState, int growth)
	{
		this.landID = landID;

		//�ش� �۹��� ������ ����
		this.CropGrow = seedToGrow;

		//seed = Instantiate(CropGrow.seed, transform);

		//�������� �۹��� �ν��Ͻ�ȭ
		seedling = Instantiate(CropGrow.seedling, transform);

		// ������� ������Ʈ �ν��Ͻ�ȭ
		Itemdata cropToDay = CropGrow.Output;

		//crop = Instantiate(cropToDay.Object, transform);

		crop = Instantiate(CropGrow.Harvested, transform);

		//�ִ� �ڶ�°� ��ð��� �ɸ����� ���
		int hoursToGrow = TimeManage.DaysToHours(CropGrow.GrowTime);
		// ������� �ڶ���� ���
		maxgrowth = TimeManage.HourToMinute(hoursToGrow);

		this.growth = growth;

		if (CropGrow.regrowble)
		{
			ReGrowCropBehave regrowcrop = crop.GetComponent<ReGrowCropBehave>();

			//�ش��۹��� �������� �����Ѵ�.
			regrowcrop.setParent(this);
		}

		//�ʱ� �Ĺ��� ������ Seed�� �ذ���
		SwitchState(cropState);
	}

	public void Grow()
	{
		//��� �۹��� �����Ų��.
		growth++;

		//�ִ� �۹� �ڶ�½ð��� 50%�� �����Ұ�� seedling �ܰ迡 ����
		if (growth >= maxgrowth / 2 && cropState == CropState.Seed)
		{
			SwitchState(CropState.Seedling);
			LandManager.Instance.OnCropStateChange(landID, cropState, growth);
		}
		//�ְ�ܰ迡 �����Ұ�� growth�Ϸ�
		if(growth >= maxgrowth && cropState == CropState.Seedling)
		{
			SwitchState(CropState.Crop);
		}

	}

	private void SwitchState(CropState stateSwitch)
	{
		//�ش��۹��� ���¸� �ʱ�ȭ��Ų��.
		seed.SetActive(false);
		seedling.SetActive(false);
		crop.SetActive(false);

		switch(stateSwitch)
		{
			case CropState.Seed:
				seed.SetActive(true);
				break;
			case CropState.Seedling:
				seedling.SetActive(true);
				break;
			case CropState.Crop:
				if (!CropGrow.regrowble)
				{
					crop.SetActive(true);
					crop.transform.parent = null;
					RemoveCrop();
				}
				else
				{
					crop.SetActive(true);
				}
				break;
		}

		//�����۹��� ������ ����
		cropState = stateSwitch;
	}

	public void RemoveCrop()
	{ 
		LandManager.Instance.DeregisterCrop(landID);
		Destroy(this.gameObject);
	}

	public void Regrow()
	{
		//�ٽ� �ڶ�� �ð��� ����Ͽ� �ִ� �ڶ��ð� - seedling�ܰ迡�� crop�ܰ���� ���½ð��� ���ش�.
		int regrowtime = TimeManage.DaysToHours(CropGrow.daysToRegrow);

		growth = maxgrowth - TimeManage.HourToMinute(regrowtime);

		SwitchState(CropState.Seedling);

		//LandManager.Instance.OnCropStateChange(landID, cropState, growth);
	}


}
