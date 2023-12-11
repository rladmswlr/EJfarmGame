using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Crops")]

public class CropManage : Itemdata
{
    public int GrowTime;    // 작물이 자라는데 걸리는 시간

    public Itemdata Output;

    //씨앗의 자라는 과정 오브젝트
    public GameObject seed;
    public GameObject seedling;
    public GameObject Harvested;

    [Header("다시자라는식물")]
    public bool regrowble;

    public int daysToRegrow;
}
