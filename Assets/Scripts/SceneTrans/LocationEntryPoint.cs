using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationEntryPoint : MonoBehaviour
{
    [SerializeField]
    SceneTrans.Location locationToSwitch;

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			SceneTrans.Instance.SwitchLocation(locationToSwitch);
		}
	}

}
