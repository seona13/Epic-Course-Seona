using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Database")]
public class Database : ScriptableObject
{
	public Tower[] towers;

	[Space(10)]

	public GameObject[] enemies;

	[Space(10)]

	public GameObject explosion;
}