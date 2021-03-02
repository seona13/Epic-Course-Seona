using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AttackTower : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer _radiusMesh;
	[SerializeField]
	private GameObject _rotatingPart;

	private Queue<GameObject> _targets = new Queue<GameObject>();
	private GameObject _currentTarget;



	private void OnTriggerEnter(Collider other)
	{
		_targets.Enqueue(other.gameObject);

		if (_currentTarget == null)
		{
			AcquireTarget();
		}
	}


	private void OnTriggerStay(Collider other)
	{
		LookAtTarget();
		Attack();
	}


	private void OnTriggerExit(Collider other)
	{
		StopAttack();
		_targets.Dequeue();

		AcquireTarget();
	}


	public virtual void Attack() { }


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
			Debug.DrawRay(transform.position, directionToFace, Color.green);
		}
		else
		{
			lookRotation = Quaternion.LookRotation(Vector3.zero);
		}
		_rotatingPart.transform.rotation = Quaternion.Slerp(_rotatingPart.transform.rotation, lookRotation, Time.deltaTime * 10);
	}
}