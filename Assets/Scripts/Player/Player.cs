using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //�̵�����
    private CharacterController playercontrol;
    private Animator ani;
    
    public float speed =4f;

    //�÷��̾� ���ͷ�Ʈ �Լ� ������
    Interact playerInteract;

    public float gravity = 20f;

    public GameObject InventoryPanal;

    public GameObject PlayerInventory;

    // Start is called before the first frame update
    void Start()
    {
        gravity = 20f;
        //�ʱ�ȭ
        playercontrol = GetComponent<CharacterController>();

        ani = GetComponent<Animator>();
        playerInteract = GetComponentInChildren<Interact>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();

        PlayerInteract();


        //���ǻ� �ð��������°� �����ֱ����� ]�� ������ �ð��� ������ ������
        if (Input.GetKey(KeyCode.RightBracket))
		{
            TimeManager.Instance.SetTick();
		}
    }

    public void PlayerInteract()
	{
        if (Input.GetButtonDown("Interact"))
		{
            //���ͷ�Ʈ ��ȣ�ۿ�Ű ���� ��ȣ�ۿ� Ŭ������ �̵��Ͽ� ��ȣ�ۿ� �ý��� �Լ� ����

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

        if (direct.magnitude >= 0.1f) //�ӵ��� 0.1f�̻��϶� ���������Ѵ�.
        {
            //�ش� �������� ȸ��
            transform.rotation = Quaternion.LookRotation(direct);

            //�̵�
            //playercontrol.Move(vec);
		}

        vec.y -= gravity * Time.deltaTime;
        //�̵�
        playercontrol.Move(vec);

        // ���� ĳ������ �����ӿ� ���� Speed�� ������
        ani.SetFloat("Speed", vec.magnitude);


        //�����ϰ�� 
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") !=0)
		{
            //IsWalk Ȱ��ȭ
            ani.SetBool("IsWalk", true);
		}

        else
		{
            //�������� ������ IsWalk ���߱�
            ani.SetBool("IsWalk", false);
		}


        //Run Ű�� ������ RunŰ�� shift��.
        if (Input.GetKey(KeyCode.LeftShift))
		{
           //�ӵ��� �ø�
           speed = 7f;
           ani.SetBool("IsRun", true);   //runȰ��ȭ
        }

        //�޸��� ������� �ӵ��� ����
        else
        {
            speed = 4f;
            ani.SetBool("IsRun", false);   //run ��Ȱ��ȭ
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
