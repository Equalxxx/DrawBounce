using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform myTransform;
	private Vector3 originPosition;
    private Transform targetTrans;

	public float smoothFactor = 10f;
	public float limitWidth = 1f;

	public float offsetHeight = 5f;

	private bool isFollow;
	private PlayableBlock player;

	public Transform endEffectTrans;

    private void Awake()
    {
        myTransform = transform;
		originPosition = myTransform.position;
    }

	private void OnEnable()
	{
		GameManager.GameInitAction += Init;
		GameManager.GamePlayAction += StartFollow;
		GameManager.SetStartHeightAction += SetStartHeight;
	}

	private void OnDisable()
	{
		GameManager.GameInitAction -= Init;
		GameManager.GamePlayAction -= StartFollow;
		GameManager.SetStartHeightAction -= SetStartHeight;
	}

	void Init()
	{
		player = GameManager.Instance.player;
		targetTrans = player.transform;

		isFollow = false;
		myTransform.position = originPosition;
		endEffectTrans.position = new Vector3(0f, -5f, 10f);
	}

	void SetStartHeight(float height)
	{
		if (height < GameManager.Instance.limitStartHeight)
			return;

		Vector3 targetPos = new Vector3(0f, height, 0f);
		StartCoroutine(MoveToStartHeight(targetPos));
	}

	IEnumerator MoveToStartHeight(Vector3 targetPos)
	{
		isFollow = false;
		float t = 0f;
		Vector3 campPos = myTransform.position;

		while (t < 1f)
		{
			t += Time.deltaTime / GameManager.Instance.moveToDuration;
			myTransform.position = Vector3.Lerp(campPos, targetPos, t);

			yield return null;
		}

		isFollow = true;
		Debug.Log("Move start height done! : camera");
	}

	void StartFollow()
	{
		if (!isFollow)
			isFollow = true;
	}

	private void FixedUpdate()
    {
		if (!isFollow)
			return;
		if (myTransform.position.y < 0f)
			return;

		Vector3 endPos = new Vector3(0f, player.lastHeight - offsetHeight - Camera.main.orthographicSize, 10f);
		if (endPos.y < -Camera.main.orthographicSize)
			endPos.y = -Camera.main.orthographicSize;

		endEffectTrans.position = endPos;

		Debug.DrawLine(new Vector3(-3f, player.lastHeight - offsetHeight - Camera.main.orthographicSize, 0f), new Vector3(3f, player.lastHeight - offsetHeight - Camera.main.orthographicSize, 0f), Color.red);

		Vector3 playerPos = targetTrans.position;

		if (playerPos.y < player.lastHeight - offsetHeight)
		{
			playerPos.y = player.lastHeight - offsetHeight;
		}

		Vector3 targetPos = Vector3.Lerp(myTransform.position, playerPos, Time.fixedDeltaTime * smoothFactor);

		if (targetPos.x > limitWidth)
			targetPos.x = limitWidth;
		if (targetPos.x < -limitWidth)
			targetPos.x = -limitWidth;

		targetPos.z = 0f;

		if (targetPos.y < 0f)
			targetPos.y = 0f;

		myTransform.position = targetPos;

	}
}
