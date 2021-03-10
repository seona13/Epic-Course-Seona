using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoSingleton<PlayerManager>
{
	public static event Action<int> onWarFundsChanged;

	public int warFund = 500;



	void Start()
	{

	}


	void Update()
	{

	}


	public void OnWarFundsChanged(int amount)
	{
		warFund += amount;
		onWarFundsChanged?.Invoke(warFund);
	}
}