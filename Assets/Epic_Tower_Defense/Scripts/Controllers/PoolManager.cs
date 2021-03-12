using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoSingleton<PoolManager>
{
	[SerializeField]
	private Database _assetDatabase;
	[SerializeField]
	private int _defaultPoolSize = 10;

	[Space(10)]

	[SerializeField]
	private Transform _towerContainer;
	[SerializeField]
	private Transform _enemyContainer;
	[SerializeField]
	private Transform _explosionContainer;

	private Dictionary<int, List<GameObject>> _towerPools;
	private List<GameObject> _enemyPool;
	private List<GameObject> _explosionPool;



	public override void Init()
	{
		base.Init();
		_towerPools = new Dictionary<int, List<GameObject>>();
		_enemyPool = new List<GameObject>();
		_explosionPool = new List<GameObject>();
	}


	void Start()
	{
		GenerateEnemies(_defaultPoolSize);
		GenerateExplosions(_defaultPoolSize);
		GenerateTowers(_defaultPoolSize);
	}


	Dictionary<int, List<GameObject>> GenerateTowers(int amount)
	{
		for (int i = 0; i < _assetDatabase.towers.Length; i++)
		{
			_towerPools[i] = new List<GameObject>();

			GameObject newContainer = new GameObject();
			newContainer.transform.parent = _towerContainer;
			newContainer.name = i.ToString();

			for (int j = 0; j < amount; j++)
			{
				GameObject tower = Instantiate(_assetDatabase.towers[i].prefab);
				tower.transform.parent = newContainer.transform;
				tower.SetActive(false);

				_towerPools[i].Add(tower);
			}
		}
		return _towerPools;
	}


	public GameObject RequestTower(int towerType)
	{
		foreach (GameObject tower in _towerPools[towerType])
		{
			if (tower.activeInHierarchy == false)
			{
				tower.SetActive(true);
				return tower;
			}
		}

		GameObject newTower = Instantiate(_assetDatabase.towers[towerType].prefab);
		newTower.transform.parent = _towerContainer.Find(towerType.ToString());
		_towerPools[towerType].Add(newTower);

		return newTower;
	}


	List<GameObject> GenerateEnemies(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			GameObject enemy = Instantiate(_assetDatabase.enemies[Random.Range(0, _assetDatabase.enemies.Length)]);
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

		GameObject newEnemy = Instantiate(_assetDatabase.enemies[Random.Range(0, _assetDatabase.enemies.Length)]);
		newEnemy.transform.parent = _enemyContainer;
		_enemyPool.Add(newEnemy);

		return newEnemy;
	}


	List<GameObject> GenerateExplosions(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			GameObject explosion = Instantiate(_assetDatabase.explosion);
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

		GameObject newExplosion = Instantiate(_assetDatabase.explosion);
		newExplosion.transform.parent = _explosionContainer;
		_explosionPool.Add(newExplosion);

		return newExplosion;
	}
}