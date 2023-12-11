using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
	public static LocationManager Instance { get; private set; }

	public List<StartPoint> startPoints;

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

	public Transform GetPlayerStartingPosition(SceneTrans.Location enteringFrom)
	{
		//Tries to find the matching startpoint based on the Location given
		StartPoint startingPoint = startPoints.Find(x => x.eneteringFrom == enteringFrom);

		//Return the transform
		return startingPoint.playerStart;
	}

}
