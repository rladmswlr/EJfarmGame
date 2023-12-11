using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour, TimeTracker
{
	public static GameState Instance { get; private set; }


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	void Start()
	{
		TimeManager.Instance.RegisterTrack(this);
	}

	public void ClockUpdate(TimeManage timestamp)
	{
		UpdateShippingState(timestamp);
		UpdateFarmState(timestamp);
	}

	void UpdateShippingState(TimeManage timestamp)
	{
		if(timestamp.hour == ShippingBin.hourToShop && timestamp.minute == 0)
		{
			ShippingBin.ShipItem();
		}
	}

	void UpdateFarmState(TimeManage timestamp)
	{
		if (SceneTrans.Instance.currentLocation != SceneTrans.Location.MainGame)
		{
			if (LandManager.farmData == null)
			{
				return;
			}

			List<LandSaveState> landData = LandManager.farmData.Item1;
			List<CropSaveState> cropData = LandManager.farmData.Item2;

			if (cropData.Count == 0) return;

			for (int i = 0; i < cropData.Count; i++)
			{
				CropSaveState crop = cropData[i];
				LandSaveState land = landData[crop.landID];


				land.ClockUpdate(timestamp);

				if (land.landStatus == Ground.Grounded.Mud && crop.cropState != CropBehavior.CropState.Crop)
				{
					crop.Grow();
				}

				cropData[i] = crop;
				landData[crop.landID] = land;
			}

			//LandManager.farmData.Item2.ForEach((CropSaveState crop) =>
			//{
			//	Debug.Log(crop.seedToGrow + "/n Growth : " + crop.growth + "/n State: " + crop.cropState.ToString());
			//});
		}
	}

	public void Sleep()
	{
		TimeManage timestampOfNextDay = TimeManager.Instance.GetGameTimemanage();
		timestampOfNextDay.day += 1;
		timestampOfNextDay.hour = 6;
		timestampOfNextDay.minute = 0;

		TimeManager.Instance.SkipTime(timestampOfNextDay);

		SaveManager.Save(ExportSaveState());

	}

	public GameSaveState ExportSaveState()
	{
		List<LandSaveState> landData = LandManager.farmData.Item1;
		List<CropSaveState> cropData = LandManager.farmData.Item2;

		ItemSlotdata[] toolSlots = Inventory.Instance.GetInventorySlots(InventorySlot.Inventorytype.Tool);
		ItemSlotdata[] itemSlots = Inventory.Instance.GetInventorySlots(InventorySlot.Inventorytype.Item);

		ItemSlotdata equippedToolSlot = Inventory.Instance.GetEquippedSlot(InventorySlot.Inventorytype.Tool);
		ItemSlotdata equippedItemSlot = Inventory.Instance.GetEquippedSlot(InventorySlot.Inventorytype.Item);

		TimeManage timestamp = TimeManager.Instance.GetGameTimemanage();
		return new GameSaveState(landData, cropData, toolSlots, itemSlots, equippedItemSlot, equippedToolSlot, timestamp, PlayerStats.Money);
	}

	public void LoadSave()
	{
		GameSaveState save = SaveManager.Load();

		SceneTrans.Instance.SwitchLocation(SceneTrans.Location.MainGame);

		TimeManager.Instance.LoadTime(save.timestamp);

		ItemSlotdata[] toolSlots = ItemSlotdata.DeserializeArray(save.toolSlots);
		ItemSlotdata equippedToolSlot = ItemSlotdata.DeserializeData(save.equippedToolSlot); 
		ItemSlotdata[] itemSlots = ItemSlotdata.DeserializeArray(save.itemSlots);
		ItemSlotdata equippedItemSlot = ItemSlotdata.DeserializeData(save.equippedItemSlot);
		Inventory.Instance.LoadInventory(toolSlots, equippedToolSlot, itemSlots, equippedItemSlot);

		LandManager.farmData = new System.Tuple<List<LandSaveState>, List<CropSaveState>>(save.landData, save.cropData);

		PlayerStats.LoadStats(save.money);

	}
}
