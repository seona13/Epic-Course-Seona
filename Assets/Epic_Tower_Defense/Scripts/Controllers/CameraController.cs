using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
	[SerializeField]
	private float _zoomSpeed = 10f;
	private float _zoomMin = 20f;
	private float _zoomMax = 40f;

	[SerializeField]
	private float _panSpeed = 10f;
	[SerializeField]
	private float _edgeBuffer = 0.05f;
	[SerializeField]
	private bool _edgeScrolling = true;
	private float _panXMin = -40f, _panXMax = -20f;
	private float _panZMin = -25f, _panZMax = -7f;
	private float _leftEdge, _rightEdge, _bottomEdge, _topEdge;

	private Vector3 _direction;
	private Vector3 _mousePos;
	private bool _inBounds;



	void Start()
	{
		CalculateEdges();
	}


	void Update()
	{
		if (_edgeScrolling)
		{
			BoundaryCheck();
		}
		DoPan();
		DoZoom();
	}


	void DoZoom()
	{
		if (Input.mouseScrollDelta.y != 0)
		{
			Camera.main.fieldOfView += -Input.mouseScrollDelta.y * _zoomSpeed * Time.deltaTime;
			Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, _zoomMin, _zoomMax);
		}
	}


	void DoPan()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		_mousePos = Input.mousePosition;

		if (_edgeScrolling && _inBounds)
		{
			if (Input.mousePosition.x < _leftEdge)
			{
				horizontalInput = -1;
			}
			else if (Input.mousePosition.x > _rightEdge)
			{
				horizontalInput = 1;
			}

			if (Input.mousePosition.y < _bottomEdge)
			{
				verticalInput = -1;
			}
			else if (Input.mousePosition.y > _topEdge)
			{
				verticalInput = 1;
			}
		}

		_direction.x = horizontalInput;
		_direction.z = verticalInput;

		transform.Translate(_direction * _panSpeed * Time.deltaTime);
		Vector3 newPos = transform.position;
		newPos.x = Mathf.Clamp(newPos.x, _panXMin, _panXMax);
		newPos.z = Mathf.Clamp(newPos.z, _panZMin, _panZMax);
		transform.position = newPos;
	}


	void CalculateEdges()
	{
		_leftEdge = Screen.width * _edgeBuffer;
		_rightEdge = Screen.width * (1 - _edgeBuffer);
		_bottomEdge = Screen.height * _edgeBuffer;
		_topEdge = Screen.height * (1 - _edgeBuffer);
	}


	void BoundaryCheck()
	{
		if (_mousePos.x < 0 || _mousePos.x > Screen.width || _mousePos.y < 0 || _mousePos.y > Screen.height)
		{
			_inBounds = false;
		}
		else
		{
			_inBounds = true;
		}
	}
}