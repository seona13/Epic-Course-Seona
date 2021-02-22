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

	private Camera _cameraComp;
	private GameObject _parent;



	void Start()
	{
		_cameraComp = GetComponent<Camera>();
		_parent = transform.parent.gameObject;
	}


	void Update()
	{
		if (Input.mouseScrollDelta.y != 0)
		{
			_cameraComp.fieldOfView += -Input.mouseScrollDelta.y * _zoomSpeed * Time.deltaTime;
			_cameraComp.fieldOfView = Mathf.Clamp(_cameraComp.fieldOfView, 20, 40);
		}

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
			_parent.transform.position = new Vector3(Mathf.Clamp(_parent.transform.position.x, -40, -20), _parent.transform.position.y, Mathf.Clamp(_parent.transform.position.z, -25, -7));
		}
	}
}
