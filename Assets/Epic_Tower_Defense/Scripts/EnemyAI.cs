using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;


public enum EnemyType { WALKER, TANK }

public class EnemyAI : MonoBehaviour
{
	public static Action<GameObject, int> onEnemyDie;

	private NavMeshAgent _agent;
	private Animator _anim;
	private Renderer[] _renderers;
	private WaitForSeconds _pauseBeforeCleanup;
	private WaitForEndOfFrame _disolvePause;

	[SerializeField]
	private EnemyType _type;
	[SerializeField]
	private int _maxHealth = 200;
	[SerializeField]
	private int _currentHealth;
	[SerializeField]
	private int _killValue = 50;
	private bool _isDead = false;
	private bool _isFiring = false;
	private GameObject _target;

	[Space(10)]

	[SerializeField]
	private GameObject _waistToTurn;
	[SerializeField]
	private ParentConstraint _parentConstraint;


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
		AttackTower.onEnemyEntered += CheckIfEnemy;
		AttackTower.onEnemyExited += StopFiring;

		Resurrect();

		_agent.Warp(SpawnManager.Instance.GetInlet().position);
		_agent.SetDestination(SpawnManager.Instance.GetOutlet().transform.position);

	}


	private void OnDisable()
	{
		AttackTower.onCallForDamage -= CheckIfTarget;
		GameDevHQ.FileBase.Missile_Launcher.Missile.Missile.onCallForDamage -= CheckIfTarget;
		AttackTower.onEnemyEntered -= CheckIfEnemy;
		AttackTower.onEnemyExited -= StopFiring;
	}


	private void CheckIfTarget(GameObject target, int amount)
	{
		if (gameObject == target)
		{
			TakeDamage(amount);
		}
	}


	void CheckIfEnemy(GameObject caller)
	{
		if (caller == gameObject)
		{
			_target = caller;
			_isFiring = true;
		}
	}


	void StopFiring(GameObject caller)
	{
		if (caller == gameObject)
		{
			_target = null;
			_isFiring = false;
		}
	}


	public void TakeDamage(int amount)
	{
		if (_isDead == false)
		{
			_currentHealth -= amount;
			if (_currentHealth <= 0)
			{
				_isDead = true;
				StartCoroutine(Die());
			}
		}
	}


	void Resurrect()
	{
		// Give enemy full health on spawn.
		_currentHealth = _maxHealth;

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
}

//IEnumerator DoAThingOverTime(Color start, Color end, float duration)
//{
//	for (float t = 0f; t < duration; t += Time.deltaTime)
//	{
//		float normalizedTime = t / duration;
//		//right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
//		someColorValue = Color.Lerp(start, end, normalizedTime);
//		yield return null;
//	}
//	someColorValue = end; //without this, the value will end at something like 0.9992367
//}
