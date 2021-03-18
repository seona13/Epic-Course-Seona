using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Damagable : MonoBehaviour
{
	[SerializeField]
	private int _maxHealth = 200;
	[SerializeField]
	private int _currentHealth;



	public int GetCurrentHealth()
	{
		return _currentHealth;
	}


	public int GetMaxHealth()
	{
		return _maxHealth;
	}


	public void ResetHealth()
	{
		_currentHealth = _maxHealth;
	}


	public void TakeDamage(int amount)
	{
		_currentHealth -= amount;
	}
}