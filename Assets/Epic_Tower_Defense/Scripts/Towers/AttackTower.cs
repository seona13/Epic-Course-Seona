using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class AttackTower : MonoBehaviour
{
	public static event Action<GameObject, int> onCallForDamage;

	[SerializeField]
	private MeshRenderer _radiusMesh;
	[SerializeField]
	private GameObject _rotatingPart;
	public int damage;

	private List<GameObject> _targets = new List<GameObject>();
	private GameObject _currentTarget;



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
		if (other.CompareTag("Enemy"))
		{
			_targets.Add(other.gameObject);

			if (_currentTarget == null)
			{
				AcquireTarget();
			}
		}
	}


	private void OnTriggerStay(Collider other)
	{
		LookAtTarget();
		Attack(_currentTarget);
		CallForDamage();
	}


	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			EnemyDied(other.gameObject);
		}
	}


	public virtual void Attack(GameObject target) { }


	public virtual void StopAttack() { }


	private void AcquireTarget()
	{
		if (_targets.Count > 0)
		{
			_currentTarget = _targets.First();
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
			Vector3 offset = new Vector3(0, 1.3f, 0);
			Vector3 directionToFace = _currentTarget.transform.position - _rotatingPart.transform.position;
			directionToFace += offset;
			lookRotation = Quaternion.LookRotation(directionToFace);
		}
		else
		{
			lookRotation = Quaternion.LookRotation(Vector3.zero);
		}
		_rotatingPart.transform.rotation = Quaternion.Slerp(_rotatingPart.transform.rotation, lookRotation, Time.deltaTime * 10);
	}


	private void CallForDamage()
	{
		if (_currentTarget != null)
		{
			onCallForDamage?.Invoke(_currentTarget, damage);
		}
	}


	private void EnemyDied(GameObject enemy)
	{
		if (_targets.Contains(enemy))
		{
			_targets.Remove(enemy);
		}

		if (_currentTarget == enemy)
		{
			StopAttack();
			AcquireTarget();
		}
	}
}