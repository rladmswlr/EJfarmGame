using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//에셋메뉴를 추가한다. Menu이름 : Item 으로 하여 인벤토리 관리를 쉽게한다.
[CreateAssetMenu(menuName = "Items/Item")]


// monobehaviour을 자동 상속받는것을 지우고 ScriptableObject를 상속받는다
// 이방법으로 조금더 쉽게 아이템 data에 대한 설명을 할수있다.
public class Itemdata : ScriptableObject
{
	//인벤토리에 필요한 그림
	public Sprite Drawing;

	//아이템에 대한 설명
	public string Desc;

	//게임 오브젝트
	public GameObject Object;

	public int cost;
}
