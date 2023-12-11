using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandManager : MonoBehaviour
{
	public static LandManager Instance { get; private set; }

	public static Tuple<List<LandSaveState>, List<CropSaveState>> farmData = null;

	List<Ground> landPlots = new List<Ground>();

	List<LandSaveState> landData = new List<LandSaveState>();
	List<CropSaveState> cropData = new List<CropSaveState>();

	//싱글톤 클래스로 설정
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

	void OnEnable()
	{

		RegisterLandPlots();
		StartCoroutine(LoadFarmData());
	}

	//코루틴을 실행해 땅에 진입했을때 현재 땅의 상황을 저장하게끔함
	public IEnumerator LoadFarmData()
	{
		yield return new WaitForEndOfFrame();
		if (farmData != null)
		{
			ImportLandData(farmData.Item1);
			ImportCropData(farmData.Item2);
		}

	}

	private void OnDestroy()
	{
		farmData = new Tuple<List<LandSaveState>, List<CropSaveState>>(landData, cropData);
	}

	void RegisterLandPlots()
	{

		foreach(Transform landTransform in transform)
		{

			Ground land = landTransform.GetComponent<Ground>();
			landPlots.Add(land);

			landData.Add(new LandSaveState());

			land.id = landPlots.Count - 1;
		}
	}

	public void RegisterCrop(int landID, CropManage seedToGrow, CropBehavior.CropState cropState, int growth)
	{
		cropData.Add(new CropSaveState( landID, seedToGrow.name, cropState, growth));
	}

	public void DeregisterCrop(int landID)
	{
		cropData.RemoveAll(x => x.landID == landID);
	}

	//땅의 변경상태를 저장한다.
	public void OnLandStateChange(int id, Ground.Grounded landStatus, TimeManage lastWatered)
	{
		landData[id] = new LandSaveState(landStatus, lastWatered);
	}

	public void OnCropStateChange(int landID, CropBehavior.CropState cropState, int growth)
	{
		int cropIndex =  cropData.FindIndex(x => x.landID == landID);

		string seedToGrow = cropData[cropIndex].seedToGrow;

		cropData[cropIndex] = new CropSaveState(landID, seedToGrow, cropState, growth);
	}

	//팜데이터를 저장한다.
	public void ImportLandData(List<LandSaveState> landDatasetToLoad)
	{
		for(int i = 0; i < landDatasetToLoad.Count; i++)
		{
			LandSaveState landDataToLoad = landDatasetToLoad[i];
			landPlots[i].LoadLandData(landDataToLoad.landStatus, landDataToLoad.lastWatered);
		}

		landData = landDatasetToLoad;
	}
	
	public void ImportCropData(List<CropSaveState> cropDatasetToLoad)
	{
		foreach (CropSaveState cropSave in cropDatasetToLoad)
		{
			Ground landToPlant = landPlots[cropSave.landID];

			CropBehavior cropToPlant = landToPlant.SpawnCrop();

			CropManage seedToGrow = (CropManage)Inventory.Instance.ItemIndex.GetItemFromString(cropSave.seedToGrow);
			cropToPlant.LoadCrop(cropSave.landID, seedToGrow, cropSave.cropState, cropSave.growth);

		}
		cropData = cropDatasetToLoad;
	}

	void Update()
	{
		
	}
}
