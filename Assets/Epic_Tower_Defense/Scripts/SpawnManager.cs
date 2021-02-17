using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
	private static SpawnManager _instance;
	public static SpawnManager Instance
	{
		get
		{
			if (_instance == null)
			{
				Debug.LogError("Missing SpawnManager");
			}
			return _instance;
		}
	}

	public Transform inlet;
	public Transform outlet;
	[SerializeField]
	private GameObject[] _enemies;

	private WaitForSeconds _spawnRate = new WaitForSeconds(2.5f);
	private int _waveCount = 1;
	private int _waveMultiplier = 10;
	[SerializeField]
	private int _waveSpawns;
	[SerializeField]
	private int _spawnCount = 0;
	[SerializeField]
	private int _killCount = 0;



	void Awake()
	{
		if (_instance != null)
		{
			Debug.LogWarning("Second instance of SpawnManager created. Automatic self-destruct triggered.");
			Destroy(this.gameObject);
		}
		_instance = this;
	}


	void Start()
	{
		_waveSpawns = _waveCount * _waveMultiplier;
		StartCoroutine(SpawnEnemy());
	}


	void Update()
	{
		if (_killCount == _waveSpawns)
		{
			StopCoroutine(SpawnEnemy());
			_waveCount++;
			_waveSpawns = _waveCount * _waveMultiplier;
			_spawnCount = 0;
			_killCount = 0;
			StartCoroutine(SpawnEnemy());
		}
	}


	void OnDestroy()
	{
		if (_instance == this)
		{
			_instance = null;
		}
	}


	IEnumerator SpawnEnemy()
	{
		while (_spawnCount < _waveSpawns)
		{
			GameObject enemy = PoolManager.Instance.RequestEnemy();
			_spawnCount++;

			yield return _spawnRate;
		}
	}


	public void DespawnEnemy(EnemyAI enemy)
	{
		_killCount++;
		enemy.gameObject.SetActive(false);
	}
}