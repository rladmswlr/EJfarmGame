using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehavior : MonoBehaviour
{
	int landID;

	public static CropBehavior Instance { get; private set; }

	//작물이 얼마나 자라는지 알려주기위한 클래스
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
		//이것 이외에 한개 더 있을경우 오브젝트 삭제
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}

		else
		{
			Instance = this;
		}
	}

	//이니셜라이징 작업
	public void Plant(int landID, CropManage CropGrow)
	{
		this.landID = landID;

		//해당 작물의 정보를 저장
		this.CropGrow = CropGrow;

		//seed = Instantiate(CropGrow.seed, transform);

		//성장중인 작물의 인스턴스화
		seedling = Instantiate(CropGrow.seedling, transform);

		// 결과물의 오브젝트 인스턴스화
		Itemdata cropToDay = CropGrow.Output;

		//crop = Instantiate(cropToDay.Object, transform);

		crop = Instantiate(CropGrow.Harvested, transform);

		//최대 자라는게 몇시간이 걸리는지 계산
		int hoursToGrow = TimeManage.DaysToHours(CropGrow.GrowTime);
		// 몇분으로 자라는지 계산
		maxgrowth = TimeManage.HourToMinute(hoursToGrow);

		if (CropGrow.regrowble)
		{
			ReGrowCropBehave regrowcrop = crop.GetComponent<ReGrowCropBehave>();

			//해당작물이 무엇인지 세팅한다.
			regrowcrop.setParent(this);
		}

		//초기 식물의 설정을 Seed로 해결함
		SwitchState(cropState);
		LandManager.Instance.RegisterCrop(landID, CropGrow, cropState, growth);
	}

	public void LoadCrop(int landID, CropManage seedToGrow, CropBehavior.CropState cropState, int growth)
	{
		this.landID = landID;

		//해당 작물의 정보를 저장
		this.CropGrow = seedToGrow;

		//seed = Instantiate(CropGrow.seed, transform);

		//성장중인 작물의 인스턴스화
		seedling = Instantiate(CropGrow.seedling, transform);

		// 결과물의 오브젝트 인스턴스화
		Itemdata cropToDay = CropGrow.Output;

		//crop = Instantiate(cropToDay.Object, transform);

		crop = Instantiate(CropGrow.Harvested, transform);

		//최대 자라는게 몇시간이 걸리는지 계산
		int hoursToGrow = TimeManage.DaysToHours(CropGrow.GrowTime);
		// 몇분으로 자라는지 계산
		maxgrowth = TimeManage.HourToMinute(hoursToGrow);

		this.growth = growth;

		if (CropGrow.regrowble)
		{
			ReGrowCropBehave regrowcrop = crop.GetComponent<ReGrowCropBehave>();

			//해당작물이 무엇인지 세팅한다.
			regrowcrop.setParent(this);
		}

		//초기 식물의 설정을 Seed로 해결함
		SwitchState(cropState);
	}

	public void Grow()
	{
		//계속 작물을 성장시킨다.
		growth++;

		//최대 작물 자라는시간의 50%에 도달할경우 seedling 단계에 진입
		if (growth >= maxgrowth / 2 && cropState == CropState.Seed)
		{
			SwitchState(CropState.Seedling);
			LandManager.Instance.OnCropStateChange(landID, cropState, growth);
		}
		//최고단계에 도착할경우 growth완료
		if(growth >= maxgrowth && cropState == CropState.Seedling)
		{
			SwitchState(CropState.Crop);
		}

	}

	private void SwitchState(CropState stateSwitch)
	{
		//해당작물의 상태를 초기화시킨다.
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

		//현재작물의 정보를 저장
		cropState = stateSwitch;
	}

	public void RemoveCrop()
	{ 
		LandManager.Instance.DeregisterCrop(landID);
		Destroy(this.gameObject);
	}

	public void Regrow()
	{
		//다시 자라는 시간을 계산하여 최대 자란시간 - seedling단계에서 crop단계까지 가는시간을 빼준다.
		int regrowtime = TimeManage.DaysToHours(CropGrow.daysToRegrow);

		growth = maxgrowth - TimeManage.HourToMinute(regrowtime);

		SwitchState(CropState.Seedling);

		//LandManager.Instance.OnCropStateChange(landID, cropState, growth);
	}


}
