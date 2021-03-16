using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoSingleton<GameManager>
{
	public static event Action<int> onLivesChanged;
	public static event Action<int> onWarFundsChanged;
	public static event Action onLevelComplete;
	public static event Action onGameWon;
	public static event Action onGameLost;

	private float _fixedDeltaTime;
	public int warFund = 500;
	[SerializeField]
	private int _lives;



	private void OnEnable()
	{
		TowerManager.onTowerPlaced += OnTowerBought;
		TowerManager.onTowerUpgraded += OnTowerUpgraded;
		TowerManager.onTowerSold += OnTowerSold;
		EnemyAI.onEnemyDie += OnEnemyDied;
		UIManager.onPauseButtonClicked += OnPauseButtonClicked;
		UIManager.onPlayButtonClicked += OnPlayButtonClicked;
		UIManager.onFFButtonClicked += OnFFButtonClicked;
		UIManager.onRestartButtonClicked += OnRestartButtonClicked;
		SpawnManager.onWaveIncrement += OnWaveIncrement;
		Outlet.onEnemyEscaped += OnEnemyEscaped;
	}


	void Start()
	{
		_fixedDeltaTime = Time.fixedDeltaTime;
		ChangeTimeScale(1);
		LivesChanged();
	}


	void Update()
	{

	}


	private void OnDisable()
	{
		TowerManager.onTowerPlaced -= OnTowerBought;
		TowerManager.onTowerUpgraded -= OnTowerUpgraded;
		TowerManager.onTowerSold -= OnTowerSold;
		EnemyAI.onEnemyDie -= OnEnemyDied;
		UIManager.onPauseButtonClicked -= OnPauseButtonClicked;
		UIManager.onPlayButtonClicked -= OnPlayButtonClicked;
		UIManager.onFFButtonClicked -= OnFFButtonClicked;
		UIManager.onRestartButtonClicked -= OnRestartButtonClicked;
		SpawnManager.onWaveIncrement -= OnWaveIncrement;
		Outlet.onEnemyEscaped -= OnEnemyEscaped;
	}


	#region War Funds
	void WarFundChanged()
	{
		onWarFundsChanged?.Invoke(warFund);
	}


	public void OnTowerBought(Vector3 pos, Tower tower, GameObject towerGO)
	{
		warFund -= tower.buyFor;
		WarFundChanged();
	}


	void OnTowerUpgraded(int amount)
	{
		warFund -= amount;
		WarFundChanged();
	}


	void OnTowerSold(int amount)
	{
		warFund += amount;
		WarFundChanged();
	}


	public void OnEnemyDied(GameObject enemy, int value)
	{
		warFund += value;
		WarFundChanged();
	}
	#endregion


	#region Game Speed
	void ChangeTimeScale(float scale)
	{
		if (Time.timeScale != scale)
		{
			Time.timeScale = scale;
			Time.fixedDeltaTime = _fixedDeltaTime * Time.timeScale;
		}
	}


	void OnPauseButtonClicked()
	{
		ChangeTimeScale(0);
	}


	void OnPlayButtonClicked()
	{
		ChangeTimeScale(1);
	}


	void OnFFButtonClicked()
	{
		ChangeTimeScale(2);
	}
	#endregion


	void OnRestartButtonClicked()
	{
		SceneManager.LoadScene(0);
	}


	void LivesChanged()
	{
		onLivesChanged?.Invoke(_lives);
	}


	void OnWaveIncrement(int wave)
	{
		if (wave > 10)
		{
			onGameWon?.Invoke();
		}
		else
		{
			onLevelComplete?.Invoke();
			// Countdown to next level
		}
	}


	void OnEnemyEscaped()
	{
		_lives--;

		if (_lives > 0)
		{
			LivesChanged();
		}
		else
		{
			onGameLost?.Invoke();
		}
	}
}