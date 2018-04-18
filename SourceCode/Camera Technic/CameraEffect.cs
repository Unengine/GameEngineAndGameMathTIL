using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffect : MonoBehaviour {

	bool isShaking = false;

	public void shakeCamera(float range,float duration)
	{
		object[] param = new object[2] { range, duration };
		StartCoroutine("shake", param);
	}

	IEnumerator shake(object[] param)
	{
		isShaking = true;
		fxMessage();
		float range = (float)param[0];
		float duration = (float)param[1];
		Vector3 origin = transform.position;

		int frame = (int)(duration / Time.deltaTime);
		for (int i = 0; i < frame; i++)
		{
			float x = Random.Range(-range, range);
			float y = Random.Range(-range, range);
			transform.position = new Vector3(origin.x + x, origin.y + y, transform.position.z);

			range -= range * 0.05f / duration;      //점점 더 화면 흔들리는 범위 좁힘
			yield return new WaitForEndOfFrame();
		}

		//transform.position = last;
		isShaking = false;
		fxMessage();
	}

	public void fxMessage()
	{
		GetComponent<CameraFollow>().set_isFX(isShaking);
	}
}
