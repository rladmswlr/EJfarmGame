using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : InteractableObject
{
	public override void Pickup()
	{
		UIManager.Instance.TriggerYesNoPrompt("����� ���ڱ⸦ ���Ͻʴϱ�?", GameState.Instance.Sleep);
	}
}
