using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *@author GameDevHQ 
 * For support, visit gamedevhq.com
 */

namespace GameDevHQ.FileBase.Missile_Launcher.Missile
{
	[RequireComponent(typeof(Rigidbody))] //require rigidbody
	[RequireComponent(typeof(AudioSource))] //require audiosource
	public class Missile : MonoBehaviour
	{
		public static event Action<GameObject, int> onCallForDamage;

		[SerializeField]
		private ParticleSystem _particle; //reference to the particle system
		[SerializeField]
		private GameObject _explosionPrefab; //Explosion prefab to play on impact
		[SerializeField]
		private float _launchSpeed; //launch speed of the rocket
		[SerializeField]
		private float _power; //power of the rocket
		[SerializeField]
		private float _fuseDelay; //fuse delay of the rocket

		private Rigidbody _rigidbody; //reference to the rigidbody of the rocket
		private AudioSource _audioSource; //reference to the audiosource of the rocket

		private bool _launched = false; //bool for if the rocket has launched
		private float _initialLaunchTime = 1.0f; //initial launch time for the rocket
		private bool _thrust; //bool to enable the rocket thrusters

		private bool _fuseOut = false; //bool for if the rocket fuse
		private bool _trackRotation = true; //bool to track rotation of the rocket

		private Transform _target;
		private int _damage;



		IEnumerator Start()
		{
			_rigidbody = GetComponent<Rigidbody>(); //assign the rigidbody component 
			_audioSource = GetComponent<AudioSource>(); //assign the audiosource component
			_audioSource.pitch = UnityEngine.Random.Range(0.7f, 1.9f); //randomize the pitch of the rocket audio
			_particle.Play(); //play the particles of the rocket
			_audioSource.Play(); //play the rocket sound

			yield return new WaitForSeconds(_fuseDelay); //wait for the fuse delay

			_initialLaunchTime = Time.time + 1.0f; //set the initial launch time
			_fuseOut = true; //set fuseOut to true
			_launched = true; //set the launch bool to true 
			_thrust = false; //set thrust bool to false
		}


		void FixedUpdate()
		{
			if (_fuseOut == false) //check if fuseOut is false
				return;

			if (_launched == true) //check if launched is true
			{
				_rigidbody.AddForce(transform.forward * _launchSpeed); //add force to the rocket in the forward direction

				if (Time.time > _initialLaunchTime + _fuseDelay) //check if the initial launch + fuse delay has passed
				{
					_launched = false; //launched bool goes false
					_thrust = true; //thrust bool goes true
				}
			}

			if (_thrust == true) //if thrust is true
			{
				_rigidbody.useGravity = true; //enable gravity 
				_rigidbody.velocity = transform.forward * _power; //set velocity multiplied by the power variable
				_thrust = false; //set thrust bool to false
				_trackRotation = true; //track rotation bool set to true
			}

			if (_trackRotation == true) //check track rotation bool
			{
				Vector3 direction = _target.position - transform.position; //calculate direciton for rocket to face
				direction.Normalize(); //set the magnitude of the vector to 1
				Vector3 turnAmount = Vector3.Cross(transform.forward, direction); //using cross product, we multiply our forward vector of the rocket by the direction vector, to create a perpendular vector, which specifies the turn amount

				_rigidbody.angularVelocity = turnAmount * _power; //apply angular velocity
				_rigidbody.velocity = transform.forward * _power; //apply forward velocity
			}
		}


		/// <summary>
		/// This method is used to assign traits to our missle from the launcher.
		/// </summary>
		public void AssignMissleRules(Transform target, float launchSpeed, float power, float fuseDelay, float destroyTimer, int damage)
		{
			_target = target; //Who should the rocket follow?
			_launchSpeed = launchSpeed; //set the launch speed
			_power = power; //set the power
			_fuseDelay = fuseDelay; //set the fuse delay
			_damage = damage; //set the damage done to targets
			Destroy(this.gameObject, destroyTimer); //destroy the rocket after destroyTimer 
		}


		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Enemy"))
			{
				CallForDamage(); // Damage collided object

				//if (_explosionPrefab != null)
				//	Instantiate(_explosionPrefab, transform.position, Quaternion.identity); //instantiate explosion
			}

			Destroy(this.gameObject); //destroy the rocket (this)
		}


		protected void CallForDamage()
		{
			onCallForDamage?.Invoke(_target.gameObject, _damage);
		}
	}
}