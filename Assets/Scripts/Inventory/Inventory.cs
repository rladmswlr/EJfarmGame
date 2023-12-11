using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }


    //싱글톤 클래스로 설정
	private void Awake()
	{
        //이것 이외에 한개 더 있을경우 오브젝트 삭제
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
    //아이템슬롯
    [SerializeField]private ItemSlotdata[] toolSlots = new ItemSlotdata[9];

    //장착한 아이템
    [SerializeField] private ItemSlotdata equipToolSlot = null;


    [Header ("Item")]
    //아이템슬롯
    [SerializeField] private ItemSlotdata[] ItemsSlots = new ItemSlotdata[9];

    //장착한 아이템
    [SerializeField] private ItemSlotdata equipItemSlot = null;

    //아이템을 장착할 포인트
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
        //장착한 아이템
        ItemSlotdata handToEquip = equipToolSlot;

        //아이템 슬롯들
        ItemSlotdata[] inventoryToAlter = toolSlots;

        if(InventoryType == InventorySlot.Inventorytype.Item)
		{
            //장착한 아이템 변경
            handToEquip = equipItemSlot;
            inventoryToAlter = ItemsSlots;
		}

        //아이템을 쌓을수 있을때
        if(handToEquip.Stackable(inventoryToAlter[slotNum]))
		{
            ItemSlotdata slotToAlter = inventoryToAlter[slotNum];

            //아이템갯수를 한개올림
            handToEquip.AddQuantity(slotToAlter.quantity);

            //들고있는 아이템 비움
            slotToAlter.Empty();
		}

        //아이템을 쌓을수 없을경우
        else
		{
            //인벤토리 정보 변경
            ItemSlotdata slotToEquip = new ItemSlotdata(inventoryToAlter[slotNum]);

            //인벤토리의 정보를 핸드슬롯으로 변경
            inventoryToAlter[slotNum] = new ItemSlotdata(handToEquip);

            EquipHandSlot(slotToEquip);

            //슬롯아이템 정보를 장착한 아이템으로 변경
            //handToEquip = slotToEquip;
		}

        //아이템 장착씬 렌더
        if (InventoryType == InventorySlot.Inventorytype.Item)
        {
            RenderHand();
        }
        //인벤토리를 다시 렌더링함
        UIManager.Instance.DrawInventory();
    }
    
    public void HandGoInventory(InventorySlot.Inventorytype InventoryType)
	{
        //장착한 아이템
        ItemSlotdata handSlot = equipToolSlot;

        //아이템 슬롯들
        ItemSlotdata[] inventoryToAlter = toolSlots;

        if (InventoryType == InventorySlot.Inventorytype.Item)
        {
            handSlot = equipItemSlot;
            inventoryToAlter = ItemsSlots;
        }

        if (!StackItemToInventory(handSlot, inventoryToAlter))
        {
            //장착아이템을 인벤토리슬롯에 넣기위해 가장 첫번째 비어있는 아이템을 찾는다.
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    //장착한 아이템을 슬롯에 넣는다.
                    inventoryToAlter[i] = new ItemSlotdata(handSlot);
                    //현재 장착한 아이템을 비우게 함
                    handSlot.Empty();
                    break;
                }
            }
        }

        //아이템 장착씬 렌더
        if (InventoryType == InventorySlot.Inventorytype.Item)
        {
            RenderHand();
        }
        //인벤토리를 다시 렌더링함
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
            //장착아이템을 인벤토리슬롯에 넣기위해 가장 첫번째 비어있는 아이템을 찾는다.
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    //장착한 아이템을 슬롯에 넣는다.
                    inventoryToAlter[i] = new ItemSlotdata(itemSlotToMove);
                    break;
                }
            }
        }

        //인벤토리를 다시 렌더링함
        UIManager.Instance.DrawInventory();
        RenderHand();
    }

    public void RenderHand()
	{
        //만약 핸드포인트의 자식이있으면 -> 아이템을 들고있다면
        if(handPoint.childCount > 0)
		{
            Destroy(handPoint.GetChild(0).gameObject);
		}

        //아이템을 장착하고있지않은경우
        if(SlotEquipped(InventorySlot.Inventorytype.Item))
        {
            //장착한 아이템을 인스턴스화시킴
            Instantiate(GetEquippedSlotItem(InventorySlot.Inventorytype.Item).Object, handPoint);


            //Instantiate(GetEquippedSlotItem(InventorySlot.Inventorytype.Item).gamemodel, handPoint);
                handPoint.GetChild(0).gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
	}

    #region 아이템 장착확인 , 장착아이템 데이터정보;

    //인벤토리 타입반환
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

    //아이템을 장착하고있는지 체크
    public bool SlotEquipped(InventorySlot.Inventorytype inventorytype)
	{
        if (inventorytype == InventorySlot.Inventorytype.Item)
        {
            return !equipItemSlot.IsEmpty();
        }

        return !equipToolSlot.IsEmpty();
    }

    //장착한게 장비인지 확인
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


	#region 아이템 갯수확인
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
            Debug.LogError("작물을 더이상 가지고있지않음");
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

    //인벤토리 값이 변경될때마다 벨리데이터를 이용해 확인함
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
