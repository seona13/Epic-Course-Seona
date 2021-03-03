using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
	[SerializeField]
	private GameObject _enemyContainer;
	[SerializeField]
	private GameObject[] _enemies;
	private List<GameObject> _enemyPool;
	[SerializeField]
	private GameObject _explosion;



	public override void Init()
	{
		base.Init();
		_enemyPool = new List<GameObject>();
	}


	void Start()
	{
		GenerateEnemies(SpawnManager.Instance.GetWaveMultiplier());

		_explosion = Instantiate(_explosion);
		_explosion.transform.parent = _enemyContainer.transform;
		_explosion.SetActive(false);
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


	public GameObject RequestExplosion()
	{
		return _explosion;
	}
}
