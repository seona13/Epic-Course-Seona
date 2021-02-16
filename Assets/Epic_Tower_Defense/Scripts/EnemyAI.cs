using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
	private NavMeshAgent _agent;
	[SerializeField]
	private Transform _outlet;
	[SerializeField]
	private int _maxHealth = 200;
	[SerializeField]
	private int _currentHealth;
	[SerializeField]
	private int _killValue = 50;


	void Start()
	{
		_agent = GetComponent<NavMeshAgent>();
		if (_agent == null)
		{
			Debug.LogError("Missing NavMeshAgent on enemy");
		}
		_agent.SetDestination(_outlet.position);

		// Give enemy full health on spawn.
		_currentHealth = _maxHealth;
	}


	void Update()
	{

	}
}