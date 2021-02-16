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
		for (int i = 0; i < 10; i++)
		{
			SpawnEnemy();
		}
	}


	void OnDestroy()
	{
		if (_Instance == this)
		{
			_Instance = null;
		}
	}


	void SpawnEnemy()
	{
		GameObject enemy = Instantiate(_enemies[Random.Range(0, _enemies.Length)], _inlet.position, Quaternion.identity, transform);
		enemy.GetComponent<EnemyAI>().SetDestination(_outlet.position);
	}
}