using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //이동관리
    private CharacterController playercontrol;
    private Animator ani;
    
    public float speed =4f;

    //플레이어 인터렉트 함수 가져옴
    Interact playerInteract;

    public float gravity = 20f;

    public GameObject InventoryPanal;

    public GameObject PlayerInventory;

    // Start is called before the first frame update
    void Start()
    {
        gravity = 20f;
        //초기화
        playercontrol = GetComponent<CharacterController>();

        ani = GetComponent<Animator>();
        playerInteract = GetComponentInChildren<Interact>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();

        PlayerInteract();


        //편의상 시간지나가는걸 보여주기위해 ]을 누를시 시간이 빠르게 지나감
        if (Input.GetKey(KeyCode.RightBracket))
		{
            TimeManager.Instance.SetTick();
		}
    }

    public void PlayerInteract()
	{
        if (Input.GetButtonDown("Interact"))
		{
            //인터렉트 상호작용키 적용 상호작용 클래스로 이동하여 상호작용 시스템 함수 적용

            playerInteract.InteractSystem();
		}

        if (Input.GetKeyDown(KeyCode.F))
		{
            playerInteract.ItemInteract();

        }

        if(Input.GetKeyDown(KeyCode.R))
		{
            playerInteract.ItemKeep();

        }

        if(Input.GetKeyDown(KeyCode.Escape))
		{
            Application.Quit();
		}
	}

	public void PlayerMove()
	{
        float Horizon = Input.GetAxisRaw("Horizontal");
        float Vertical = Input.GetAxisRaw("Vertical");


        Vector3 direct = new Vector3(Horizon, 0f, Vertical).normalized;
        Vector3 vec = speed * Time.deltaTime * direct;

        if (direct.magnitude >= 0.1f) //속도가 0.1f이상일때 움직여야한다.
        {
            //해당 방향으로 회전
            transform.rotation = Quaternion.LookRotation(direct);

            //이동
            //playercontrol.Move(vec);
		}

        vec.y -= gravity * Time.deltaTime;
        //이동
        playercontrol.Move(vec);

        // 현재 캐릭터의 움직임에 따라 Speed가 빨라짐
        ani.SetFloat("Speed", vec.magnitude);


        //움직일경우 
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") !=0)
		{
            //IsWalk 활성화
            ani.SetBool("IsWalk", true);
		}

        else
		{
            //움직임을 멈출경우 IsWalk 멈추기
            ani.SetBool("IsWalk", false);
		}


        //Run 키를 누를시 Run키는 shift임.
        if (Input.GetKey(KeyCode.LeftShift))
		{
           //속도를 늘림
           speed = 7f;
           ani.SetBool("IsRun", true);   //run활성화
        }

        //달리지 않을경우 속도를 줄임
        else
        {
            speed = 4f;
            ani.SetBool("IsRun", false);   //run 비활성화
        }

        if (Input.GetButtonDown("Inventory"))
		{
            if (InventoryPanal.activeSelf == false)
            {
                InventoryPanal.SetActive(true);
                UIManager.Instance.DrawInventory();

            }
            else
			{
                InventoryPanal.SetActive(false);
			}
		}


    }
}
