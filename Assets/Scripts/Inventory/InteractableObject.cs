using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    //아이템의 정보만 가지고있고, 아이템 구분하는데 사용
    public Itemdata item;


    //집기 함수
    public virtual void Pickup()
	{
        

        //플레이어 아이템 장착에 현재 아이템을 넣고
        Inventory.Instance.EquipHandSlot(item);

        //손에 현재아이템을 쥐어줌
        Inventory.Instance.RenderHand();

        //여러개를 못들게 하기위해 만들어둔 게임오브젝트 제거
        Destroy(gameObject);
	}
}
