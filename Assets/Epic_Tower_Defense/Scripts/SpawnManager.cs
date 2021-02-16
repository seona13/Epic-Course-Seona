using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
	private static SpawnManager _Instance;
	public static SpawnManager Instance
	{
		get
		{
			if (_Instance == null)
			{
				var obj = new GameObject().AddComponent<SpawnManager>();
				obj.name = "Spawn_Manager";
				_Instance = obj.GetComponent<SpawnManager>();
			}
			return _Instance;
		}
	}

	[SerializeField]
	private Transform _inlet;
	[SerializeField]
	private Transform _outlet;
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
		if (_Instance != null)
		{
			Debug.LogWarning("Second instance of SpawnManager created. Automatic self-destruct triggered.");
			Destroy(this.gameObject);
		}
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
		if (_Instance == this)
		{
			_Instance = null;
		}
	}


	IEnumerator SpawnEnemy()
	{
		while (_spawnCount < _waveSpawns)
		{
			GameObject enemy = Instantiate(_enemies[Random.Range(0, _enemies.Length)], _inlet.position, Quaternion.identity, transform);
			enemy.GetComponent<EnemyAI>().SetDestination(_outlet.position);
			_spawnCount++;

			yield return _spawnRate;
		}
	}
}