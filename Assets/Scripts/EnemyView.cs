using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
	//PlayerController player;
	public int viewType = 0;
	Transform playerTrans;
	Transform cameraTrans;
	Transform trans;
	// Start is called before the first frame update
    void Start()
    {
		trans			= transform;
		
    }

    // Update is called once per frame
    void Update()
    {
		if(viewType == 0)
			View_Player();
		else if (viewType == 1)
			View_Camera();
	}

	void View_Player()
	{
		//Debug.Log(1);
		if (playerTrans == null)
		{
			GameObject _go = GameObject.FindGameObjectWithTag("Player");
			if (_go != null)
			{
				playerTrans = _go.transform;
			}
			return;
		}
		//Debug.Log(12);

		//Vector3 _dir = playerTrans.position - trans.position;
		//Vector3 _angle = Quaternion.LookRotation(_dir).eulerAngles;
		//_angle.x = 0;
		//_angle.z = 0;
		//trans.eulerAngles = _angle;

		//---------------------------------
		//player <- Enemy : dir
		Vector3 _dir = playerTrans.position - trans.position;

		//player direction -> Rotation
		trans.rotation = Quaternion.LookRotation(_dir);

		//rotation -> angle and x, z clear
		Vector3 _angle = trans.rotation.eulerAngles;
		_angle.x = 0;
		_angle.z = 0;

		//angle -> rotaion
		trans.eulerAngles = _angle;
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
