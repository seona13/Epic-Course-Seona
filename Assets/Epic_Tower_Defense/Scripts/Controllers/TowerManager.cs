using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowerManager : MonoBehaviour
{
	public static event Action<bool> onPlacementModeChange;
	public static event Action<Vector3> onTowerPlaced;

	private bool _towerPlacementMode = false;
	private bool _canPlaceTower = false;

	[SerializeField]
	private Transform _towerContainer;
	[SerializeField]
	private Tower[] _towers;
	private GameObject[] _prototypePool;
	private GameObject _activePrototype;
	private MeshRenderer _radius;

	private int _towerType;
	private Vector3 _plotPos;
	[SerializeField]
	private int _warFund = 500;

	[SerializeField]
	private Color _availableRadius;
	[SerializeField]
	private Color _unavailableRadius;



	void OnEnable()
	{
		BuildSpot.onOpenPlot += CheckValidPlacement;
		BuildSpot.onTryPlaceTower += TryPlaceTower;
	}


	void Start()
	{
		_prototypePool = new GameObject[_towers.Length];
		for (int i = 0; i < _towers.Length; i++)
		{
			_prototypePool[i] = Instantiate(_towers[i].prototypePrefab, transform);
			_prototypePool[i].SetActive(false);
		}
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			PlacementModeChange(true);
			DeactivatePrototype();
			TowerPlacementMode(0);
			_radius = _prototypePool[_towerType].GetComponentInChildren<Radius>().GetComponent<MeshRenderer>();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			PlacementModeChange(true);
			DeactivatePrototype();
			TowerPlacementMode(1);
			_radius = _prototypePool[_towerType].GetComponentInChildren<Radius>().GetComponent<MeshRenderer>();
		}

		if (Input.GetMouseButtonDown(1))
		{
			PlacementModeChange(false);
			DeactivatePrototype();
		}

		TowerCastingRay();
	}


	void OnDisable()
	{
		BuildSpot.onOpenPlot -= CheckValidPlacement;
		BuildSpot.onTryPlaceTower -= TryPlaceTower;
	}


	void DeactivatePrototype()
	{
		if (_activePrototype != null)
		{
			_activePrototype.SetActive(false);
		}
	}


	void PlacementModeChange(bool status)
	{
		_towerPlacementMode = status;
		if (onPlacementModeChange != null)
		{
			onPlacementModeChange(status);
		}
	}


	void TowerPlacementMode(int type)
	{
		_towerType = type;
		_activePrototype = _prototypePool[type];
		_activePrototype.SetActive(true);
	}


	void TowerCastingRay()
	{
		if (_towerPlacementMode)
		{
			Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(rayOrigin, out RaycastHit hitInfo))
			{
				if (_canPlaceTower)
				{
					_plotPos = hitInfo.transform.position;
					_activePrototype.transform.position = _plotPos;
				}
				else
				{
					_activePrototype.transform.position = hitInfo.point;
				}
			}
		}
	}


	void CheckValidPlacement(bool isOpen)
	{
		_canPlaceTower = isOpen;
		if (_canPlaceTower)
		{
			_radius.material.color = _availableRadius;
		}
		else
		{
			_radius.material.color = _unavailableRadius;
		}
	}


	void TryPlaceTower()
	{
		int towerCost = _towers[_towerType].buyFor;
		if (_warFund >= towerCost)
		{
			Instantiate(_towers[_towerType].prefab, _plotPos, Quaternion.identity, _towerContainer);
			_warFund -= towerCost;
			if (onTowerPlaced != null)
			{
				onTowerPlaced(_plotPos);
			}
		}
	}
}