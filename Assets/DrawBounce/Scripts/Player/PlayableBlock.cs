using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

public enum PlayableBlockType { Circle, Rectangle, Triangle, Star, Male, Female, Heart }
public class PlayableBlock : MonoBehaviour, IPoolObject
{
    private Rigidbody2D myRigidbody2D;
    private Transform myTransform;

    [Header("PlayerInfo")]
    public int HP;
    public float jumpPower = 6f;
	public float addHeightCoinPer = 100f;

	[Header("PlayerGraphic")]
	public PlayableBlockType blockType;

	public ParticleSystem trailParticle;
	private ParticleSystem.MainModule trailModule;
	public ParticleSystem boostTrail;
	private ParticleSystem.MainModule boostModule;

	[Header("PhysicsInfo")]
	public bool isFastMove;

    public float height;
	public float offsetHeight;
	public float lastHeight;
	public float lastOldHeight;

	public string hitSmallTag;
    public string hitBigTag;
    public string explosionTag;

	public static Action DamagedAction;
	public static Action MoveToAction;

	private void Awake()
    {
        PoolManager.Instance.PrepareAssets(hitSmallTag);
        PoolManager.Instance.PrepareAssets(hitBigTag);
        PoolManager.Instance.PrepareAssets(explosionTag);
		PoolManager.Instance.PrepareAssets("AddCoinEffect");

		//trailModule = trailParticle.main;
		boostModule = boostTrail.main;
		trailModule = trailParticle.main;
    }

	private void OnEnable()
	{
		GameManager.SetStartHeightAction += SetStartHeight;
	}

	private void OnDisable()
	{
		GameManager.SetStartHeightAction -= SetStartHeight;
	}

	public void InitPlayer()
    {
		Show(true);

        if (myRigidbody2D == null)
            myRigidbody2D = GetComponent<Rigidbody2D>();

        myRigidbody2D.velocity = Vector2.zero;

        if (myTransform == null)
            myTransform = transform;

		myTransform.position = Vector3.zero;

		lastHeight = 0f;
		lastOldHeight = 0f;

		Debug.Log("Player initialized!");
    }

	void SetStartHeight(float height)
	{
		if (height < GameManager.Instance.limitStartHeight)
		{
			MoveToAction?.Invoke();
			return;
		}

		Vector3 targetPos = new Vector3(0f, height, 0f);
		lastHeight = height;
		lastOldHeight = height;
		SoundManager.Instance.PlaySound2D("Boost");
		StartCoroutine(MoveToStartHeight(targetPos));
	}

	IEnumerator MoveToStartHeight(Vector3 targetPos)
	{
		Vector3 playerPos = myTransform.position;
		float t = 0f;
		
		isFastMove = true;
		myRigidbody2D.isKinematic = true;
		//boostModule.duration = GameManager.Instance.moveToDuration;
		//boostTrail.Play();

		while (t < 1f)
		{
			t += Mathf.Sin(Time.deltaTime / GameManager.Instance.moveToDuration);

			myTransform.position = Vector3.Lerp(playerPos, targetPos, t);

			yield return null;
		}

		myRigidbody2D.isKinematic = false;
		myRigidbody2D.velocity = Vector2.up * 5f;
		isFastMove = false;

		MoveToAction?.Invoke();
		Debug.Log("Move start height done!");
	}

    private void FixedUpdate()
    {
        height = myTransform.position.y + offsetHeight;
		trailModule.startRotation = -myTransform.localEulerAngles.z * Mathf.Deg2Rad;

		if (isFastMove)
			return;

		if (GameManager.Instance.gameState != GameState.GamePlay)
			return;

		if (lastHeight < myTransform.position.y)
		{
			lastHeight = myTransform.position.y;

			if (lastHeight + offsetHeight > lastOldHeight + GameManager.Instance.getCoinHeight)
			{
				if(GameManager.Instance.IsAddCoin(GameManager.Instance.GetHeightCoinValue()))
				{
					GameManager.Instance.AddCoin(GameManager.Instance.GetHeightCoinValue());
					AddCoinEffect coinEffect = PoolManager.Instance.Spawn("AddCoinEffect", myTransform.position, Quaternion.identity).GetComponent<AddCoinEffect>();
					SoundManager.Instance.PlaySound2D("AddCoin");
					coinEffect.RefreshEffect(GameManager.Instance.GetHeightCoinValue());
				}

				lastOldHeight = GetLastHeight();
			}
		}
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("SemiBlock"))
        {
            Vector2 velo = myRigidbody2D.velocity;
            velo.y = 0f;
            myRigidbody2D.velocity = velo;

            myRigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            PoolManager.Instance.Spawn(hitSmallTag, transform.position, Quaternion.identity);
        }
        else if(collision.collider.CompareTag("Side"))
        {
            PoolManager.Instance.Spawn(hitSmallTag, myTransform.position, Quaternion.identity);
        }
		else if(collision.collider.CompareTag("EndBlock"))
		{
			PoolManager.Instance.Spawn(hitBigTag, myTransform.position, Quaternion.identity);

			ShakeCamera.ShakePosOrder();

			SoundManager.Instance.PlaySound2D("Explosion_Over");
			Dead();
		}
        else
        {
            Debug.LogFormat("Hit : {0}", collision.collider.name);
            PoolManager.Instance.Spawn(hitBigTag, myTransform.position, Quaternion.identity);

            ShakeCamera.ShakePosOrder();

            HP -= 1;
            DamagedAction?.Invoke();

			if(GameManager.Instance.isVibe)
				Handheld.Vibrate();

			if (HP <= 0)
            {
				SoundManager.Instance.PlaySound2D("Explosion_Over");
				Dead();
            }
			else
			{
				SoundManager.Instance.PlaySound2D("Explosion_Hit");
			}
        }

		PlayRandomBounceSound();
	}
	
    void Dead()
    {
        Debug.Log("Player Dead!");

        GameManager.Instance.SetGameState(GameState.GameOver);

		if(GetLastHeight() > GameManager.Instance.gameInfo.lastHeight)
			GameManager.Instance.gameInfo.lastHeight = GetLastHeight();

		PoolManager.Instance.Spawn(explosionTag, myTransform.position, Quaternion.identity);

		if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }

	public float GetLastHeight()
	{
		return lastHeight + offsetHeight;
	}

	public void Show(bool show)
	{
		if (gameObject.activeSelf != show)
			gameObject.SetActive(show);
	}

	void PlayRandomBounceSound()
	{
		int idx = UnityEngine.Random.Range(1, 6);
		string soundTag = string.Format("Bounce_{0}", idx);
		SoundManager.Instance.PlaySound2D(soundTag);
	}

	public void OnSpawnObject()
	{
	}
}
