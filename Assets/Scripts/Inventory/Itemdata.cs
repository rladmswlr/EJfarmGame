using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���¸޴��� �߰��Ѵ�. Menu�̸� : Item ���� �Ͽ� �κ��丮 ������ �����Ѵ�.
[CreateAssetMenu(menuName = "Items/Item")]


// monobehaviour�� �ڵ� ��ӹ޴°��� ����� ScriptableObject�� ��ӹ޴´�
// �̹������ ���ݴ� ���� ������ data�� ���� ������ �Ҽ��ִ�.
public class Itemdata : ScriptableObject
{
	//�κ��丮�� �ʿ��� �׸�
	public Sprite Drawing;

	//�����ۿ� ���� ����
	public string Desc;

	//���� ������Ʈ
	public GameObject Object;

	public int cost;
}
