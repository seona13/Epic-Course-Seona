using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowerManager : MonoBehaviour
{
	public static event Action<bool> onPlacementModeChange;
	public static event Action<Vector3, Tower, GameObject> onTowerPlaced;

	private bool _towerPlacementMode = false;
	private bool _towerModifyMode = false;
	private bool _canPlaceTower = false;

	[SerializeField]
	private Database _assetDatabase;

	private GameObject[] _prototypePool;
	private GameObject _activePrototype;
	private GameObject _activeTower;
	private MeshRenderer _radius;


	private int _towerType;
	private Vector3 _plotPos;

	[SerializeField]
	private Color _availableRadius;
	[SerializeField]
	private Color _unavailableRadius;



	void OnEnable()
	{
		BuildSpot.onOpenPlot += CheckValidPlacement;
		BuildSpot.onTryPlaceTower += TryPlaceTower;
		BuildSpot.onSelectTower += TowerModifyMode;
		UIManager.onBuildButtonClicked += OnBuildButtonClicked;
	}


	void Start()
	{
		_prototypePool = new GameObject[_assetDatabase.towers.Length];
		for (int i = 0; i < _assetDatabase.towers.Length; i++)
		{
			if (_assetDatabase.towers[i].prototypePrefab != null)
			{
				_prototypePool[i] = Instantiate(_assetDatabase.towers[i].prototypePrefab, transform);
				_prototypePool[i].SetActive(false);
			}
		}
	}


	void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			OnPlacementModeChange(false);
			DeactivatePrototype();
		}

		TowerCastingRay();
	}


	void OnDisable()
	{
		BuildSpot.onOpenPlot -= CheckValidPlacement;
		BuildSpot.onTryPlaceTower -= TryPlaceTower;
		BuildSpot.onSelectTower -= TowerModifyMode;
		UIManager.onBuildButtonClicked -= OnBuildButtonClicked;
	}


	void OnBuildButtonClicked(int towerType)
	{
		OnPlacementModeChange(true);
		DeactivatePrototype();
		TowerPlacementMode(towerType);
		_radius = _prototypePool[_towerType].GetComponentInChildren<Radius>().GetComponent<MeshRenderer>();
	}


	void DeactivatePrototype()
	{
		if (_activePrototype != null)
		{
			_activePrototype.SetActive(false);
		}
	}


	void OnPlacementModeChange(bool status)
	{
		_towerPlacementMode = status;
		onPlacementModeChange?.Invoke(status);
	}


	void TowerPlacementMode(int type)
	{
		_towerType = type;
		_activePrototype = _prototypePool[type];
		_activePrototype.SetActive(true);
	}


	void TowerModifyMode(bool status, GameObject towerGO, Tower tower)
	{
		_towerModifyMode = status;

		if (status)
		{
			_activeTower = towerGO;
		}
		else
		{
			_activeTower = null;
		}
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
		int towerCost = _assetDatabase.towers[_towerType].buyFor;
		if (GameManager.Instance.warFund >= towerCost)
		{
			GameObject tower = PoolManager.Instance.RequestTower(_towerType);
			tower.transform.position = _plotPos;
			onTowerPlaced?.Invoke(_plotPos, _assetDatabase.towers[_towerType], tower);
		}
	}
}