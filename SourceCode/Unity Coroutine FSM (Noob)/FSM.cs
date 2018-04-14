using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour {

	Animator ani;
	Rigidbody2D rd2d;
	int horizontal;
	float walkStartTime;

	// Use this for initialization
	void Start () {
		ani = GetComponent<Animator>();
		rd2d = GetComponent<Rigidbody2D>();

		walkStartTime = 0;

		StartCoroutine("idle");
	}
	
	// Update is called once per frame
	void Update () {
		horizontal = (int)Input.GetAxisRaw("Horizontal");
	}

	IEnumerator idle()
	{
		bool flag = true;
		while(flag)
		{
			if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
			{
				ani.ResetTrigger("idle");
				ani.SetTrigger("walk");

				flag = false;
			}
			yield return new WaitForEndOfFrame();
		}
		StartCoroutine("walk");
	}

	IEnumerator walk()
	{
		bool flag = true;
		walkStartTime = Time.time;
		while(flag)
		{
			Debug.Log("walking");
			if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
			{
				ani.ResetTrigger("walk");
				ani.SetTrigger("idle");
			
				flag = false;
			}

			if(Time.time - walkStartTime > 2)
			{
				ani.ResetTrigger("walk");
				ani.SetTrigger("run");

				flag = false;
			}
			move();

			yield return new WaitForEndOfFrame();
		}
		StartCoroutine("idle");
	}

	IEnumerator run()
	{
		bool flag = true;
		while(flag)
		{
			if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
			{
				ani.ResetTrigger("run");
				ani.SetTrigger("idle");
				flag = false;
			}
			move();

			yield return new WaitForEndOfFrame();
		}
	}

	void move()
	{
		Vector2 newPos = transform.position;
		Vector3 localScale = transform.localScale;
		if(horizontal > 0)
		{
			localScale.x = Mathf.Abs(localScale.x);
			newPos += Vector2.right * Time.deltaTime;
		}
		else if(horizontal < 0)
		{
			localScale.x = -Mathf.Abs(localScale.x);
			newPos += Vector2.left * Time.deltaTime;
		}

		transform.localScale = localScale;
		rd2d.MovePosition(newPos);
	}
}
