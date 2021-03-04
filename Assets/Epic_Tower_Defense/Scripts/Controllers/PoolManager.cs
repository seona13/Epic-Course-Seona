using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoSingleton<PoolManager>
{
	[SerializeField]
	private Transform _enemyContainer;
	[SerializeField]
	private GameObject[] _enemies;
	private List<GameObject> _enemyPool;

	[Space(10)]

	[SerializeField]
	private Transform _explosionContainer;
	[SerializeField]
	private GameObject _explosion;
	private List<GameObject> _explosionPool;



	public override void Init()
	{
		base.Init();
		_enemyPool = new List<GameObject>();
		_explosionPool = new List<GameObject>();
	}


	void Start()
	{
		GenerateEnemies(SpawnManager.Instance.GetWaveMultiplier());
		GenerateExplosions(SpawnManager.Instance.GetWaveMultiplier());
	}


	List<GameObject> GenerateEnemies(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			GameObject enemy = Instantiate(_enemies[Random.Range(0, _enemies.Length)]);
			enemy.transform.parent = _enemyContainer;
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
		newEnemy.transform.parent = _enemyContainer;
		_enemyPool.Add(newEnemy);

		return newEnemy;
	}


	List<GameObject> GenerateExplosions(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			GameObject explosion = Instantiate(_explosion);
			explosion.transform.parent = _explosionContainer;
			explosion.SetActive(false);

			_explosionPool.Add(explosion);
		}
		return _explosionPool;
	}


	public GameObject RequestExplosion()
	{
		foreach (GameObject explosion in _explosionPool)
		{
			if (explosion.activeInHierarchy == false)
			{
				explosion.SetActive(true);
				return explosion;
			}
		}

		GameObject newExplosion = Instantiate(_explosion);
		newExplosion.transform.parent = _explosionContainer;
		_explosionPool.Add(newExplosion);

		return newExplosion;
	}
}