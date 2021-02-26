using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildSpot : MonoBehaviour
{
	private ParticleSystem _particles;
	private bool _available = true;
	private bool _canPlaceTower = false;
	private bool _placementModeActive = false;



	private void OnEnable()
	{
		TowerPlacement.onPlacementModeActive += PlacementMode;
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
		TowerPlacement.onPlacementModeActive -= PlacementMode;
	}


	void OnMouseEnter()
	{
		if (_placementModeActive && _available)
		{
			TowerPlacement.Instance.PossibleTowerPlacement(transform.position);
		}
	}


	void OnMouseDown()
	{
		if (_canPlaceTower)
		{
			TowerPlacement.Instance.PlaceTower();
			_available = false;
		}
	}


	void OnMouseExit()
	{
		if (_placementModeActive && _available)
		{
			TowerPlacement.Instance.LeaveBuildSpot();
		}
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
}