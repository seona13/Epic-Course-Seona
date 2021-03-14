﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
	public static event Action<int> onBuildButtonClicked;
	public static event Action<int> onUpgradeButtonClicked;
	public static event Action<int> onSellTowerButtonClicked;

	[SerializeField]
	private Text _warFundDisplay;

	[Space(10)]

	[SerializeField]
	private Database _assetDatabase;
	[SerializeField]
	private Button _buildGatlingGunButton;
	[SerializeField]
	private Button _buildMissileLauncherButton;

	[Space(10)]

	[SerializeField]
	private Button _upgradeButtonGun;
	[SerializeField]
	private Text _upgradeGunCost;

	[Space(10)]

	[SerializeField]
	private Button _upgradeButtonMissile;
	[SerializeField]
	private Text _upgradeMissileCost;

	[Space(10)]

	[SerializeField]
	private Button _sellTowerButton;
	[SerializeField]
	private Text _sellTowerRefund;



	private void OnEnable()
	{
		BuildSpot.onSelectTower += ShowTowerOptions;
		EnemyAI.onEnemyDie += OnEnemyDied;
	}


	void Start()
	{
		_warFundDisplay.text = PlayerManager.Instance.warFund.ToString();

		_buildGatlingGunButton.onClick.AddListener(() => BuildButtonClicked(0));
		_buildGatlingGunButton.GetComponentInChildren<Text>().text = "$" + _assetDatabase.towers[0].buyFor;
		_buildMissileLauncherButton.onClick.AddListener(() => BuildButtonClicked(1));
		_buildMissileLauncherButton.GetComponentInChildren<Text>().text = "$" + _assetDatabase.towers[1].buyFor;

		_upgradeButtonGun.interactable = false;
		_upgradeButtonMissile.interactable = false;
		_sellTowerButton.interactable = false;

		_upgradeButtonGun.onClick.AddListener(() => UpgradeButtonClicked(_assetDatabase.towers[0].upgradesTo.towerType));
		_upgradeButtonMissile.onClick.AddListener(() => UpgradeButtonClicked(_assetDatabase.towers[1].upgradesTo.towerType));
	}


	void Update()
	{

	}


	private void OnDisable()
	{
		BuildSpot.onSelectTower -= ShowTowerOptions;
		TowerManager.onTowerPlaced += OnTowerPlaced;
		EnemyAI.onEnemyDie -= OnEnemyDied;
	}


	void BuildButtonClicked(int towerID)
	{
		onBuildButtonClicked?.Invoke(towerID);
	}


	void UpgradeButtonClicked(int towerID)
	{
		onUpgradeButtonClicked?.Invoke(towerID);
	}


	void SellTowerButtonClicked(int towerID)
	{
		onSellTowerButtonClicked?.Invoke(towerID);
	}


	void OnTowerPlaced(Vector3 pos, Tower tower, GameObject towerGO)
	{
		int warFunds = int.Parse(_warFundDisplay.text);
		warFunds -= tower.buyFor;
		_warFundDisplay.text = warFunds.ToString();
	}


	void OnEnemyDied(GameObject go, int amount)
	{
		int warFunds = int.Parse(_warFundDisplay.text);
		warFunds += amount;
		_warFundDisplay.text = warFunds.ToString();
	}


	void ShowTowerOptions(bool status, GameObject towerGO, Tower tower)
	{
		if (status)
		{
			if (tower.towerType == 0 && tower.upgradesTo != null)
			{
				_upgradeButtonMissile.interactable = false;
				_upgradeButtonGun.interactable = true;
				_upgradeGunCost.text = "$" + tower.upgradesTo.buyFor.ToString();
			}
			else if (tower.towerType == 1 && tower.upgradesTo != null)
			{
				_upgradeButtonGun.interactable = false;
				_upgradeButtonMissile.interactable = true;
				_upgradeMissileCost.text = "$" + tower.upgradesTo.buyFor.ToString();
			}

			_sellTowerButton.interactable = true;
			_sellTowerRefund.text = "$" + tower.sellFor.ToString();
			_sellTowerButton.onClick.RemoveAllListeners();
			_sellTowerButton.onClick.AddListener(() => SellTowerButtonClicked(tower.towerType));
		}
		else
		{
			_upgradeButtonGun.interactable = false;
			_upgradeButtonMissile.interactable = false;
			_sellTowerButton.interactable = false;
		}
	}
}