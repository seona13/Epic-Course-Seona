using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
	[SerializeField]
	private float _zoomSpeed = 10f;
	[SerializeField]
	private float _panSpeed = 10f;
	[SerializeField]
	private float _panBorder = 25f;

	private float _zoomMin = 20f;
	private float _zoomMax = 40f;
	private float _panXMin = -40f;
	private float _panXMax = -20f;
	private float _panZMin = -25f;
	private float _panZMax = -7f;

	private Camera _cameraComp;
	private GameObject _parent;



	void Start()
	{
		_cameraComp = GetComponent<Camera>();
		_parent = transform.parent.gameObject;
	}


	void Update()
	{
		DoZoom();
		DoPan();
	}


	void DoZoom()
	{
		if (Input.mouseScrollDelta.y != 0)
		{
			_cameraComp.fieldOfView += -Input.mouseScrollDelta.y * _zoomSpeed * Time.deltaTime;
			_cameraComp.fieldOfView = Mathf.Clamp(_cameraComp.fieldOfView, _zoomMin, _zoomMax);
		}
	}


	void DoPan()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		if (Input.mousePosition.x < _panBorder)
		{
			horizontalInput = -1;
		}
		else if (Input.mousePosition.x > (Screen.width - _panBorder))
		{
			horizontalInput = 1;
		}

		float verticalInput = Input.GetAxis("Vertical");
		if (Input.mousePosition.y < _panBorder)
		{
			verticalInput = -1;
		}
		else if (Input.mousePosition.y > (Screen.height - _panBorder))
		{
			verticalInput = 1;
		}

		if (horizontalInput != 0 || verticalInput != 0)
		{
			_parent.transform.Translate(new Vector3(horizontalInput, 0, verticalInput) * _panSpeed * Time.deltaTime);
			Vector3 newPos = new Vector3();
			newPos.x = Mathf.Clamp(_parent.transform.position.x, _panXMin, _panXMax);
			newPos.y = _parent.transform.position.y;
			newPos.z = Mathf.Clamp(_parent.transform.position.z, _panZMin, _panZMax);
			_parent.transform.position = newPos;
		}
	}
}