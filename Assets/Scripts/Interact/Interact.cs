using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    Player Player; // 플레이어의 정보를 가져온다.

    Ground Groundselect = null;

    InteractableObject selectedItem = null;

    // Start is called before the first frame update
    void Start()
    {
        Player = transform.parent.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit Interacthit;     //레이캐스트를 사용해서 인터렉트키 E를 사용할수있게한다
        if(Physics.Raycast(transform.position, Vector3.down,out Interacthit, 1))        //raycast를 통해 interact object 아래한칸까지 확인
		{
            InteractHit(Interacthit);   //히트 함수 적용
		}


    }

    void InteractHit(RaycastHit hit)
	{
        Collider hitcast = hit.collider;
        if(hitcast.tag == "Ground" && !Inventory.Instance.SlotEquipped(InventorySlot.Inventorytype.Item))
		{
            Ground ground = hitcast.GetComponent<Ground>();
            SelectedGround(ground);
            return;
		}

        if(hitcast.tag == "Item")
		{
            selectedItem = hitcast.GetComponent<InteractableObject>();
            return;
		}

        if(selectedItem != null)
		{
            selectedItem = null;
		}

        if(Groundselect != null)
		{
            Groundselect.SelectedGround(false);
            Groundselect = null;

        }
	}

    void SelectedGround(Ground ground)
	{
        if (Groundselect != null)
        {
            Groundselect.SelectedGround(false);

        }


        if (Inventory.Instance.SlotEquipped(InventorySlot.Inventorytype.Item))
		{
            return;
		}

        //
      

        Groundselect = ground;
        ground.SelectedGround(true);
	}

    //상호작용 시스템 
    public void InteractSystem()
	{
        

        //플레이어가 땅을 선택할때
        if (Groundselect != null)
        {
            Groundselect.GroundInteract();
            return;
        }


        //땅에 있지 않을때 상호작용을 할경우
        else
        {
            Debug.Log("땅에 있지 않습니다.");
        }
	}

    public void ItemInteract()
    {
        
        if(selectedItem != null)
		{
            selectedItem.Pickup();


		}
    }

    public void ItemKeep()
	{
        //플레이어가 무언가를 들고있다면 해당 아이템을 넣습니다.
        if (Inventory.Instance.SlotEquipped(InventorySlot.Inventorytype.Item))
        {
            Inventory.Instance.HandGoInventory(InventorySlot.Inventorytype.Item);
            return;
        }
    }
}
