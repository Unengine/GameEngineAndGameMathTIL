using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {

	FSM player;
	Animator ani;
	CameraEffect cam;

	bool isAttacking;
	bool atkFlag = true;

	// Use this for initialization
	void Start () {
		player = GetComponent<FSM>();
		ani = GetComponent<Animator>();
		cam = Camera.main.gameObject.GetComponent<CameraEffect>();
	}
	
	// Update is called once per frame
	void Update () {
		isAttacking = player.isAttacking;
		if(isAttacking && atkFlag)
		{
			atkFlag = false;
			attack();
		}
	}

	void attack()
	{
		float distance = 1f;
		RaycastHit2D[] hit;

		Vector3 front;
		if (transform.rotation.y == 0)
		{
			front = transform.position + new Vector3(1.5f, 1);
			distance = 1f;
		}
		else
		{
			front = transform.position + new Vector3(-1.5f, 1);
			distance = -1f;
		}

		hit = Physics2D.BoxCastAll(front, new Vector2(1.25f, 1.25f), 0, transform.TransformDirection(transform.right), distance);
		//boxcast 모든 범위 내 게임오브젝트 얻음

		StartCoroutine("deal", hit);
	}

	IEnumerator deal(RaycastHit2D[] targets)
	{
		while(ani.GetFloat("attackTime") <= 0.5f)
		{
			yield return new WaitForEndOfFrame();
		}

		if (targets.Length > 0)
		{
			cam.shakeCamera(0.4f, 0.5f);
			foreach (RaycastHit2D rayhit in targets)
			{
				if (rayhit.transform.tag != "Enemy") continue;
				Debug.Log(rayhit.transform);
				rayhit.transform.GetComponent<Rigidbody2D>().AddForce(transform.right * 200);
			}
		}
		else
		{
			Debug.Log("miss!");
		}

		while (ani.GetFloat("attackTime") < 1)
		{
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForEndOfFrame();
		atkFlag = true;
	}

}
