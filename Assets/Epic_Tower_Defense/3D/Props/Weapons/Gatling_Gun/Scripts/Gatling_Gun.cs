using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameDevHQ.FileBase.Gatling_Gun
{
	/// <summary>
	/// This script will allow you to view the presentation of the Turret and use it within your project.
	/// Please feel free to extend this script however you'd like. To access this script from another script
	/// (Script Communication using GetComponent) -- You must include the namespace (using statements) at the top. 
	/// "using GameDevHQ.FileBase.Gatling_Gun" without the quotes. 
	/// 
	/// For more, visit GameDevHQ.com
	/// 
	/// @authors
	/// Al Heck
	/// Jonathan Weinberger
	/// </summary>

	[RequireComponent(typeof(AudioSource))] //Require Audio Source component
	public class Gatling_Gun : AttackTower
	{
		[SerializeField]
		private Transform[] _gunBarrel; //Reference to hold the gun barrel
		public GameObject[] Muzzle_Flash; //reference to the muzzle flash effect to play when firing
		public ParticleSystem[] bulletCasings; //reference to the bullet casing effect to play when firing

		private AudioSource _audioSource; //reference to the audio source component
		private bool _startWeaponNoise = true;
		private WaitForSeconds _fireDelay;
		private Coroutine _damageRoutine;



		void Start()
		{
			_audioSource = GetComponent<AudioSource>(); //ssign the Audio Source to the reference variable

			_fireDelay = new WaitForSeconds(_fireRate);
		}


		void Update()
		{
			if (_currentTarget != null)
			{
				for (int i = 0; i < _gunBarrel.Length; i++)
				{
					_gunBarrel[i].transform.Rotate(Vector3.forward * Time.deltaTime * -500f);
					bulletCasings[i].Emit(1); //Emit the bullet casing particle effect  
				}
			}
		}


		public override void Attack()
		{
			if (_currentTarget != null)
			{
				base.Attack();
				for (int i = 0; i < Muzzle_Flash.Length; i++)
				{
					Muzzle_Flash[i].SetActive(true); //enable muzzle effect particle effect
				}

				_audioSource.Play(); //play audio clip attached to audio source

				_damageRoutine = StartCoroutine(DamageEnemy());
			}
		}


		public override void StopAttack()
		{
			base.StopAttack();
			for (int i = 0; i < Muzzle_Flash.Length; i++)
			{
				Muzzle_Flash[i].SetActive(false); //turn off muzzle flash particle effect
			}
			_audioSource.Stop(); //stop the sound effect from playing
			_startWeaponNoise = true; //set the start weapon noise value to true
			StopCoroutine(_damageRoutine);
		}


		IEnumerator DamageEnemy()
		{
			while (_currentTarget != null)
			{
				CallForDamage();
				yield return _fireDelay;
			}
		}
	}
}