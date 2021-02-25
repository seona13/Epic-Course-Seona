using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSpot : MonoBehaviour
{
	bool _available = true;

	bool canPlaceTower = false;



	private void OnMouseEnter()
	{
		if (TowerPlacement.Instance.towerPlacementMode && _available)
		{
			TowerPlacement.Instance.PossibleTowerPlacement();
		}
	}


	private void OnMouseDown()
	{
		if (canPlaceTower)
		{
			TowerPlacement.Instance.PlaceTower();
			_available = false;
		}
	}


	private void OnMouseExit()
	{

	}


	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
