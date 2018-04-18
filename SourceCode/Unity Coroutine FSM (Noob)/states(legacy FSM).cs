using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class states : MonoBehaviour {

	Animator ani;
	Rigidbody2D rd2d;
	KnightStat stat;

	int horizontal;
	float walkStartTime = 0;
	float movementSpeed;

	bool isFixed = false;
	public bool isAttacking = false;

	// Use this for initialization
	void Start () {
		ani = GetComponent<Animator>();
		rd2d = GetComponent<Rigidbody2D>();
		stat = GetComponent<KnightStat>();

		ani.SetFloat("movementSpeed", movementSpeed);
		StartCoroutine("idle");
	}
	
	// Update is called once per frame
	void Update () {
		movementSpeed = stat.getMovementSpeed();
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
				if (movementSpeed >= 3f)
				{
					movementSpeed = 1.5f;
				}
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
			ani.SetFloat("movementSpeed", movementSpeed);
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
		stat.setMovementSpeed(3.0f);
		while(true)
		{
			ani.SetTrigger("run");
			ani.SetFloat("movementSpeed", movementSpeed);
			if (isAttacking)
			{
				ani.ResetTrigger("run");
				break;
			}
			if (horizontal == 0 && !isFixed)
			{
				stat.setMovementSpeed(1.5f);
				ani.SetFloat("movementSpeed", movementSpeed);
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
		bool isRun = ani.GetCurrentAnimatorStateInfo(0).IsName("run");
		ani.Rebind();
		isAttacking = true;
		ani.speed = 2f;

		while (ani.GetFloat("attackTime") < 1f)
		{
			ani.SetTrigger("attack");
			ani.SetFloat("attackTime", ani.GetCurrentAnimatorStateInfo(0).normalizedTime);
			yield return new WaitForEndOfFrame();
		}

		isAttacking = false;
		yield return new WaitForEndOfFrame();

		ani.ResetTrigger("attack");
		if (!isFixed && !isRun)
		{
			stat.setMovementSpeed(1.5f);
			StartCoroutine("idle");
		}
		else
		{
			StartCoroutine("run");
		}
		ani.speed = 1;
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
