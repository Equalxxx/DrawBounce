using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

public enum PlayerColorType { White, Red, Green, Blue }
public class Player : MonoBehaviour
{
    private Rigidbody2D myRigidbody2D;
    private Transform myTransform;

    [Header("PlayerInfo")]
    public int HP;
    public int maxHP = 5;
    public float jumpPower = 6f;

	[Header("PlayerGraphic")]
	public PlayerColorType curColorType;
	public Color currentColor = Color.white;
	public SpriteRenderer sprRenderer;
	public ParticleSystem trailParticle;
	private ParticleSystem.MainModule trailModule;

    [Header("PhysicsInfo")]
    public bool onGrounded;
    public LayerMask groundMask;
    public float checkGroundDist;
    public float limitVelo = 5f;

    public float height;
	public float offsetHeight;
	public float lastHeight;

    public string hitSmallTag;
    public string hitBigTag;

    public static Action DamagedAction;

    private void Awake()
    {
        PoolManager.Instance.PrepareAssets(hitSmallTag);
        PoolManager.Instance.PrepareAssets(hitBigTag);
		trailModule = trailParticle.main;
    }

	public void InitPlayer()
    {
		if (!gameObject.activeSelf)
			gameObject.SetActive(true);

        if (myRigidbody2D == null)
            myRigidbody2D = GetComponent<Rigidbody2D>();

        myRigidbody2D.velocity = Vector2.zero;

        if (myTransform == null)
            myTransform = transform;

        myTransform.position = GameManager.Instance.spawnTrans.position;

        HP = maxHP;
		lastHeight = 0f;

		Debug.Log("Player initialized!");
    }

    private void FixedUpdate()
    {
        Vector2 velo = myRigidbody2D.velocity;
        if (velo.x > limitVelo)
            velo.x = limitVelo;
        if (velo.x < -limitVelo)
            velo.x = -limitVelo;
        if (velo.y > limitVelo)
            velo.y = limitVelo;
        if (velo.y < -limitVelo)
            velo.y = -limitVelo;

        height = myTransform.position.y + offsetHeight;

		if (GameManager.Instance.gameState != GameState.GamePlay)
			return;

		if (lastHeight < myTransform.position.y)
		{
			lastHeight = myTransform.position.y;
		}

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			ChangeColor(PlayerColorType.Red);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			ChangeColor(PlayerColorType.Green);
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			ChangeColor(PlayerColorType.Blue);
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
            PoolManager.Instance.Spawn(hitSmallTag, transform.position, Quaternion.identity);
        }
		else if(collision.collider.CompareTag("EndBlock"))
		{
			PoolManager.Instance.Spawn(hitBigTag, transform.position, Quaternion.identity);

			ShakeCamera.ShakePosOrder();

			Dead();
		}
        else
        {
            Debug.LogFormat("Hit : {0}", collision.collider.name);
            PoolManager.Instance.Spawn(hitBigTag, transform.position, Quaternion.identity);

            ShakeCamera.ShakePosOrder();

            HP -= 1;
            DamagedAction?.Invoke();

            if (HP <= 0)
            {
                Dead();
            }
        }
    }

	void ChangeColor(PlayerColorType colorType)
	{
		if (curColorType == colorType)
			return;

		curColorType = colorType;

		Color playerColor = Color.white;

		switch(colorType)
		{
			case PlayerColorType.Red:
				playerColor = Color.red;
				break;
			case PlayerColorType.Green:
				playerColor = Color.green;
				break;
			case PlayerColorType.Blue:
				playerColor = Color.blue;
				break;
		}

		sprRenderer.color = playerColor;
		playerColor.a = 0.3f;
		trailModule.startColor = playerColor;
	}

    void Dead()
    {
        Debug.Log("Player Dead!");

        GameManager.Instance.SetGameState(GameState.GameOver);

        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
