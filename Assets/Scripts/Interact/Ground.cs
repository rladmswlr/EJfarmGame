using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���� �������� �ִ� Ŭ����

public class Ground : MonoBehaviour, TimeTracker
{
    public int id;
    public enum Grounded
	{
        Grass, Soil, Mud
	}

    public Grounded Groundstatus = Grounded.Grass;

    public Material Grass, Soil, Mud;

    Renderer groundrender;

    public GameObject selectGround;

    TimeManage timeGround;

    [Header("�۹�����")]
    //�۹��� ����������
    public GameObject CropPrefab;

    //�⺻������ ���� �ɱ������� �۹��� ����
    CropBehavior cropPlanted = null;

    // Start is called before the first frame update
    void Start()
    {
        groundrender = GetComponent<Renderer>();

        //�Լ� �ʱ�ȭ �׷����� ���������ϰԲ���
        //SwapGround(Grounded.Grass);

        //Ÿ�ӸŴ����� ���� �ش� �׶����� ������Ʈ�� Ÿ�ӿ� �����Ű�Բ���
        TimeManager.Instance.RegisterTrack(this);
    }

    public void LoadLandData(Grounded statusToSwitch, TimeManage lastWatered)
	{
        Groundstatus = statusToSwitch;
        timeGround = lastWatered;

        Material swapMaterial = Grass;

        if (statusToSwitch == Grounded.Grass)
        {
            swapMaterial = Grass;
        }

        else if (statusToSwitch == Grounded.Soil)
        {
            swapMaterial = Soil;


        }

        else if (statusToSwitch == Grounded.Mud)
        {
            swapMaterial = Mud;
        }

        groundrender.material = swapMaterial;
    }

    //�� Material�� �����ϴ� �Լ�
    public void SwapGround(Grounded swapGround)
	{
        Groundstatus = swapGround;
        Material swapMaterial = Grass;


        if(swapGround == Grounded.Grass)
		{
            swapMaterial = Grass;
		}

		else if(swapGround == Grounded.Soil)
        {
            swapMaterial = Soil;
			

		}

        else if (swapGround == Grounded.Mud)
        {
            swapMaterial = Mud;

            timeGround = TimeManager.Instance.GetGameTimemanage();
        }

        groundrender.material = swapMaterial;

        LandManager.Instance.OnLandStateChange(id, Groundstatus, timeGround);
    }

    //select��� ���� �����׵θ��� Ȱ��ȭ��Ŵ 
    public void SelectedGround(bool selected)
	{
        selectGround.SetActive(selected);
	}


    //���� �ִ��� üũ�ϴ� Interact �Լ�
    public void GroundInteract()
	{
        //SwapGround(Grounded.Soil);

        //���� �÷��̾ ����ִ� ������ Ȯ����
        Itemdata toolSlot = Inventory.Instance.GetEquippedSlotItem(InventorySlot.Inventorytype.Tool);

        if (!Inventory.Instance.SlotEquipped(InventorySlot.Inventorytype.Tool))
		{
            return;
		}

        //���� ������ �������� ������ ������
        EquipItemData nowequipTool = toolSlot as EquipItemData;

        if(nowequipTool  != null)
		{
            //���� �� Ÿ���� Ȯ���մϴ�.
            EquipItemData.ToolType toolType = nowequipTool.tooltype;

            switch(toolType)
			{
                case EquipItemData.ToolType.Rake:
                    SwapGround(Grounded.Soil);
                    break;
                case EquipItemData.ToolType.WateringCan:
                    if(Groundstatus == Grounded.Soil)
                    {
                        SwapGround(Grounded.Mud);
                    }
                    break;
            }

            return;
		}

        CropManage cropTool = toolSlot as CropManage;

        //�۹��� �տ� ������� �ʰ� ���� ���۵Ǿ��ְų� �����̾���ϰ�, ������ �տ� ����������
        if (cropTool != null && Groundstatus != Grounded.Grass && cropPlanted == null)
		{
            SpawnCrop();

            //�ش� ũ������ ����
            cropPlanted.Plant(id, cropTool);
            //cropPlanted.Plant(cropseed);

            Inventory.Instance.ConsumeItem(Inventory.Instance.GetEquippedSlot(InventorySlot.Inventorytype.Tool));

        }
    }

    public CropBehavior SpawnCrop()
	{
        //ũ�ӿ�����Ʈ�� �ν��Ͻ�ȭ��.
        GameObject cropObject = Instantiate(CropPrefab, transform);

        //������ �ν��Ͻ��� ��ġ�� ���̰Բ� ����
        cropObject.transform.position = new Vector3(transform.position.x, 0.527f, transform.position.z);

        cropPlanted = cropObject.GetComponent<CropBehavior>();

        return cropPlanted;
    }

    public void ClockUpdate(TimeManage timemanage)
	{
        if(Groundstatus == Grounded.Mud)
		{
            int CheckTime = TimeManage.CompareTimeManage(timeGround, timemanage);

            //�ð������� �۹��� ����
            if(cropPlanted != null)
			{
                cropPlanted.Grow();
			}

            //24�ð��� �����ٸ� ���붥�� ����������
            if(CheckTime > 24)
			{
                SwapGround(Grounded.Soil);
			}
		}

	}

	private void OnDestroy()
	{
        TimeManager.Instance.UnRegisterTrack(this);
	}

}
