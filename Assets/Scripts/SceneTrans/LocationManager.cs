using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
	public static LocationManager Instance { get; private set; }

	public List<StartPoint> startPoints;

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

	public Transform GetPlayerStartingPosition(SceneTrans.Location enteringFrom)
	{
		//Tries to find the matching startpoint based on the Location given
		StartPoint startingPoint = startPoints.Find(x => x.eneteringFrom == enteringFrom);

		//Return the transform
		return startingPoint.playerStart;
	}

}
