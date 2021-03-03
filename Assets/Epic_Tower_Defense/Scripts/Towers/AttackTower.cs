﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AttackTower : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer _radiusMesh;
	[SerializeField]
	private GameObject _rotatingPart;
	public int damage;

	private Queue<EnemyAI> _targets = new Queue<EnemyAI>();
	private EnemyAI _currentTarget;



	private void OnEnable()
	{
		EnemyAI.onEnemyDie += EnemyDied;
	}


	private void OnDisable()
	{
		EnemyAI.onEnemyDie -= EnemyDied;
	}


	private void OnTriggerEnter(Collider other)
	{
		_targets.Enqueue(other.GetComponent<EnemyAI>());

		if (_currentTarget == null)
		{
			AcquireTarget();
		}
	}


	private void OnTriggerStay(Collider other)
	{
		LookAtTarget();
		Attack(_currentTarget);
	}


	private void OnTriggerExit(Collider other)
	{
		EnemyDied();
	}


	public virtual void Attack(EnemyAI target) { }


	public virtual void StopAttack() { }


	private void AcquireTarget()
	{
		if (_targets.Count > 0)
		{
			_currentTarget = _targets.Peek();
		}
		else
		{
			_currentTarget = null;
		}
	}


	private void LookAtTarget()
	{
		Quaternion lookRotation;

		if (_targets.Count > 0)
		{
			Vector3 directionToFace = _currentTarget.transform.position - _rotatingPart.transform.position;
			lookRotation = Quaternion.LookRotation(directionToFace);
		}
		else
		{
			lookRotation = Quaternion.LookRotation(Vector3.zero);
		}
		_rotatingPart.transform.rotation = Quaternion.Slerp(_rotatingPart.transform.rotation, lookRotation, Time.deltaTime * 10);
	}


	private void EnemyDied()
	{
		StopAttack();
		_targets.Dequeue();

		AcquireTarget();
	}
}