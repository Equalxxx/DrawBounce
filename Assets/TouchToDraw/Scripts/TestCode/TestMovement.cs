﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
	public float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		transform.Translate(Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed);
    }
}
