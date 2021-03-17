using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthBar : MonoBehaviour
{
	MaterialPropertyBlock _matBlock;
	MeshRenderer _meshRenderer;
	Camera _mainCamera;
	Damagable _damagable;



	void Awake()
	{
		_meshRenderer = GetComponent<MeshRenderer>();
		_matBlock = new MaterialPropertyBlock();
		_damagable = GetComponentInParent<Damagable>();
	}


	void Start()
	{
		_mainCamera = Camera.main;
	}


	void Update()
	{
		if (_damagable.GetCurrentHealth() < _damagable.GetMaxHealth())
		{
			_meshRenderer.enabled = true;
			AlignCamera();
			UpdateParams();
		}
		else
		{
			_meshRenderer.enabled = false;
		}
	}


	void AlignCamera()
	{
		if (_mainCamera != null)
		{
			var camXform = _mainCamera.transform;
			var forward = transform.position - camXform.position;
			forward.Normalize();
			var up = Vector3.Cross(forward, camXform.right);
			transform.rotation = Quaternion.LookRotation(forward, up);
		}
	}


	void UpdateParams()
	{
		_meshRenderer.GetPropertyBlock(_matBlock);
		_matBlock.SetFloat("_Fill", _damagable.GetCurrentHealth() / (float)_damagable.GetMaxHealth());
		_meshRenderer.SetPropertyBlock(_matBlock);
	}
}