using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildSpot : MonoBehaviour
{
	public static event Action<bool> onOpenPlot;
	public static event Action onTryPlaceTower;
	public static event Action<BuildSpot> onSelectTower;

	private bool _placementModeActive = false;
	private bool _available = true;
	private ParticleSystem _particles;
	private Tower _tower;
	private GameObject _towerGO;



	private void OnEnable()
	{
		TowerManager.onPlacementModeChange += PlacementMode;
		TowerManager.onTowerPlaced += PlaceTower;
	}


	void Start()
	{
		_particles = GetComponentInChildren<ParticleSystem>();
	}


	void Update()
	{
	}


	private void OnDisable()
	{
		TowerManager.onPlacementModeChange -= PlacementMode;
		TowerManager.onTowerPlaced -= PlaceTower;
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
			onSelectTower?.Invoke(this);
		}
	}


	void OnMouseExit()
	{
		OpenPlot(false);
	}


	public Tower GetTower()
	{
		return _tower;
	}


	public GameObject GetGameObject()
	{
		return _towerGO;
	}


	public void SetNewTowerInfo(GameObject newGO, Tower newTower)
	{
		_towerGO = newGO;
		_tower = newTower;
	}


	public void EmptyTowerInfo()
	{
		_tower = null;
		_towerGO = null;
		_available = true;
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
}