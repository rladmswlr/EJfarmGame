using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, TimeTracker
{
    public static UIManager Instance { get; private set; }
    [Header("Status Bar")]
    //현재 장착하고있는 장비
    public Image nowtoolEquip;
    public Text toolQuantityText;

    public Text timeText;
    public Text dateText;

    [Header("Inventory System")]
    public InventorySlot[] toolSlots;
    public InventorySlot[] ItemSlots;

    public HandInventorySlot HandEquiptool;
    public HandInventorySlot HandEquipitem;

    [Header("아이템이 박스안에있을경우")]
    //아이템이 박스안에 들어올때
    public GameObject itemInfoBox;
    public Text ItemNameText;
    public Text ItemDescText;

    [Header("선택지 버튼관련")]
    public YesNoPrompt yesNoPrompt;

    [Header("돈관련")]
    public Text moneyText;

    [Header("상점관련")]
    public ShopListingManager shopListingManager;

    //싱글톤 클래스로 설정
    private void Awake()
    {
        //이것 이외에 한개 더 있을경우 오브젝트 삭제
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        else
        {
            Instance = this;
        }
    }

	private void Start()
    { 
        DrawInventory();
        AssignSlotIndexes();
        RenderPlayerStats();
        DisplayItemInfo(null); 

        //리스트에 해당 정보를 추가한다.
        TimeManager.Instance.RegisterTrack(this);
    }

    public void TriggerYesNoPrompt(string message, System.Action onYesCallback)
	{
        yesNoPrompt.gameObject.SetActive(true);

        yesNoPrompt.CreatePrompt(message, onYesCallback);
	}

    public void AssignSlotIndexes()
	{
        for (int i = 0; i < toolSlots.Length; i++)
		{
            toolSlots[i].CheckSlotNum(i);
            ItemSlots[i].CheckSlotNum(i);
        }
	}

	public void DrawInventory()
	{
        //슬롯의 정보를 가져온다

        ItemSlotdata[] InventorytoolSlots = Inventory.Instance.GetInventorySlots(InventorySlot.Inventorytype.Tool);
        ItemSlotdata[] InventoryItemSlots = Inventory.Instance.GetInventorySlots(InventorySlot.Inventorytype.Item);

        //툴영역을 그린다.
        DrawInventoryPanal(InventorytoolSlots, toolSlots);

        //아이템영역을 그린다.
        DrawInventoryPanal(InventoryItemSlots, ItemSlots);

        //현재 장착한 아이템의 디스플레이를 구한다.
        HandEquiptool.Display(Inventory.Instance.GetEquippedSlot(InventorySlot.Inventorytype.Tool));
        HandEquipitem.Display(Inventory.Instance.GetEquippedSlot(InventorySlot.Inventorytype.Item));

        Itemdata equippedTool = Inventory.Instance.GetEquippedSlotItem(InventorySlot.Inventorytype.Tool);

        toolQuantityText.text = "";

        if (equippedTool != null)
        {
            nowtoolEquip.sprite = equippedTool.Drawing;
         

            nowtoolEquip.gameObject.SetActive(true);

            int quantity = Inventory.Instance.GetEquippedSlot(InventorySlot.Inventorytype.Tool).quantity;

            if (quantity > 1)
			{
                toolQuantityText.text = quantity.ToString();
			}

            return;
        }

        nowtoolEquip.gameObject.SetActive(false);


    }

    void DrawInventoryPanal(ItemSlotdata[] slots, InventorySlot[] uiSlots)
	{
        for (int i = 0; i < uiSlots.Length; i++)
        {
            //슬롯을 그린다.
            uiSlots[i].Display(slots[i]);
        }

    }

    public void DisplayItemInfo(Itemdata data)
	{
        if(data == null)
		{
            ItemNameText.text = "";
            ItemDescText.text = "";

            itemInfoBox.SetActive(false);
            return;
		}

        itemInfoBox.SetActive(true);
        ItemNameText.text = data.name;
        ItemDescText.text = data.Desc;

	}

    //UI에 시간을 변경하게 해주기위한 함수
    public void ClockUpdate(TimeManage timemanage)
	{
        //타임을 관리함 24시간을 쓰지만 12시간씩 AM , PM으로 나누기 위해 prefix 추가

        int hours = timemanage.hour;
        int minutes = timemanage.minute;

        string prefix = "AM ";

        //시간이 12시가 지나면 AM을 PM으로 변경해줌
        if(hours > 12)
		{
            prefix = "PM ";
            hours -= 12;
		}
        //시간을 텍스트 변경에 맞춰서 바꿔줌
        timeText.text = prefix + hours.ToString("00") + ":"+ minutes.ToString("00");

        int day = timemanage.day;
        string season = timemanage.season.ToString();
        string dayOfTheWeek = timemanage.GetDayWeek().ToString();

        dateText.text = season + " " + day + " ( " + dayOfTheWeek + " )";
	}

    public void RenderPlayerStats()
	{
        moneyText.text = PlayerStats.Money + PlayerStats.CURRENCY;
	}

    public void OpenShop(List<Itemdata> shopItems)
	{
        shopListingManager.gameObject.SetActive(true);
        shopListingManager.RenderShop(shopItems);
	}
} 
