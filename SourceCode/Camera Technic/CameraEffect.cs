using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffect : MonoBehaviour {

	public void shakeCamera(float range,float duration)
	{
		object[] param = new object[2] { range, duration };
		StartCoroutine("shake", param);
	}

	IEnumerator shake(object[] param)
	{
		float range = (float)param[0];
		float duration = (float)param[1];
		Vector3 last = transform.position;

		int frame = (int)(duration / Time.deltaTime);
		for (int i = 0; i < frame; i++)
		{
			range -= range * 0.2f / duration;		//점점 더 화면 흔들리는 범위 좁힘

			float x = Random.Range(-range, range);
			float y = Random.Range(-range, range);
			transform.position = new Vector3(x, y, transform.position.z);
			yield return new WaitForEndOfFrame();
		}

		transform.position = last;
	}
}
