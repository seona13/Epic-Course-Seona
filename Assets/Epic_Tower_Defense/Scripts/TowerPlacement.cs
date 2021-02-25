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
	public Action onPlacementModeActivate;
	public Action onPlacementModeDeactivate;



	void Start()
	{

	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			towerPlacementMode = true;
			Destroy(_prototype);
			_prototype = Instantiate(_prototypes[0]);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			towerPlacementMode = true;
			Destroy(_prototype);
			_prototype = Instantiate(_prototypes[1]);
		}

		// Right-click to exit placement mode
		if (towerPlacementMode && Input.GetMouseButtonDown(1))
		{
			towerPlacementMode = false;
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