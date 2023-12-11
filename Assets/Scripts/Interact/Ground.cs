using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//땅의 변경점을 주는 클래스

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

    [Header("작물정보")]
    //작물의 프리팹저장
    public GameObject CropPrefab;

    //기본적으로 땅에 심기전까진 작물이 없음
    CropBehavior cropPlanted = null;

    // Start is called before the first frame update
    void Start()
    {
        groundrender = GetComponent<Renderer>();

        //함수 초기화 그래스로 먼저시작하게끔함
        //SwapGround(Grounded.Grass);

        //타임매니저를 통해 해당 그라운드의 오브젝트를 타임에 적용시키게끔함
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

    //땅 Material을 변경하는 함수
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

    //select라는 투명 빨간테두리를 활성화시킴 
    public void SelectedGround(bool selected)
	{
        selectGround.SetActive(selected);
	}


    //땅에 있는지 체크하는 Interact 함수
    public void GroundInteract()
	{
        //SwapGround(Grounded.Soil);

        //현재 플레이어가 들고있는 도구를 확인함
        Itemdata toolSlot = Inventory.Instance.GetEquippedSlotItem(InventorySlot.Inventorytype.Tool);

        if (!Inventory.Instance.SlotEquipped(InventorySlot.Inventorytype.Tool))
		{
            return;
		}

        //현재 장착된 아이템의 정보를 가져옴
        EquipItemData nowequipTool = toolSlot as EquipItemData;

        if(nowequipTool  != null)
		{
            //현재 툴 타입을 확인합니다.
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

        //작물을 손에 들고있지 않고 땅이 경작되어있거나 진흙이어야하고, 도구가 손에 들려있을경우
        if (cropTool != null && Groundstatus != Grounded.Grass && cropPlanted == null)
		{
            SpawnCrop();

            //해당 크롭툴을 심음
            cropPlanted.Plant(id, cropTool);
            //cropPlanted.Plant(cropseed);

            Inventory.Instance.ConsumeItem(Inventory.Instance.GetEquippedSlot(InventorySlot.Inventorytype.Tool));

        }
    }

    public CropBehavior SpawnCrop()
	{
        //크롭오브젝트를 인스턴스화함.
        GameObject cropObject = Instantiate(CropPrefab, transform);

        //생성된 인스턴스를 위치에 보이게끔 수정
        cropObject.transform.position = new Vector3(transform.position.x, 0.527f, transform.position.z);

        cropPlanted = cropObject.GetComponent<CropBehavior>();

        return cropPlanted;
    }

    public void ClockUpdate(TimeManage timemanage)
	{
        if(Groundstatus == Grounded.Mud)
		{
            int CheckTime = TimeManage.CompareTimeManage(timeGround, timemanage);

            //시간에따른 작물의 성장
            if(cropPlanted != null)
			{
                cropPlanted.Grow();
			}

            //24시간이 지난다면 진흙땅을 마르게해줌
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
