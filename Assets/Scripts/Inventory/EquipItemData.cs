using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���� ������ ���� ���� ����
[CreateAssetMenu(menuName = "Items/Equip")]
//������ ���� ����Ÿ ���� �����۵����͸� ��ӹ���
public class EquipItemData : Itemdata
{
    public enum ToolType
	{
		Rake, WateringCan
	}

	public ToolType tooltype;
}
