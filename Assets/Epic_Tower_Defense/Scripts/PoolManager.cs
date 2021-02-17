using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
	private static PoolManager _instance;
	public static PoolManager Instance
	{
		get
		{
			if (_instance == null)
			{
				Debug.LogError("Missing PoolManager.");
			}

			return _instance;
		}
	}

	[SerializeField]
	private GameObject _enemyContainer;
	[SerializeField]
	private GameObject[] _enemies;
	private List<GameObject> _enemyPool;



	void Awake()
	{
		_instance = this;
	}


	private void Start()
	{
		_enemyPool = new List<GameObject>();
		GenerateEnemies(10);
	}


	List<GameObject> GenerateEnemies(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			GameObject enemy = Instantiate(_enemies[Random.Range(0, _enemies.Length)]);
			enemy.transform.parent = _enemyContainer.transform;
			enemy.SetActive(false);
			_enemyPool.Add(enemy);
		}
		return _enemyPool;
	}


	public GameObject RequestEnemy()
	{
		foreach (GameObject enemy in _enemyPool)
		{
			if (enemy.activeInHierarchy == false)
			{
				enemy.SetActive(true);
				return enemy;
			}
		}

		GameObject newEnemy = Instantiate(_enemies[Random.Range(0, _enemies.Length)]);
		newEnemy.transform.parent = _enemyContainer.transform;
		_enemyPool.Add(newEnemy);

		return newEnemy;
	}
}
