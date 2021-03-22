using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
	public static event Action<int> onBuildButtonClicked;
	public static event Action<BuildSpot> onUpgradeButtonClicked;
	public static event Action<BuildSpot> onSellTowerButtonClicked;
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
	private GameObject _levelPanel;
	[SerializeField]
	private Text _levelText;
	[SerializeField]
	private Text _countdownText;

	[Space(10)]

	[SerializeField]
	private Color _normalUIColour;
	[SerializeField]
	private Color _dangerUIColour;
	[SerializeField]
	private Color _criticalUIColour;
	[SerializeField]
	private Text _statusText;
	[SerializeField]
	private Image[] _panels;


	private int _selectedTower = 999;



	private void OnEnable()
	{
		BuildSpot.onSelectTower += ShowTowerOptions;
		TowerManager.onExitModifyMode += DisableTowerOptions;
		SpawnManager.onWaveIncrement += OnWaveIncrement;
		GameManager.onLivesChanged += OnLivesChanged;
		GameManager.onStatusChanged += OnStatusChanged;
		GameManager.onWarFundsChanged += OnWarFundChanged;
		GameManager.onLevelComplete += OnLevelComplete;
		GameManager.onGameWon += OnGameWon;
		GameManager.onGameLost += OnGameLost;
		GameManager.onCountdownDecrease += OnCountdownDecrease;
	}


	void Start()
	{
		_warFundDisplay.text = GameManager.Instance.warFund.ToString();

		_buildGatlingGunButton.onClick.AddListener(() => BuildButtonClicked(0));
		_buildGatlingGunButton.GetComponentInChildren<Text>().text = "$" + _assetDatabase.towers[0].buyFor;
		_buildMissileLauncherButton.onClick.AddListener(() => BuildButtonClicked(1));
		_buildMissileLauncherButton.GetComponentInChildren<Text>().text = "$" + _assetDatabase.towers[1].buyFor;

		_upgradeGunCost.text = "$" + _assetDatabase.towers[0].upgradesTo.buyFor.ToString();
		_upgradeMissileCost.text = "$" + _assetDatabase.towers[1].upgradesTo.buyFor.ToString();

		DisableTowerOptions();
	}


	void Update()
	{

	}


	private void OnDisable()
	{
		BuildSpot.onSelectTower -= ShowTowerOptions;
		TowerManager.onExitModifyMode -= DisableTowerOptions;
		SpawnManager.onWaveIncrement -= OnWaveIncrement;
		GameManager.onLivesChanged -= OnLivesChanged;
		GameManager.onStatusChanged -= OnStatusChanged;
		GameManager.onWarFundsChanged -= OnWarFundChanged;
		GameManager.onLevelComplete -= OnLevelComplete;
		GameManager.onGameWon -= OnGameWon;
		GameManager.onGameLost -= OnGameLost;
		GameManager.onCountdownDecrease -= OnCountdownDecrease;
	}


	#region Tower Management
	void BuildButtonClicked(int towerID)
	{
		onBuildButtonClicked?.Invoke(towerID);
	}


	void UpgradeButtonClicked(BuildSpot buildSpot)
	{
		onUpgradeButtonClicked?.Invoke(buildSpot);
	}


	void SellTowerButtonClicked(BuildSpot buildSpot)
	{
		onSellTowerButtonClicked?.Invoke(buildSpot);
	}


	void ShowTowerOptions(BuildSpot buildSpot)
	{
		Tower tower = buildSpot.GetTower();
		if (tower.towerType == 0 && tower.upgradesTo != null)
		{
			_selectedTower = 0;
			if (tower.buyFor < GameManager.Instance.warFund)
			{
				_upgradeButtonGun.interactable = true;
			}
			_upgradeButtonMissile.interactable = false;
			_upgradeButtonGun.onClick.RemoveAllListeners();
			_upgradeButtonGun.onClick.AddListener(() => UpgradeButtonClicked(buildSpot));
		}
		else if (tower.towerType == 1 && tower.upgradesTo != null)
		{
			_selectedTower = 1;
			if (tower.buyFor < GameManager.Instance.warFund)
			{
				_upgradeButtonMissile.interactable = true;
			}
			_upgradeButtonGun.interactable = false;
			_upgradeButtonMissile.onClick.RemoveAllListeners();
			_upgradeButtonMissile.onClick.AddListener(() => UpgradeButtonClicked(buildSpot));
		}

		_sellTowerButton.interactable = true;
		_sellTowerRefund.text = "$" + tower.sellFor.ToString();
		_sellTowerButton.onClick.RemoveAllListeners();
		_sellTowerButton.onClick.AddListener(() => SellTowerButtonClicked(buildSpot));
	}


	void DisableTowerOptions()
	{
		_upgradeButtonGun.interactable = false;
		_upgradeButtonMissile.interactable = false;
		_sellTowerButton.interactable = false;
		_sellTowerRefund.text = "";
		_selectedTower = 999;
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


	#region Level Management
	void OnWaveIncrement(int wave)
	{
		_waveCounter.text = wave + " / 10";
	}


	void OnLevelComplete()
	{
		_statusText.text = "LEVEL\nCOMPLETE";
		_levelPanel.SetActive(true);
	}


	void OnGameWon()
	{
		_statusText.text = "YOU\nTRIUMPH";
		_levelPanel.SetActive(true);
	}


	void OnGameLost()
	{
		_statusText.text = "YOU\nLOST";
		_levelPanel.SetActive(true);
	}


	void OnCountdownDecrease(int count)
	{
		if (count == 0)
		{
			_levelPanel.SetActive(false);
			return;
		}

		_countdownText.text = count.ToString();
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

		if (amount < _assetDatabase.towers[0].buyFor)
		{
			_buildGatlingGunButton.interactable = false;
		}
		else
		{
			_buildGatlingGunButton.interactable = true;
		}

		if (amount < _assetDatabase.towers[1].buyFor)
		{
			_buildMissileLauncherButton.interactable = false;
		}
		else
		{
			_buildMissileLauncherButton.interactable = true;
		}

		if (_selectedTower == 2 && amount < _assetDatabase.towers[2].buyFor)
		{
			_upgradeButtonGun.interactable = false;
		}
		else
		{
			_upgradeButtonGun.interactable = true;
		}

		if (_selectedTower == 3 && amount < _assetDatabase.towers[3].buyFor)
		{
			_upgradeButtonMissile.interactable = false;
		}
		else
		{
			_upgradeButtonMissile.interactable = true;
		}
	}


	void OnStatusChanged(Status status)
	{
		Color currentColour = _normalUIColour;
		_statusText.text = "Good";

		if (status == Status.DANGER)
		{
			currentColour = _dangerUIColour;
			_statusText.text = "Danger";
		}
		else if (status == Status.CRITICAL)
		{
			currentColour = _criticalUIColour;
			_statusText.text = "Critical";
		}

		foreach (Image panel in _panels)
		{
			panel.color = currentColour;
		}
	}
}