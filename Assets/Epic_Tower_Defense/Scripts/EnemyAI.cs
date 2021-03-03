﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyType { WALKER, TANK }

public class EnemyAI : MonoBehaviour
{
	public static Action onEnemyDie;

	private NavMeshAgent _agent;
	private Animator _anim;
	private WaitForSeconds _pauseBeforeCleanup;

	[SerializeField]
	private EnemyType _type;
	[SerializeField]
	private int _maxHealth = 200;
	[SerializeField]
	private int _currentHealth;
	[SerializeField]
	private int _killValue = 50;
	[SerializeField]
	private bool _isDead = false;


	void Awake()
	{
		_pauseBeforeCleanup = new WaitForSeconds(5f);

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
	}


	void OnEnable()
	{
		_agent.enabled = true;
		_agent.Warp(SpawnManager.Instance.GetInlet().position);
		_agent.SetDestination(SpawnManager.Instance.GetOutlet().transform.position);

		// Give enemy full health on spawn.
		_currentHealth = _maxHealth;

		// Make sure enemy is not in "dead" state
		_anim.SetBool("isDead", false);
		_isDead = false;
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


	IEnumerator Die()
	{
		_agent.isStopped = true;
		_agent.enabled = false;
		GameObject explosion = PoolManager.Instance.RequestExplosion();
		explosion.transform.position = transform.position;
		explosion.SetActive(true);
		_anim.SetBool("isDead", true);

		onEnemyDie?.Invoke();

		yield return _pauseBeforeCleanup;
		SpawnManager.Instance.DespawnEnemy(this);
	}
}