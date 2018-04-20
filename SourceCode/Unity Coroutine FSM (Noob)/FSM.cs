using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour {

	public enum States
	{
		idle = 0,
		walk,
		run,
		attack
	}

	public States state;
	Animator ani;
	Rigidbody2D rd2d;

	bool isFixed = false;
	bool atkFlag = false;
	bool jumpFlag = false;
	bool isGround = false;

	float horizontal;
	float movementSpeed;
	float currentSpeed;
	float lastSpeed;
	float jumpPower;
	float walkTime = 0;
	float stopTime = 0;

	// Use this for initialization
	void Start () {
		state = States.idle;
		ani = GetComponent<Animator>();
		rd2d = GetComponent<Rigidbody2D>();
		jumpPower = GetComponent<KnightStat>().getJumpPower();

		currentSpeed = transform.position.x;
	}

	// Update is called once per frame
	void Update()
	{
		lastSpeed = currentSpeed;
		currentSpeed = transform.position.x;
		float delta = currentSpeed - lastSpeed;

		Debug.Log(delta);
		horizontal = Input.GetAxisRaw("Horizontal");
		movementSpeed = ani.GetFloat("movementSpeed");

		if (Input.GetKey(KeyCode.LeftShift) && isGround)
		{
			isFixed = true;
		}
		else
		{
			isFixed = false;
		}

		if(Input.GetKeyDown(KeyCode.Space) && isGround)
		{
			jump();
		}

		if(Input.GetKeyDown(KeyCode.RightShift))
		{
			state = States.attack;
		}

		switch(state)
		{
			case States.idle:
				stopTime += Time.deltaTime;
				StartCoroutine(idle());
				break;
			case States.walk:
				walkTime += Time.deltaTime;
				StartCoroutine(walk());
				break;
			case States.run:
				StartCoroutine(run());
				break;
			case States.attack:
				if(!atkFlag)
					StartCoroutine(attack());
				break;
		}
	}

	IEnumerator idle()
	{
		ani.SetTrigger("idle");
		if (stopTime >= 0.5f)
		{
			walkTime = 0;
		}

		if (horizontal != 0)
		{
			ani.ResetTrigger("idle");
			state = States.walk;
		}
		yield return new WaitForEndOfFrame();
	}

	IEnumerator walk()
	{
		stopTime = 0;
		ani.SetTrigger("walk");
		ani.SetFloat("movementSpeed", 2);
		move(movementSpeed);
		if(horizontal == 0)
		{
			ani.ResetTrigger("walk");
			state = States.idle;
		}

		if(walkTime >= 2 || isFixed)
		{
			ani.ResetTrigger("walk");
			state = States.run;
		}

		yield return new WaitForEndOfFrame();
	}

	IEnumerator run()
	{
		stopTime = 0;
		ani.ResetTrigger("walk");
		ani.SetTrigger("run");
		ani.SetFloat("movementSpeed", 10);
		move(movementSpeed);

		if (horizontal == 0)
		{
			ani.ResetTrigger("run");
			state = States.idle;
		}
		yield return new WaitForEndOfFrame();
	}

	IEnumerator attack()
	{
		atkFlag = true;
		ani.Rebind();
		ani.speed = 2f;

		while (ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
		{
			ani.SetTrigger("attack");
			yield return new WaitForEndOfFrame();
		}

		ani.ResetTrigger("attack");
		state = States.idle;
		ani.speed = 1;
		atkFlag = false;
	}

	void move(float speed)
	{
		Vector2 newPos = Vector2.zero;
		//Vector3 localScale = transform.localScale;
		Quaternion rot = transform.rotation;
		if (horizontal > 0)
		{
			//localScale.x = Mathf.Abs(localScale.x);
			rot = Quaternion.Euler(new Vector3(0, 0, 0));
			newPos = Vector2.right * Time.deltaTime * speed;
		}
		else if (horizontal < 0)
		{
			//localScale.x = -Mathf.Abs(localScale.x);
			rot = Quaternion.Euler(new Vector3(0, 180, 0));
			newPos = Vector2.left * Time.deltaTime * speed;
		}
		else if (isFixed)
		{
			newPos = (Vector2)transform.right * Time.deltaTime * speed;
		}

		//transform.localScale = localScale;
		transform.rotation = rot;

		//rigidbody2d.MovePosition 는 주어진 벡터로 이동하도록 힘을 가함.
		if (isGround)
		{
			rd2d.MovePosition((Vector2)transform.position + newPos);
		}
		else
		{
			rd2d.AddForce(newPos * 100, ForceMode2D.Impulse);
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.transform.tag == "Ground")
		{
			isGround = true;
		}
	}

	void OnCollisionExit2D(Collision2D col)
	{
		if(col.transform.tag == "Ground")
		{
			isGround = false;
		}
	}

	void jump()
	{
		jumpFlag = true;
		rd2d.AddForce(Vector2.up * jumpPower);
		jumpFlag = false;
	}

	/*
	public void OnDrawGizmos()
	{
		RaycastHit2D hit;
		hit = Physics2D.BoxCast(transform.position + new Vector3(2f,1), new Vector2(1f, 1.5f), 0, transform.TransformDirection(transform.right), 1);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position + new Vector3(2f, 1), transform.position + Vector3.right * 3.5f + Vector3.up);
		if (hit)
			Gizmos.DrawCube(transform.position + new Vector3(2f, 1), new Vector2(3f, 1.5f));
	}
	*/
}
