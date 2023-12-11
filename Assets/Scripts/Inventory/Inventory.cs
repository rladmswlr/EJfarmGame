using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }


    //�̱��� Ŭ������ ����
	private void Awake()
	{
        //�̰� �̿ܿ� �Ѱ� �� ������� ������Ʈ ����
        if(Instance != null && Instance != this)
		{
            Destroy(this);
		}

        else
		{
            Instance = this;
		}
	}

    public ItemIndex ItemIndex;

    [Header("Tool")]
    //�����۽���
    [SerializeField]private ItemSlotdata[] toolSlots = new ItemSlotdata[9];

    //������ ������
    [SerializeField] private ItemSlotdata equipToolSlot = null;


    [Header ("Item")]
    //�����۽���
    [SerializeField] private ItemSlotdata[] ItemsSlots = new ItemSlotdata[9];

    //������ ������
    [SerializeField] private ItemSlotdata equipItemSlot = null;

    //�������� ������ ����Ʈ
    public Transform handPoint;

    public void LoadInventory(ItemSlotdata[] toolSlots, ItemSlotdata equippedToolSlot, ItemSlotdata[] itemSlots, ItemSlotdata equippedItemSlot)
	{
        this.toolSlots = toolSlots;
        this.equipToolSlot = equippedToolSlot;
        this.ItemsSlots = itemSlots;
        this.equipItemSlot = equippedItemSlot;

        UIManager.Instance.DrawInventory();
	}
    
    public void InventoryGoHand(int slotNum, InventorySlot.Inventorytype InventoryType)
	{
        //������ ������
        ItemSlotdata handToEquip = equipToolSlot;

        //������ ���Ե�
        ItemSlotdata[] inventoryToAlter = toolSlots;

        if(InventoryType == InventorySlot.Inventorytype.Item)
		{
            //������ ������ ����
            handToEquip = equipItemSlot;
            inventoryToAlter = ItemsSlots;
		}

        //�������� ������ ������
        if(handToEquip.Stackable(inventoryToAlter[slotNum]))
		{
            ItemSlotdata slotToAlter = inventoryToAlter[slotNum];

            //�����۰����� �Ѱ��ø�
            handToEquip.AddQuantity(slotToAlter.quantity);

            //����ִ� ������ ���
            slotToAlter.Empty();
		}

        //�������� ������ �������
        else
		{
            //�κ��丮 ���� ����
            ItemSlotdata slotToEquip = new ItemSlotdata(inventoryToAlter[slotNum]);

            //�κ��丮�� ������ �ڵ彽������ ����
            inventoryToAlter[slotNum] = new ItemSlotdata(handToEquip);

            EquipHandSlot(slotToEquip);

            //���Ծ����� ������ ������ ���������� ����
            //handToEquip = slotToEquip;
		}

        //������ ������ ����
        if (InventoryType == InventorySlot.Inventorytype.Item)
        {
            RenderHand();
        }
        //�κ��丮�� �ٽ� ��������
        UIManager.Instance.DrawInventory();
    }
    
    public void HandGoInventory(InventorySlot.Inventorytype InventoryType)
	{
        //������ ������
        ItemSlotdata handSlot = equipToolSlot;

        //������ ���Ե�
        ItemSlotdata[] inventoryToAlter = toolSlots;

        if (InventoryType == InventorySlot.Inventorytype.Item)
        {
            handSlot = equipItemSlot;
            inventoryToAlter = ItemsSlots;
        }

        if (!StackItemToInventory(handSlot, inventoryToAlter))
        {
            //������������ �κ��丮���Կ� �ֱ����� ���� ù��° ����ִ� �������� ã�´�.
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    //������ �������� ���Կ� �ִ´�.
                    inventoryToAlter[i] = new ItemSlotdata(handSlot);
                    //���� ������ �������� ���� ��
                    handSlot.Empty();
                    break;
                }
            }
        }

        //������ ������ ����
        if (InventoryType == InventorySlot.Inventorytype.Item)
        {
            RenderHand();
        }
        //�κ��丮�� �ٽ� ��������
        UIManager.Instance.DrawInventory();

    }
    
    public bool StackItemToInventory(ItemSlotdata itemSlot, ItemSlotdata[] inventoryArray)
	{
        for(int i = 0; i < inventoryArray.Length; i++)
		{
            if(inventoryArray[i].Stackable(itemSlot))
			{
                inventoryArray[i].AddQuantity(itemSlot.quantity);

                itemSlot.Empty();

                return true;
			}
		}

        return false;
	}

    public void ShopToInventory(ItemSlotdata itemSlotToMove)
	{
        ItemSlotdata[] inventoryToAlter = IsTool(itemSlotToMove.itemdata) ? toolSlots : ItemsSlots;

        if (!StackItemToInventory(itemSlotToMove, inventoryToAlter))
        {
            //������������ �κ��丮���Կ� �ֱ����� ���� ù��° ����ִ� �������� ã�´�.
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    //������ �������� ���Կ� �ִ´�.
                    inventoryToAlter[i] = new ItemSlotdata(itemSlotToMove);
                    break;
                }
            }
        }

        //�κ��丮�� �ٽ� ��������
        UIManager.Instance.DrawInventory();
        RenderHand();
    }

    public void RenderHand()
	{
        //���� �ڵ�����Ʈ�� �ڽ��������� -> �������� ����ִٸ�
        if(handPoint.childCount > 0)
		{
            Destroy(handPoint.GetChild(0).gameObject);
		}

        //�������� �����ϰ������������
        if(SlotEquipped(InventorySlot.Inventorytype.Item))
        {
            //������ �������� �ν��Ͻ�ȭ��Ŵ
            Instantiate(GetEquippedSlotItem(InventorySlot.Inventorytype.Item).Object, handPoint);


            //Instantiate(GetEquippedSlotItem(InventorySlot.Inventorytype.Item).gamemodel, handPoint);
                handPoint.GetChild(0).gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
	}

    #region ������ ����Ȯ�� , ���������� ����������;

    //�κ��丮 Ÿ�Թ�ȯ
    public Itemdata GetEquippedSlotItem(InventorySlot.Inventorytype inventorytype)
	{
        if(inventorytype == InventorySlot.Inventorytype.Item)
		{
            return equipItemSlot.itemdata;
		}

        return equipToolSlot.itemdata;

	}

    public ItemSlotdata GetEquippedSlot(InventorySlot.Inventorytype inventorytype)
	{
        if (inventorytype == InventorySlot.Inventorytype.Item)
        {
            return equipItemSlot;
        }

        return equipToolSlot;
    }

    public ItemSlotdata[] GetInventorySlots(InventorySlot.Inventorytype inventorytype)
	{
        if (inventorytype == InventorySlot.Inventorytype.Item)
        {
            return ItemsSlots;
        }

        return toolSlots;
    }

    //�������� �����ϰ��ִ��� üũ
    public bool SlotEquipped(InventorySlot.Inventorytype inventorytype)
	{
        if (inventorytype == InventorySlot.Inventorytype.Item)
        {
            return !equipItemSlot.IsEmpty();
        }

        return !equipToolSlot.IsEmpty();
    }

    //�����Ѱ� ������� Ȯ��
    public bool IsTool(Itemdata item)
	{
        EquipItemData equipment = item as EquipItemData;
        if(equipment != null)
		{
            return true;
		}

        CropManage seed = item as CropManage;
        return seed != null;
	}

	#endregion


	#region ������ ����Ȯ��
	public void EquipHandSlot(Itemdata item)
	{
        if(IsTool(item))
		{
            equipToolSlot = new ItemSlotdata(item);
		}

		else
		{
            equipItemSlot = new ItemSlotdata(item);
		}
	}

    public void EquipHandSlot(ItemSlotdata itemSlot)
    {
        Itemdata item = itemSlot.itemdata;

        if (IsTool(item))
        {
            equipToolSlot = new ItemSlotdata(itemSlot);
        }

        else
        {
            equipItemSlot = new ItemSlotdata(itemSlot);
        }
    }

    public void ConsumeItem(ItemSlotdata itemSlot)
	{
        if(itemSlot.IsEmpty())
		{
            Debug.LogError("�۹��� ���̻� ��������������");
            return;
		}

        itemSlot.Remove();

        RenderHand();
        UIManager.Instance.DrawInventory();
	}


    private void OnValidate()
	{
        ValidateInventorySlots(equipToolSlot);
        ValidateInventorySlots(equipItemSlot);

        ValidateInventorySlots(ItemsSlots);
        ValidateInventorySlots(toolSlots);

    }

    //�κ��丮 ���� ����ɶ����� ���������͸� �̿��� Ȯ����
    void ValidateInventorySlots(ItemSlotdata slot)
	{
        if (slot.itemdata != null && slot.quantity == 0)
        {
            slot.quantity = 1;
        }
	}

    void ValidateInventorySlots(ItemSlotdata[] array)
	{
        foreach(ItemSlotdata slot in array)
		{
            ValidateInventorySlots(slot);
		}
	}

	#endregion 

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
