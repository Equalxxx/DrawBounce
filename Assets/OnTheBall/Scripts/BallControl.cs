using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    private Rigidbody2D myRigidbody2D;
    private Transform myTransform;
    public float jumpPower = 6f;
    public Collider2D selectedCollider;

    public bool onGrounded;
    public LayerMask groundMask;
    public float checkGroundDist;

    public float limitVelo = 5f;

    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myTransform = transform;
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.collider.CompareTag("Block"))
        //{
        //    onGrounded = Physics2D.Raycast(transform.position, Vector2.down, checkGroundDist, groundMask);

        //    if (onGrounded)
        //    {
        //        if (selectedCollider != collision.collider)
        //        {
        //            selectedCollider = collision.collider;
        //            myRigidbody2D.velocity = Vector2.zero;
        //            JoyconFramework.ShakeCamera.ShakePosOrder();
        //        }
        //        else
        //        {
        //            Vector2 velo = myRigidbody2D.velocity;
        //            velo.y = 0f;
        //            myRigidbody2D.velocity = velo;
        //        }

        //        myRigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        //    }
        //}
        //else if (collision.collider.CompareTag("SemiBlock"))
        //{
        //    if (selectedCollider != null)
        //        selectedCollider = null;

        //    Vector2 velo = myRigidbody2D.velocity;
        //    velo.y = 0f;
        //    myRigidbody2D.velocity = velo;

        //    myRigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        //}

        if (collision.collider.CompareTag("SemiBlock"))
        {
            Vector2 velo = myRigidbody2D.velocity;
            velo.y = 0f;
            myRigidbody2D.velocity = velo;

            myRigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }
}
