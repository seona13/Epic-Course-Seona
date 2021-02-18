using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outlet : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();
			if (enemy != null)
			{
				SpawnManager.Instance.DespawnEnemy(enemy);
			}
			else
			{
				Debug.LogError("Missing EnemyAI on exiting enemy.");
			}
		}
	}
}
