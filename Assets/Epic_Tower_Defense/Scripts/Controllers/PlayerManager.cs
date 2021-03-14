using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoSingleton<PlayerManager>
{
	public int warFund = 500;



	private void OnEnable()
	{
		TowerManager.onTowerPlaced += TowerBought;
		EnemyAI.onEnemyDie += EnemyDied;
	}


	void Start()
	{

	}


	void Update()
	{

	}


	private void OnDisable()
	{
		TowerManager.onTowerPlaced -= TowerBought;
	}


	public void TowerBought(Vector3 pos, Tower tower, GameObject towerGO)
	{
		warFund -= tower.buyFor;
	}


	public void EnemyDied(GameObject enemy, int value)
	{
		warFund += value;
	}
}