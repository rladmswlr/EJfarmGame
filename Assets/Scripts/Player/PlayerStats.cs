using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
	public static int Money { get; private set; }

	public const string CURRENCY = "¿ø";

	public static void Spend(int cost)
	{
		if(cost > Money)
		{
			return;
		}

		Money -= cost;
		UIManager.Instance.RenderPlayerStats();
	}

	public static void Earn(int income)
	{
		Money += income;
		UIManager.Instance.RenderPlayerStats();
	}

	public static void LoadStats(int money)
	{
		Money = money;
		UIManager.Instance.RenderPlayerStats();
	}
}
