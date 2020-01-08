using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoyconFramework;

public class Player : MonoBehaviour
{
    private Rigidbody2D myRigidbody2D;
    private Transform myTransform;

    [Header("PlayerInfo")]
    public int HP;
    public int maxHP = 5;
    public float jumpPower = 6f;

    [Header("PhysicsInfo")]
    public bool onGrounded;
    public LayerMask groundMask;
    public float checkGroundDist;
    public float limitVelo = 5f;

    public float height;
	public float offsetHeight;

    public string hitSmallTag;
    public string hitBigTag;

    public static Action DamagedAction;

    private void Awake()
    {
        PoolManager.Instance.PrepareAssets(hitSmallTag);
        PoolManager.Instance.PrepareAssets(hitBigTag);

        InitPlayer();
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

    void Dead()
    {
        Debug.Log("Player Dead!");

        GameManager.Instance.SetGameState(GameState.GameOver);

        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
