using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveState
{
    public List<LandSaveState> landData;
    public List<CropSaveState> cropData;

    public ItemSlotSaveData[] toolSlots;
    public ItemSlotSaveData[] itemSlots;

    public ItemSlotSaveData equippedItemSlot;
    public ItemSlotSaveData equippedToolSlot;

    public TimeManage timestamp;

    public int money;

    public GameSaveState(List<LandSaveState> landData, List<CropSaveState> cropData, ItemSlotdata[] toolSlots, ItemSlotdata[] itemSlots, ItemSlotdata equippedItemSlot, ItemSlotdata equippedToolSlot, TimeManage timestamp, int money)
	{
        this.landData = landData;
        this.cropData = cropData;
        this.toolSlots = ItemSlotdata.SerializeArray(toolSlots);
        this.itemSlots = ItemSlotdata.SerializeArray(itemSlots);
        this.equippedItemSlot = ItemSlotdata.SerializeData(equippedItemSlot);
        this.equippedToolSlot = ItemSlotdata.SerializeData(equippedToolSlot);
        this.timestamp = timestamp;
        this.money = money;
	}
}                                                                                                                                       
