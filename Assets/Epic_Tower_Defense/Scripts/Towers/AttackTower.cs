using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AttackTower : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer _radiusMesh;
	[SerializeField]
	private GameObject _rotatingPart;
	public int damage;

	private List<EnemyAI> _targets = new List<EnemyAI>();
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
		_targets.Add(other.GetComponent<EnemyAI>());

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
		EnemyDied(other.GetComponent<EnemyAI>());
	}


	public virtual void Attack(EnemyAI target) { }


	public virtual void StopAttack() { }


	private void AcquireTarget()
	{
		if (_targets.Count > 0)
		{
			_currentTarget = _targets[0];
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


	private void EnemyDied(EnemyAI enemy)
	{
		if (_currentTarget == enemy)
		{
			StopAttack();
			_targets.Remove(_currentTarget);
			AcquireTarget();
		}
		else
		{
			_targets.Remove(enemy);
		}
	}
}