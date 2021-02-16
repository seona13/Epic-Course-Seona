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



	void Awake()
	{
		if (_Instance != null)
		{
			Debug.LogWarning("Second instance of SpawnManager created. Automatic self-destruct triggered.");
			Destroy(this.gameObject);
		}
	}


	private void OnDestroy()
	{
		if (_Instance == this)
		{
			_Instance = null;
		}
	}
}