using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFSM : MonoBehaviour {

	/*
	 *	상태들은 enum으로 저장
	 *	상태들의 전환은 코루틴 내에서 직접 전이 조건 검사 후 실행
	 *	예를 들어, idle 상태에서 패턴 대기 시간 후 PAT_A로 넘어갔으면 PAT_A의 처리(애니메이션 처리, 공격 처리)가
	 *	끝난 후에 다시 idle 상태로 복귀.
	 *	다음에 쓸 패턴은 난수로 돌림.
	 */

	public enum States
	{
		IDLE=0,
		PAT_A,
		PAT_B,
		PAT_C,
		PAT_D
	}

	[SerializeField]
	States currentState;

	float normalizedAnimtime;
	float waitTime;
	bool condition;		//temp

	// Use this for initialization
	void Start () {
		//초기화
		currentState = States.IDLE;
		waitTime = 0;
		normalizedAnimtime = 0;

		StartCoroutine(idle());
	}
	
	// Update is called once per frame
	void Update () {
		//actions
		Debug.Log("current : " + currentState);

	}

	IEnumerator idle()
	{
		normalizedAnimtime = 0;
		currentState = States.IDLE;
		//action

		while(waitTime < 1)
		{
			waitTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		States nextPattern = (States)Random.Range(1, 4);
		currentState = nextPattern;
		StartCoroutine(nextPattern.ToString());

		yield return new WaitForEndOfFrame();
	}

	IEnumerator PAT_A()
	{
		waitTime = 0;
		while (normalizedAnimtime < 1)	//애니메이션의 정규화된 플레이 시간이 1이 될때까지 (즉, 완료될때까지)
		{
			normalizedAnimtime += Time.deltaTime;  //계속 정규화된 애니메이션 타임 불러오는 것으로 대체해야함.
			yield return new WaitForEndOfFrame();
		}

		//actions
		currentState = States.IDLE;
		StartCoroutine(idle());

		yield return new WaitForEndOfFrame();
	}

	IEnumerator PAT_B()
	{
		waitTime = 0;
		while (normalizedAnimtime < 1)	//위와 동일
		{
			normalizedAnimtime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		//actions
		currentState = States.IDLE;
		StartCoroutine(idle());

		yield return new WaitForEndOfFrame();
	}

	IEnumerator PAT_C()
	{
		waitTime = 0;
		currentState = States.PAT_C;
		while (normalizedAnimtime < 1)
		{
			normalizedAnimtime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		//actions
		currentState = States.IDLE;
		StartCoroutine(idle());

		yield return new WaitForEndOfFrame();
	}

	States getCurrentState()
	{
		return currentState;
	}

	void setCurrentState(States state)
	{
		currentState = state;
	}
}
