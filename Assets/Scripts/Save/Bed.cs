using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : InteractableObject
{
	public override void Pickup()
	{
		UIManager.Instance.TriggerYesNoPrompt("당신은 잠자기를 원하십니까?", GameState.Instance.Sleep);
	}
}
