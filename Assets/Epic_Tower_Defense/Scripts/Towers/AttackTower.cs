using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum TowerType { GATLING, MISSILE };


public abstract class AttackTower : MonoBehaviour
{
	public static event Action<GameObject, int> onCallForDamage;

	[SerializeField]
	protected TowerType _towerType;
	[SerializeField]
	protected MeshRenderer _radiusMesh;
	[SerializeField]
	protected GameObject _rotatingPart;
	[SerializeField]
	protected int _damage;
	[SerializeField]
	protected float _fireRate = 0.5f;

	protected List<GameObject> _targets = new List<GameObject>();
	protected GameObject _currentTarget;



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
				Attack();
			}
		}
	}


	private void OnTriggerStay(Collider other)
	{
		if (_currentTarget != null)
		{
			LookAtTarget();
		}
	}


	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			EnemyDied(other.gameObject, 0);
		}
	}


	public virtual void Attack() { }


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
			StopAttack();
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


	protected void CallForDamage()
	{
		onCallForDamage?.Invoke(_currentTarget, _damage);
	}


	private void EnemyDied(GameObject enemy, int value)
	{
		if (_targets.Contains(enemy))
		{
			_targets.Remove(enemy);
		}

		if (_currentTarget == enemy)
		{
			AcquireTarget();
		}
	}
}