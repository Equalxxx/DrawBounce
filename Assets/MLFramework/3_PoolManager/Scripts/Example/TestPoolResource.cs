﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

public class TestPoolResource : MonoBehaviour {
    public ResourceManager.LinkType linkType;

	// Use this for initialization
	void Awake () {
        PoolManager.Instance.resLinkType = linkType;
    }
}