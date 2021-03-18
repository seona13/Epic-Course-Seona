using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;


public enum EnemyType { WALKER, TANK }

public class EnemyAI : MonoBehaviour
{
	public static event Action<GameObject, int> onEnemyDie;

	private NavMeshAgent _agent;
	private Animator _anim;
	private Damagable _damagable;
	private Renderer[] _renderers;
	private WaitForSeconds _pauseBeforeCleanup;
	private WaitForEndOfFrame _disolvePause;

	[SerializeField]
	private EnemyType _type;
	[SerializeField]
	private int _killValue = 50;
	private bool _isDead = false;

	[Space(10)]

	[SerializeField]
	private GameObject _waistToTurn;
	[SerializeField]
	private ParentConstraint _parentConstraint;
	private bool _isFiring = false;
	private List<GameObject> _targets = new List<GameObject>();
	private GameObject _currentTarget;


	void Awake()
	{
		_pauseBeforeCleanup = new WaitForSeconds(5f);
		_disolvePause = new WaitForEndOfFrame();

		_agent = GetComponent<NavMeshAgent>();
		if (_agent == null)
		{
			Debug.LogError("Missing NavMeshAgent on enemy");
		}

		_anim = GetComponent<Animator>();
		if (_anim == null)
		{
			Debug.LogError("Missing Animator on enemy");
		}

		_damagable = GetComponent<Damagable>();
		if (_damagable == null)
		{
			Debug.LogError("Missing Damagable component on enemy");
		}

		_renderers = GetComponentsInChildren<Renderer>();
		if (_renderers.Length == 0)
		{
			Debug.LogError("No renderers found on enemy");
		}
	}


	void OnEnable()
	{
		AttackTower.onCallForDamage += CheckIfTarget;
		GameDevHQ.FileBase.Missile_Launcher.Missile.Missile.onCallForDamage += CheckIfTarget;

		Resurrect();

		_agent.Warp(SpawnManager.Instance.GetInlet().position);
		_agent.SetDestination(SpawnManager.Instance.GetOutlet().transform.position);
	}


	void OnDisable()
	{
		AttackTower.onCallForDamage -= CheckIfTarget;
		GameDevHQ.FileBase.Missile_Launcher.Missile.Missile.onCallForDamage -= CheckIfTarget;
	}


	#region Targeting Towers
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Tower"))
		{
			_targets.Add(other.gameObject);

			if (_currentTarget == null)
			{
				AcquireTarget();
				StopCoroutine(StopFiring());
				StartFiring();
			}
		}
	}


	void OnTriggerStay(Collider other)
	{
		if (_currentTarget != null)
		{
			LookAtTarget();
		}
	}


	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Tower"))
		{
			TowerRemoved(other.gameObject, 0);
		}
	}


	void AcquireTarget()
	{
		if (_targets.Count > 0)
		{
			_currentTarget = _targets.First();
		}
		else
		{
			_currentTarget = null;
			StartCoroutine(StopFiring());
		}
	}


	void LookAtTarget()
	{
		Quaternion lookRotation;

		if (_currentTarget != null)
		{
			Vector3 directionToFace = _currentTarget.transform.position - _waistToTurn.transform.position;
			lookRotation = Quaternion.LookRotation(directionToFace);
		}
		else
		{
			lookRotation = Quaternion.LookRotation(Vector3.zero);
		}
		_waistToTurn.transform.rotation = Quaternion.Slerp(_waistToTurn.transform.rotation, lookRotation, Time.deltaTime * 10);
	}


	void StartFiring()
	{
		_isFiring = true;
		_anim.SetBool("isShooting", true);
	}


	IEnumerator StopFiring()
	{
		_isFiring = false;
		_anim.SetBool("isShooting", false);
		while (_waistToTurn.transform.rotation != Quaternion.LookRotation(Vector3.zero))
		{
			_waistToTurn.transform.rotation = Quaternion.Slerp(_waistToTurn.transform.rotation, Quaternion.LookRotation(Vector3.zero), Time.deltaTime * 10);
			yield return null;
		}
	}


	void TowerRemoved(GameObject tower, int value)
	{
		if (_targets.Contains(tower))
		{
			_targets.Remove(tower);
		}

		if (_currentTarget == tower)
		{
			AcquireTarget();
		}
	}
	#endregion


	#region Being Targeted
	void CheckIfTarget(GameObject target, int amount)
	{
		if (gameObject == target)
		{
			TakeDamage(amount);
		}
	}


	public void TakeDamage(int amount)
	{
		if (_isDead == false)
		{
			_damagable.TakeDamage(amount);

			if (_damagable.GetCurrentHealth() <= 0)
			{
				_isDead = true;
				StartCoroutine(Die());
			}
		}
	}
	#endregion


	#region Death and Resurrection
	void Resurrect()
	{
		_damagable.ResetHealth();

		// Make sure enemy is not in "dead" state
		_agent.enabled = true;
		_anim.SetBool("isDead", false);
		if (_parentConstraint != null)
		{
			_parentConstraint.constraintActive = true;
		}
		_isDead = false;

		// Set all renderers to "undisolved"
		for (int i = 0; i < _renderers.Length; i++)
		{
			_renderers[i].material.SetFloat("_fillAmount", 0);
		}
	}


	IEnumerator Die()
	{
		_agent.isStopped = true;
		_agent.enabled = false;
		GameObject explosion = PoolManager.Instance.RequestExplosion();
		explosion.transform.position = transform.position;
		_anim.SetBool("isDead", true);
		if (_parentConstraint != null)
		{
			_parentConstraint.constraintActive = false;
		}
		StartCoroutine(Disolve());

		onEnemyDie?.Invoke(gameObject, _killValue);

		yield return _pauseBeforeCleanup;
		SpawnManager.Instance.DespawnEnemy(this);
		explosion.SetActive(false);
	}


	IEnumerator Disolve()
	{
		float value = 0;

		while (value <= 1)
		{
			value += 0.005f;

			for (int i = 0; i < _renderers.Length; i++)
			{
				_renderers[i].material.SetFloat("_fillAmount", value);
			}

			yield return _disolvePause;
		}
	}
	#endregion
}