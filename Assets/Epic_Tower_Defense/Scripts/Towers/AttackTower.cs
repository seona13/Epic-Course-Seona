using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AttackTower : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer _radiusMesh;

	private Queue<GameObject> _targets = new Queue<GameObject>();
	private GameObject _currentTarget;



	private void OnTriggerEnter(Collider other)
	{
		_targets.Enqueue(other.gameObject);
		Debug.Log("Tower:: Targets in queue: " + _targets.Count);
		Debug.Log(_targets.Peek().name);

		if (_currentTarget == null)
		{
			_currentTarget = other.gameObject;
		}
	}


	private void OnTriggerStay(Collider other)
	{
		Attack();
	}


	private void OnTriggerExit(Collider other)
	{
		StopAttack();
		_targets.Dequeue();
		Debug.Log("Tower:: Target removed from queue");

		if (_targets.Count > 0)
		{
			_currentTarget = _targets.Peek();
		}
	}


	public virtual void Attack() { }


	public virtual void StopAttack() { }
}