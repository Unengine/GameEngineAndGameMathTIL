using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour {

	Animator ani;
	GameObject arrow;

	float movementSpeed;
	float pull;
	float release;

	bool dieFlag;
	bool isMovable;
	bool isMoving;
	bool isCool;
	Rigidbody2D rd2d;

	// Use this for initialization
	void Start () {
		ani = GetComponent<Animator>();
		rd2d = GetComponent<Rigidbody2D>();
		arrow = Resources.Load("arrow") as GameObject;

		ani.speed = 2;
		movementSpeed = 4;

		isMovable = true;
		isMoving = false;
		isCool = false;

		StartCoroutine("idle");
	}
	
	IEnumerator move()
	{
		isMoving = true;
		while (isMoving && isMovable)
		{
			Vector3 direction;
			float delta;

			if (Input.GetKey(KeyCode.D))
			{
				delta = Time.deltaTime * movementSpeed;
				direction = new Vector3(transform.position.x + delta, transform.position.y, transform.position.z);
				rd2d.MovePosition(direction);
			}
			else if (Input.GetKey(KeyCode.A))
			{
				delta = -Time.deltaTime * movementSpeed;
				direction = new Vector3(transform.position.x + delta, transform.position.y, transform.position.z);
				rd2d.MovePosition(direction);
			}
			else
			{
				isMoving = false;
				StartCoroutine("idle");
				yield return null;
			}
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator die()
	{
		isMovable = false;
		yield return null;
	}

	IEnumerator idle()
	{
		while (!isMoving)
		{
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
			{
				StartCoroutine("move");
				yield return null;
			}
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator attack()
	{
		isCool = true;
		isMovable = false;
		isMoving = false;
		pull = 0;
		ani.SetBool("attack", true);
		ani.speed = 0.1f;

		while(Input.GetMouseButton(0))
		{
			++pull;

			if (pull >= 250)
				break;

			yield return new WaitForEndOfFrame();
		}

		float power = pull;

		if(power <= 40)
		{
			power = 40;
		}

		if (power >= 180)
		{
			power = 180;
		}

		ani.speed = 4;

		arrowPower tmp = Instantiate(arrow).GetComponent<arrowPower>();
		tmp.transform.position = transform.position;

		tmp.setStatus(0, power * 40);

		yield return new WaitForSeconds(0.5f);
		isMovable = true;
		isMoving = false;
		StartCoroutine("idle");
		ani.SetBool("attack", false);
		ani.speed = 2;

		yield return new WaitForSeconds(2);
		isCool = false;
	}

	// Update is called once per frame
	void Update () {
		setAnims();

		if(Input.GetMouseButtonDown(0) && !isCool)
		{
			StartCoroutine("attack");
		}
	}

	void setAnims()
	{
		ani.SetBool("isMoving", isMoving);
	}
}