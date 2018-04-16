using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour {

	Animator ani;
	Rigidbody2D rd2d;
	int horizontal;
	float walkStartTime = 0;

	bool isFixed = false;
	public bool isAttacking = false;

	// Use this for initialization
	void Start () {
		ani = GetComponent<Animator>();
		rd2d = GetComponent<Rigidbody2D>();

		ani.SetFloat("movementSpeed", 1.5f);

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
			if (!isAttacking)
			{
				StartCoroutine("attack");
			}
		}
	}

	IEnumerator idle()
	{
		while(true)
		{
			if(isAttacking)
			{
				break;
			}
			//Debug.Log("idle");
			ani.SetTrigger("idle");
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

		walkStartTime = Time.time;
		while(true)
		{
			ani.SetTrigger("walk");
			ani.SetFloat("movementSpeed", 1.5f);
			if (isAttacking)
			{
				ani.ResetTrigger("walk");
				break;
			}

			if (horizontal == 0)
			{
				StartCoroutine("idle");

				break;
			}

			if(Time.time - walkStartTime > 2 || isFixed || ani.GetFloat("movementSpeed") == 3f)
			{
				StartCoroutine("run");

				break;
			}
			move(ani.GetFloat("movementSpeed"));

			yield return new WaitForEndOfFrame();
		}
		ani.ResetTrigger("walk");
	}

	IEnumerator run()
	{
		while(true)
		{
			ani.SetTrigger("run");
			ani.SetFloat("movementSpeed", 3f);
			if (isAttacking)
			{
				ani.ResetTrigger("run");
				break;
			}
			if (horizontal == 0 && !isFixed)
			{
				ani.SetFloat("movementSpeed", 1.5f);
				StartCoroutine("idle");
				break;
			}
			move(ani.GetFloat("movementSpeed"));

			yield return new WaitForEndOfFrame();
		}
		ani.ResetTrigger("run");
	}

	IEnumerator attack()
	{
		ani.Rebind();
		isAttacking = true;

		while (ani.GetFloat("attackTime") < 1f)
		{
			ani.SetTrigger("attack");
			ani.SetFloat("attackTime", ani.GetCurrentAnimatorStateInfo(0).normalizedTime);
			yield return new WaitForEndOfFrame();
		}

		isAttacking = false;
		yield return new WaitForEndOfFrame();

		ani.ResetTrigger("attack");
		if (!isFixed)
			StartCoroutine("idle");
		else
			StartCoroutine("run");
	}

	void move(float speed)
	{
		if (isAttacking) return;
		//Debug.Log(speed);

		Vector2 newPos = transform.position;
		//Vector3 localScale = transform.localScale;
		Quaternion rot = transform.rotation;
		if(horizontal > 0)
		{
			//localScale.x = Mathf.Abs(localScale.x);
			rot = Quaternion.Euler(new Vector3(0, 0, 0));
			newPos += Vector2.right * Time.deltaTime * speed;
		}
		else if(horizontal < 0)
		{
			//localScale.x = -Mathf.Abs(localScale.x);
			rot = Quaternion.Euler(new Vector3(0, 180, 0));
			newPos += Vector2.left * Time.deltaTime * speed;
		}
		else if(isFixed)
		{
			newPos += (Vector2)transform.right * Time.deltaTime * speed;
		}

		//transform.localScale = localScale;
		transform.rotation = rot;
		rd2d.MovePosition(newPos);
	}


}
