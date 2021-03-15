using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Outlet : MonoBehaviour
{
	public static event Action onEnemyEscaped;


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();
			if (enemy != null)
			{
				onEnemyEscaped?.Invoke();
				SpawnManager.Instance.DespawnEnemy(enemy);
			}
			else
			{
				Debug.LogError("Missing EnemyAI on exiting enemy.");
			}
		}
	}
}