using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//착용 아이템 관리 텝을 만듬
[CreateAssetMenu(menuName = "Items/Equip")]
//아이템 착용 데이타 관리 아이템데이터를 상속받음
public class EquipItemData : Itemdata
{
    public enum ToolType
	{
		Rake, WateringCan
	}

	public ToolType tooltype;
}
