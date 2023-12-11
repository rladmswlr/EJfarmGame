using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    //�������� ������ �������ְ�, ������ �����ϴµ� ���
    public Itemdata item;


    //���� �Լ�
    public virtual void Pickup()
	{
        

        //�÷��̾� ������ ������ ���� �������� �ְ�
        Inventory.Instance.EquipHandSlot(item);

        //�տ� ����������� �����
        Inventory.Instance.RenderHand();

        //�������� ����� �ϱ����� ������ ���ӿ�����Ʈ ����
        Destroy(gameObject);
	}
}
