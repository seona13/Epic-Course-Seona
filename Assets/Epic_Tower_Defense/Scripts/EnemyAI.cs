using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
	private NavMeshAgent _agent;
	[SerializeField]
	private Transform _outlet;



	void Start()
	{
		_agent = GetComponent<NavMeshAgent>();
		if (_agent == null)
		{
			Debug.LogError("Missing NavMeshAgent on enemy");
		}
		_agent.SetDestination(_outlet.position);
		Debug.Log(_outlet.position);
	}


	void Update()
	{

	}
}