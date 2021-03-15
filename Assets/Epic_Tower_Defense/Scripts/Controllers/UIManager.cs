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
	public static event Action onPauseButtonClicked;
	public static event Action onPlayButtonClicked;
	public static event Action onFFButtonClicked;
	public static event Action onRestartButtonClicked;

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

	[Space(10)]

	[SerializeField]
	private GameObject _pauseSelected;
	[SerializeField]
	private GameObject _playSelected;
	[SerializeField]
	private GameObject _ffSelected;
	[SerializeField]
	private GameObject _restartSelected;

	[Space(10)]

	[SerializeField]
	private Text _lifeCounter;
	[SerializeField]
	private Text _waveCounter;
	[SerializeField]
	private GameObject _statusPanel;
	[SerializeField]
	private Text _statusText;



	private void OnEnable()
	{
		BuildSpot.onSelectTower += ShowTowerOptions;
		SpawnManager.onWaveIncrement += OnWaveIncrement;
		GameManager.onLivesChanged += OnLivesChanged;
		GameManager.onWarFundsChanged += OnWarFundChanged;
		GameManager.onLevelComplete += OnLevelComplete;
		GameManager.onGameWon += OnGameWon;
		GameManager.onGameLost += OnGameLost;
	}


	void Start()
	{
		_warFundDisplay.text = GameManager.Instance.warFund.ToString();

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
		SpawnManager.onWaveIncrement -= OnWaveIncrement;
		GameManager.onLivesChanged -= OnLivesChanged;
		GameManager.onWarFundsChanged -= OnWarFundChanged;
		GameManager.onLevelComplete -= OnLevelComplete;
		GameManager.onGameWon -= OnGameWon;
		GameManager.onGameLost -= OnGameLost;
	}


	#region Tower Management
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
	#endregion


	#region Speed Controls
	public void PauseButtonClicked()
	{
		onPauseButtonClicked?.Invoke();

		_pauseSelected.SetActive(true);
		_playSelected.SetActive(false);
		_ffSelected.SetActive(false);
	}


	public void PlayButtonClicked()
	{
		onPlayButtonClicked?.Invoke();

		_pauseSelected.SetActive(false);
		_playSelected.SetActive(true);
		_ffSelected.SetActive(false);
	}


	public void FFButtonClicked()
	{
		onFFButtonClicked?.Invoke();

		_pauseSelected.SetActive(false);
		_playSelected.SetActive(false);
		_ffSelected.SetActive(true);
	}
	#endregion


	public void RestartButtonClicked()
	{
		_restartSelected.SetActive(true);
		onRestartButtonClicked?.Invoke();
	}


	void OnLivesChanged(int lives)
	{
		_lifeCounter.text = lives.ToString();
	}


	void OnWarFundChanged(int amount)
	{
		_warFundDisplay.text = amount.ToString();
	}


	#region Level Management
	void OnWaveIncrement(int wave)
	{
		_waveCounter.text = wave + " / 10";
	}


	void OnLevelComplete()
	{
		_statusText.text = "LEVEL\nCOMPLETE";
		_statusPanel.SetActive(true);
	}


	void OnGameWon()
	{
		_statusText.text = "YOU\nTRIUMPH";
		_statusPanel.SetActive(true);
	}


	void OnGameLost()
	{
		_statusText.text = "YOU\nLOST";
		_statusPanel.SetActive(true);
	}
	#endregion
}