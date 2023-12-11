using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    Player Player; // �÷��̾��� ������ �����´�.

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
        RaycastHit Interacthit;     //����ĳ��Ʈ�� ����ؼ� ���ͷ�ƮŰ E�� ����Ҽ��ְ��Ѵ�
        if(Physics.Raycast(transform.position, Vector3.down,out Interacthit, 1))        //raycast�� ���� interact object �Ʒ���ĭ���� Ȯ��
		{
            InteractHit(Interacthit);   //��Ʈ �Լ� ����
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

    //��ȣ�ۿ� �ý��� 
    public void InteractSystem()
	{
        

        //�÷��̾ ���� �����Ҷ�
        if (Groundselect != null)
        {
            Groundselect.GroundInteract();
            return;
        }


        //���� ���� ������ ��ȣ�ۿ��� �Ұ��
        else
        {
            Debug.Log("���� ���� �ʽ��ϴ�.");
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
        //�÷��̾ ���𰡸� ����ִٸ� �ش� �������� �ֽ��ϴ�.
        if (Inventory.Instance.SlotEquipped(InventorySlot.Inventorytype.Item))
        {
            Inventory.Instance.HandGoInventory(InventorySlot.Inventorytype.Item);
            return;
        }
    }
}
