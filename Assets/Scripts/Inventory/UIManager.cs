using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, TimeTracker
{
    public static UIManager Instance { get; private set; }
    [Header("Status Bar")]
    //���� �����ϰ��ִ� ���
    public Image nowtoolEquip;
    public Text toolQuantityText;

    public Text timeText;
    public Text dateText;

    [Header("Inventory System")]
    public InventorySlot[] toolSlots;
    public InventorySlot[] ItemSlots;

    public HandInventorySlot HandEquiptool;
    public HandInventorySlot HandEquipitem;

    [Header("�������� �ڽ��ȿ��������")]
    //�������� �ڽ��ȿ� ���ö�
    public GameObject itemInfoBox;
    public Text ItemNameText;
    public Text ItemDescText;

    [Header("������ ��ư����")]
    public YesNoPrompt yesNoPrompt;

    [Header("������")]
    public Text moneyText;

    [Header("��������")]
    public ShopListingManager shopListingManager;

    //�̱��� Ŭ������ ����
    private void Awake()
    {
        //�̰� �̿ܿ� �Ѱ� �� ������� ������Ʈ ����
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

        //����Ʈ�� �ش� ������ �߰��Ѵ�.
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
        //������ ������ �����´�

        ItemSlotdata[] InventorytoolSlots = Inventory.Instance.GetInventorySlots(InventorySlot.Inventorytype.Tool);
        ItemSlotdata[] InventoryItemSlots = Inventory.Instance.GetInventorySlots(InventorySlot.Inventorytype.Item);

        //�������� �׸���.
        DrawInventoryPanal(InventorytoolSlots, toolSlots);

        //�����ۿ����� �׸���.
        DrawInventoryPanal(InventoryItemSlots, ItemSlots);

        //���� ������ �������� ���÷��̸� ���Ѵ�.
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
            //������ �׸���.
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

    //UI�� �ð��� �����ϰ� ���ֱ����� �Լ�
    public void ClockUpdate(TimeManage timemanage)
	{
        //Ÿ���� ������ 24�ð��� ������ 12�ð��� AM , PM���� ������ ���� prefix �߰�

        int hours = timemanage.hour;
        int minutes = timemanage.minute;

        string prefix = "AM ";

        //�ð��� 12�ð� ������ AM�� PM���� ��������
        if(hours > 12)
		{
            prefix = "PM ";
            hours -= 12;
		}
        //�ð��� �ؽ�Ʈ ���濡 ���缭 �ٲ���
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
