using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowerPlacement : MonoSingleton<TowerPlacement>
{
	[SerializeField]
	private Transform _towerContainer;
	[SerializeField]
	private GameObject[] _towers;
	[SerializeField]
	private GameObject[] _prototypes;
	private GameObject[] _prototypePool;
	private int _index;
	private MeshRenderer _radius;
	private bool _followMouse = true;

	public bool towerPlacementMode = false;
	public static event Action<bool> onPlacementModeActive;



	void Start()
	{
		_prototypePool = new GameObject[_prototypes.Length];
		for (int i = 0; i < _prototypes.Length; i++)
		{
			_prototypePool[i] = Instantiate(_prototypes[i], transform);
			_prototypePool[i].SetActive(false);
		}
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			PlacmentModeActive(true);
			DeactivatePrototypes();
			_index = 0;
			_prototypePool[_index].SetActive(true);
			_radius = _prototypePool[_index].GetComponentInChildren<Radius>().GetComponent<MeshRenderer>();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			PlacmentModeActive(true);
			DeactivatePrototypes();
			_index = 1;
			_prototypePool[_index].SetActive(true);
			_radius = _prototypePool[_index].GetComponentInChildren<Radius>().GetComponent<MeshRenderer>();
		}

		// Right-click to exit placement mode
		if (towerPlacementMode && Input.GetMouseButtonDown(1))
		{
			PlacmentModeActive(false);
			DeactivatePrototypes();
		}

		if (towerPlacementMode && _followMouse)
		{
			Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(rayOrigin, out RaycastHit hitInfo))
			{
				_prototypePool[_index].transform.position = hitInfo.point;
			}
		}
	}


	void DeactivatePrototypes()
	{
		for (int i = 0; i < _prototypes.Length; i++)
		{
			_prototypePool[i].SetActive(false);
		}
		_radius = null;
	}


	public void PlacmentModeActive(bool status)
	{
		towerPlacementMode = status;
		onPlacementModeActive?.Invoke(status);
	}


	public void PossibleTowerPlacement(Vector3 pos) // OnMouseEnter
	{
		_followMouse = false;
		// snap to build spot
		_prototypePool[_index].transform.position = pos;
		// turn radius green
		_radius.material.color = Color.green;
	}


	public void PlaceTower(Vector3 pos) // OnMouseDown
	{
		Instantiate(_towers[_index], pos, Quaternion.identity, _towerContainer);
		_followMouse = true;
		_radius.material.color = Color.red;
	}


	public void LeaveBuildSpot()
	{
		_followMouse = true;
		_radius.material.color = Color.red;
	}
}