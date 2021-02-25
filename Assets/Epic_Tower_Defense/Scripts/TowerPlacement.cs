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

	public bool towerPlacementMode = false;



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

		if (towerPlacementMode)
		{
			Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;

			if (Physics.Raycast(rayOrigin, out hitInfo))
			{
				_prototype.transform.position = hitInfo.point;
			}
		}
	}


	public void PossibleTowerPlacement() // OnMouseEnter
	{
		// turn radius green
		// snap to build spot
	}


	public void PlaceTower() // OnMouseDown
	{
		// get which tower we're placing
		// 
	}
}
