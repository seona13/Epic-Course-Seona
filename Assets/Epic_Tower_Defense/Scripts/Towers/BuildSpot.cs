using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildSpot : MonoBehaviour
{
	public static event Action<bool> onOpenPlot;
	public static event Action onTryPlaceTower;

	private bool _placementModeActive = false;
	private bool _available = true;
	private ParticleSystem _particles;



	private void OnEnable()
	{
		TowerManager.onPlacementModeChange += PlacementMode;
		TowerManager.onTowerPlaced += DeactivatePlot;
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
		TowerManager.onTowerPlaced -= DeactivatePlot;
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
			if (onTryPlaceTower != null)
			{
				onTryPlaceTower();
			}
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
		if (onOpenPlot != null)
		{
			onOpenPlot(isOpen);
		}
	}


	void DeactivatePlot(Vector3 pos)
	{
		if (transform.position == pos)
		{
			_available = false;
			_particles.Stop();
		}
	}
}