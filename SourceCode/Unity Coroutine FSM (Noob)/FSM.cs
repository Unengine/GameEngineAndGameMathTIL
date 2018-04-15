using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour {

	Animator ani;
	Rigidbody2D rd2d;
	int horizontal;
	float walkStartTime;
	float movementSpeed;

	bool isFixed;
	bool isMovable;
	bool isAttacking;

	// Use this for initialization
	void Start () {
		ani = GetComponent<Animator>();
		rd2d = GetComponent<Rigidbody2D>();

		walkStartTime = 0;
		movementSpeed = 1.5f;

		isFixed = false;
		isMovable = true;
		isAttacking = false;

		StartCoroutine("idle");
	}
	
	// Update is called once per frame
	void Update () {
		isFixed = false;
		horizontal = (int)Input.GetAxisRaw("Horizontal");

		if(Input.GetKey(KeyCode.LeftShift))
		{
			isFixed = true;
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{	
			if(!isAttacking)
				StartCoroutine("attack");
		}
	}

	IEnumerator idle()
	{
		ani.SetTrigger("idle");
		while(true)
		{
			//Debug.Log("idle");
			if(isAttacking)
			{
				break;
			}

			if (horizontal != 0)
			{
				StartCoroutine("walk");
				break;
			}

			yield return new WaitForEndOfFrame();
		}
		ani.ResetTrigger("idle");
	}

	IEnumerator walk()
	{
		ani.SetTrigger("walk");
		walkStartTime = Time.time;
		while(true)
		{

			if (isAttacking)
			{
				break;
			}

			if (horizontal == 0)
			{
				StartCoroutine("idle");

				break;
			}

			if(Time.time - walkStartTime > 2 || isFixed)
			{
				StartCoroutine("run");

				break;
			}
			move(movementSpeed);

			yield return new WaitForEndOfFrame();
		}
		ani.ResetTrigger("walk");
	}

	IEnumerator run()
	{
		ani.SetTrigger("run");
		movementSpeed = 3f;
		while(true)
		{

			if (isAttacking)
			{
				break;
			}
			if (horizontal == 0 && !isFixed)
			{
				movementSpeed = 1f;
				StartCoroutine("idle");
				break;
			}
			move(movementSpeed);

			yield return new WaitForEndOfFrame();
		}
		ani.ResetTrigger("run");
	}

	IEnumerator attack()
	{
		Debug.Log("attack!");

		isAttacking = true;
		ani.SetTrigger("attack");

		yield return new WaitForSeconds(1);

		while(ani.GetCurrentAnimatorStateInfo(0).IsName("skill_1"))
		{
			yield return new WaitForEndOfFrame();
		}

		isAttacking = false;
		StartCoroutine("idle");

		ani.ResetTrigger("attack");
		yield return new WaitForEndOfFrame();
	}

	void move(float speed)
	{
		//Debug.Log(speed);

		Vector2 newPos = transform.position;
		Vector3 localScale = transform.localScale;
		if(horizontal > 0)
		{
			localScale.x = Mathf.Abs(localScale.x);
			newPos += Vector2.right * Time.deltaTime * speed;
		}
		else if(horizontal < 0)
		{
			localScale.x = -Mathf.Abs(localScale.x);
			newPos += Vector2.left * Time.deltaTime * speed;
		}
		else if(isFixed)
		{
			Debug.Log("Fixed");
			if(localScale.x > 0)
				newPos += Vector2.right * Time.deltaTime * speed;
			else if(localScale.x < 0)
				newPos += Vector2.left * Time.deltaTime * speed;
		}

		transform.localScale = localScale;
		rd2d.MovePosition(newPos);
	}
}
