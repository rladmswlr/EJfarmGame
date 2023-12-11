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

	//�̱��� Ŭ������ ����
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

	void OnEnable()
	{

		RegisterLandPlots();
		StartCoroutine(LoadFarmData());
	}

	//�ڷ�ƾ�� ������ ���� ���������� ���� ���� ��Ȳ�� �����ϰԲ���
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

	//���� ������¸� �����Ѵ�.
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

	//�ʵ����͸� �����Ѵ�.
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
