using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	private float _zoomSpeed = 10f;

	private Camera _cameraComp;



	void Start()
	{
		_cameraComp = GetComponent<Camera>();
	}


	void Update()
	{
		if (Input.mouseScrollDelta.y != 0)
		{
			_cameraComp.fieldOfView += -Input.mouseScrollDelta.y * _zoomSpeed * Time.deltaTime;
			_cameraComp.fieldOfView = Mathf.Clamp(_cameraComp.fieldOfView, 20, 40);
		}
	}
}
