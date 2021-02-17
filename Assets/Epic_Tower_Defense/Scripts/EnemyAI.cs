﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyType { WALKER, TANK }

public class EnemyAI : MonoBehaviour
{
	private NavMeshAgent _agent;

	[SerializeField]
	private EnemyType _type;
	[SerializeField]
	private int _maxHealth = 200;
	[SerializeField]
	private int _currentHealth;
	[SerializeField]
	private int _killValue = 50;


	void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
		if (_agent == null)
		{
			Debug.LogError("Missing NavMeshAgent on enemy");
		}
	}


	void OnEnable()
	{
		_agent.Warp(SpawnManager.Instance.inlet.position);
		_agent.SetDestination(SpawnManager.Instance.outlet.transform.position);

		// Give enemy full health on spawn.
		_currentHealth = _maxHealth;
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Outlet"))
		{
			SpawnManager.Instance.DespawnEnemy(this);
		}
	}
}