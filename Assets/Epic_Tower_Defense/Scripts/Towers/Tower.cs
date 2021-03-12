using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Tower")]
public class Tower : ScriptableObject
{
	public int towerType;
	public string towerName;
	public GameObject prefab;
	public GameObject prototypePrefab;
	public Tower upgradesTo;
	public int buyFor;
	public int sellFor;
}