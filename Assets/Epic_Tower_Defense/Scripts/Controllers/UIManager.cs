using System;
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
	private GameObject _upgradePanelGun;
	[SerializeField]
	private Text _upgradeGunCost;
	[SerializeField]
	private Button _upgradeGunConfirm;
	[SerializeField]
	private Button _upgradeGunCancel;

	[Space(10)]

	[SerializeField]
	private GameObject _upgradePanelMissile;
	[SerializeField]
	private Text _upgradeMissileCost;
	[SerializeField]
	private Button _upgradeMissileConfirm;
	[SerializeField]
	private Button _upgradeMissileCancel;

	[Space(10)]

	[SerializeField]
	private GameObject _sellTowerPanel;
	[SerializeField]
	private Text _sellTowerRefund;
	[SerializeField]
	private Button _sellTowerConfirm;
	[SerializeField]
	private Button _sellTowerCancel;



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

		_upgradeGunCancel.onClick.AddListener(() => CancelButtonClicked());
		_upgradeMissileCancel.onClick.AddListener(() => CancelButtonClicked());
		_sellTowerCancel.onClick.AddListener(() => CancelButtonClicked());

		_upgradeGunConfirm.onClick.AddListener(() => ConfirmUpgradeButtonClicked(_assetDatabase.towers[0].upgradesTo.towerType));
		_upgradeMissileConfirm.onClick.AddListener(() => ConfirmUpgradeButtonClicked(_assetDatabase.towers[1].upgradesTo.towerType));
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


	void CancelButtonClicked()
	{
		_upgradePanelGun.SetActive(false);
		_upgradePanelMissile.SetActive(false);
		_sellTowerPanel.SetActive(false);
	}


	void ConfirmUpgradeButtonClicked(int towerID)
	{
		onUpgradeButtonClicked?.Invoke(towerID);
	}


	void ConfirmSellTowerButtonClicked(int towerID)
	{
		onSellTowerButtonClicked?.Invoke(towerID);
	}


	void OnTowerPlaced(Vector3 pos, Tower tower)
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


	void ShowTowerOptions(Tower tower)
	{
		if (tower.towerType == 0 && tower.upgradesTo != null)
		{
			_upgradePanelMissile.SetActive(false);
			_upgradePanelGun.SetActive(true);
			_upgradeGunCost.text = tower.upgradesTo.buyFor.ToString();
		}
		else if (tower.towerType == 1 && tower.upgradesTo != null)
		{
			_upgradePanelGun.SetActive(false);
			_upgradePanelMissile.SetActive(true);
			_upgradeMissileCost.text = tower.upgradesTo.buyFor.ToString();
		}

		_sellTowerPanel.SetActive(true);
		_sellTowerRefund.text = tower.sellFor.ToString();
		_sellTowerConfirm.onClick.RemoveAllListeners();
		_sellTowerConfirm.onClick.AddListener(() => ConfirmSellTowerButtonClicked(tower.towerType));
	}
}