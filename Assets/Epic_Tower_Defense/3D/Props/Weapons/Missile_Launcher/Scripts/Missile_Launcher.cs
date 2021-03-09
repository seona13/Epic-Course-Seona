using GameDevHQ.FileBase.Missile_Launcher.Missile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *@author GameDevHQ 
 * For support, visit gamedevhq.com
 */

namespace GameDevHQ.FileBase.Missile_Launcher
{
	public class Missile_Launcher : AttackTower
	{
		[SerializeField]
		private GameObject _missilePrefab; //holds the missle gameobject to clone
		[SerializeField]
		private GameObject[] _missilePositions; //array to hold the rocket positions on the turret
		[SerializeField]
		private float _launchSpeed; //initial launch speed of the rocket
		[SerializeField]
		private float _power; //power to apply to the force of the rocket
		[SerializeField]
		private float _fuseDelay; //fuse delay before the rocket launches
		[SerializeField]
		private float _reloadTime; //time in between reloading the rockets
		[SerializeField]
		private float _destroyTime = 10.0f; //how long till the rockets get cleaned up



		private void Update()
		{
		}


		public override void Attack()
		{
			base.Attack();
			StartCoroutine(FireRocketsRoutine()); //start a coroutine that fires the rockets. 
		}


		public override void StopAttack()
		{
			base.StopAttack();
			// Do we need to do this manually?
			//StopCoroutine(FireRocketsRoutine());
		}


		IEnumerator FireRocketsRoutine()
		{
			while (_currentTarget != null)
			{
				for (int i = 0; i < _missilePositions.Length; i++) //for loop to iterate through each missle position
				{
					GameObject rocket = Instantiate(_missilePrefab) as GameObject; //instantiate a rocket

					rocket.transform.parent = _missilePositions[i].transform; //set the rockets parent to the missle launch position 
					rocket.transform.localPosition = Vector3.zero; //set the rocket position values to zero
					rocket.transform.localEulerAngles = new Vector3(-90, 0, 0); //set the rotation values to be properly aligned with the rockets forward direction
					rocket.transform.parent = null; //set the rocket parent to null

					rocket.GetComponent<GameDevHQ.FileBase.Missile_Launcher.Missile.Missile>().AssignMissleRules(_currentTarget.transform, _launchSpeed, _power, _fuseDelay, _destroyTime, _damage); //assign missle properties 

					_missilePositions[i].SetActive(false); //turn off the rocket sitting in the turret to make it look like it fired

					yield return new WaitForSeconds(_fireRate); //wait for the firedelay
				}
			}

			for (int i = 0; i < _missilePositions.Length; i++) //itterate through missle positions
			{
				yield return new WaitForSeconds(_reloadTime); //wait for reload time
				_missilePositions[i].SetActive(true); //enable fake rocket to show ready to fire
			}
		}
	}
}