using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Crops")]

public class CropManage : Itemdata
{
    public int GrowTime;    // �۹��� �ڶ�µ� �ɸ��� �ð�

    public Itemdata Output;

    //������ �ڶ�� ���� ������Ʈ
    public GameObject seed;
    public GameObject seedling;
    public GameObject Harvested;

    [Header("�ٽ��ڶ�½Ĺ�")]
    public bool regrowble;

    public int daysToRegrow;
}
