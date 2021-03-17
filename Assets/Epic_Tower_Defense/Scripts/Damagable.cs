using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Damagable : MonoBehaviour
{
	[SerializeField]
	private int _maxHealth = 200;
	[SerializeField]
	private int _currentHealth;



	private void OnEnable()
	{
		EnemyAI.onEnemyRespawn += ResetHealth;
		EnemyAI.onEnemyDamaged += TakeDamage;
	}


	private void OnDisable()
	{
		EnemyAI.onEnemyRespawn -= ResetHealth;
		EnemyAI.onEnemyDamaged -= TakeDamage;
	}


	public int GetCurrentHealth()
	{
		return _currentHealth;
	}


	public int GetMaxHealth()
	{
		return _maxHealth;
	}


	void ResetHealth()
	{
		_currentHealth = _maxHealth;
	}


	void TakeDamage(int amount)
	{
		_currentHealth -= amount;
	}
}