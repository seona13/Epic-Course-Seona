using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowerPlacement : MonoSingleton<TowerPlacement>
{
	[SerializeField]
	private Transform _towerContainer;
	[SerializeField]
	private GameObject[] _prototypes;
	private GameObject _prototype;
	private bool _followMouse = true;

	public bool towerPlacementMode = false;
	public static event Action<bool> onPlacementModeActive;



	void Start()
	{

	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			PlacmentModeActive(true);
			Destroy(_prototype);
			_prototype = Instantiate(_prototypes[0]);
			// turn on radius. How??
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			PlacmentModeActive(true);
			Destroy(_prototype);
			_prototype = Instantiate(_prototypes[1]);
			// turn on radius. How??
		}

		// Right-click to exit placement mode
		if (towerPlacementMode && Input.GetMouseButtonDown(1))
		{
			PlacmentModeActive(false);
			_prototype = null;
		}

		if (towerPlacementMode && _followMouse)
		{
			Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;

			if (Physics.Raycast(rayOrigin, out hitInfo))
			{
				_prototype.transform.position = hitInfo.point;
			}
		}
	}


	public void PlacmentModeActive(bool status)
	{
		towerPlacementMode = status;
		if (onPlacementModeActive != null)
		{
			onPlacementModeActive(status);
		}
	}


	public void PossibleTowerPlacement(Vector3 pos) // OnMouseEnter
	{
		_followMouse = false;
		// snap to build spot
		_prototype.transform.position = pos;
		// turn radius green
	}


	public void PlaceTower() // OnMouseDown
	{
		// get which tower we're placing
		// 
	}


	public void LeaveBuildSpot()
	{
		_followMouse = true;
	}
}