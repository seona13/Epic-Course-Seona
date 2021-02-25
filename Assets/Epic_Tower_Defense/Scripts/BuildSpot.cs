using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildSpot : MonoBehaviour
{
	[SerializeField]
	private GameObject _particlesObject;
	private ParticleSystem _particles;
	private bool _available = true;
	private bool _canPlaceTower = false;



	void OnMouseEnter()
	{
		if (TowerPlacement.Instance.towerPlacementMode && _available)
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
		if (TowerPlacement.Instance.towerPlacementMode && _available)
		{
			TowerPlacement.Instance.LeaveBuildSpot();
		}
	}


	void Start()
	{
		_particles = _particlesObject.GetComponent<ParticleSystem>();
	}


	void Update()
	{
	}
}