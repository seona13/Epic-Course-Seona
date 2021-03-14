﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildSpot : MonoBehaviour
{
	public static event Action<bool> onOpenPlot;
	public static event Action onTryPlaceTower;
	public static event Action<bool, GameObject, Tower> onSelectTower;

	private bool _placementModeActive = false;
	private bool _available = true;
	private ParticleSystem _particles;
	private Tower _tower;
	private GameObject _towerGO;



	private void OnEnable()
	{
		TowerManager.onPlacementModeChange += PlacementMode;
		TowerManager.onTowerPlaced += PlaceTower;
		UIManager.onUpgradeButtonClicked += OnUpgradeButtonClicked;
		UIManager.onSellTowerButtonClicked += OnSellTowerButtonClicked;
	}


	void Start()
	{
		_particles = GetComponentInChildren<ParticleSystem>();
	}


	void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			onSelectTower?.Invoke(false, _towerGO, _tower);
		}
	}


	private void OnDisable()
	{
		TowerManager.onPlacementModeChange -= PlacementMode;
		TowerManager.onTowerPlaced -= PlaceTower;
		UIManager.onUpgradeButtonClicked -= OnUpgradeButtonClicked;
		UIManager.onSellTowerButtonClicked -= OnSellTowerButtonClicked;
	}


	void OnMouseEnter()
	{
		if (_placementModeActive && _available)
		{
			OpenPlot(true);
		}
	}


	void OnMouseDown()
	{
		if (_placementModeActive && _available)
		{
			onTryPlaceTower?.Invoke();
		}

		if (_placementModeActive == false && _available == false)
		{
			onSelectTower?.Invoke(true, _towerGO, _tower);
		}
	}


	void OnMouseExit()
	{
		OpenPlot(false);
	}


	void PlacementMode(bool status)
	{
		_placementModeActive = status;
		if (status && _available)
		{
			_particles.Play();
		}
		else
		{
			_particles.Stop();
		}
	}


	void OpenPlot(bool isOpen)
	{
		onOpenPlot?.Invoke(isOpen);
	}


	void PlaceTower(Vector3 pos, Tower tower, GameObject towerGO)
	{
		if (transform.position == pos)
		{
			_available = false;
			_particles.Stop();
			_tower = tower;
			_towerGO = towerGO;
		}
	}


	void OnUpgradeButtonClicked(int towerType)
	{
		Debug.Log("Upgrading tower " + towerType);

		_towerGO.SetActive(false);

		PoolManager.Instance.RequestTower(towerType);
	}


	void OnSellTowerButtonClicked(int towerType)
	{
		Debug.Log("Selling tower " + towerType);
	}
}