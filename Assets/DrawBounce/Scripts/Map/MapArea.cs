using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
	public SpriteRenderer sprRender;

	private void OnValidate()
	{
		if (sprRender == null)
			sprRender = GetComponent<SpriteRenderer>();

		transform.position = new Vector2(0f, sprRender.size.y / 2f);
	}

	void Start()
    {
        if(!GameManager.Instance.testMode)
		{
			Destroy(gameObject);
		}
    }
}
