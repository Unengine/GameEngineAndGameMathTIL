using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	Transform target;
	Vector3 targetPos;
	bool isFX;
	
	// Use this for initialization
	void Start () {
		target = GameObject.Find("black_knight").transform;
	}
	
	// Update is called once per frame
	void Update () {
		targetPos = target.position;

		if (!isFX)
		{
			transform.position = lerpVector(transform.position, targetPos + new Vector3(0, 3), Time.deltaTime * 2);
		}
	}

	Vector3 lerpVector(Vector3 a, Vector3 b, float t)	//벡터의 선형 보간
	{
		Vector3 vec;

		vec.x = (1 - t) * a.x + t * b.x;
		vec.y = (1 - t) * a.y + t * b.y;
		vec.z = transform.position.z;

		return vec;
	}

	public void set_isFX(bool condition)
	{
		isFX = condition;
	}
}
