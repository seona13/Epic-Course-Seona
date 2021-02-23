using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoSingleton<SpawnManager>
{
	[SerializeField]
	private Transform inlet;
	[SerializeField]
	private Transform outlet;
	[SerializeField]
	private GameObject[] _enemies;

	private WaitForSeconds _spawnRate = new WaitForSeconds(2.5f);
	private int _waveCount = 1;
	[SerializeField]
	private int waveMultiplier;
	[SerializeField]
	private int _waveSpawns;
	[SerializeField]
	private int _spawnCount = 0;
	[SerializeField]
	private int _killCount = 0;



	void Start()
	{
		_waveSpawns = _waveCount * waveMultiplier;
		StartCoroutine(SpawnEnemy());
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

		if (_killCount == _waveSpawns)
		{
			StopCoroutine(SpawnEnemy());
			_waveCount++;
			_waveSpawns = _waveCount * waveMultiplier;
			_spawnCount = 0;
			_killCount = 0;
			StartCoroutine(SpawnEnemy());
		}

		enemy.gameObject.SetActive(false);
	}


	#region Data access
	public Transform GetInlet()
	{
		return inlet;
	}


	public Transform GetOutlet()
	{
		return outlet;
	}


	public int GetWaveMultiplier()
	{
		return waveMultiplier;
	}
	#endregion
}