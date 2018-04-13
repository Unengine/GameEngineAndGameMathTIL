using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowPower : MonoBehaviour {

	float atk;
	float mAngle;
	float mPower;
	Rigidbody2D rd2d;

	// Use this for initialization
	void Start()
	{
		transform.Rotate(0, 0, mAngle);
		float y = Mathf.Tan(mAngle);

		rd2d = GetComponent<Rigidbody2D>();

		rd2d.AddForce(new Vector2(1, y).normalized * mPower);
	}

	public void setStatus(float angle, float power)
	{
		mAngle = angle;
		mPower = power;
	}

	void OnCollisionEnter2D()
	{
		rd2d.velocity = Vector2.zero;
		Debug.Log("Hit!");
		StartCoroutine("stick");
	}

	IEnumerator stick()
	{
		yield return new WaitForEndOfFrame();
		Destroy(gameObject);
	}
}
