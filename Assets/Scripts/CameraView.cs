using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{
	Transform cameraTrans;
	Transform trans;
	// Start is called before the first frame update
	void Start()
	{
		trans = transform;
		cameraTrans = Camera.main.transform;
	}

	// Update is called once per frame
	void Update()
	{
		View_Camera();
	}

	void View_Camera()
	{
		if (cameraTrans == null)
		{
			Camera _camera = Camera.main;
			if (_camera != null)
			{
				cameraTrans = _camera.transform;
			}
			return;
		}

		//---------------------------------
		//Camera <- Enemy : dir
		Vector3 _dir = cameraTrans.position - trans.position;

		//Camera direction -> Rotation
		trans.rotation = Quaternion.LookRotation(_dir);

		//rotation -> angle and x, z clear
		Vector3 _angle = trans.rotation.eulerAngles;
		_angle.x = 0;
		_angle.z = 0;

		//angle -> rotaion
		trans.eulerAngles = _angle;
	}
}
