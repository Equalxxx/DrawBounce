using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemaEffect : MonoBehaviour
{
	public Sprite[] sprites;
	public Material myMaterial;

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			RefreshUI();
		}
	}

	void RefreshUI()
	{
		ChangeTexture();
	}
	
	void ChangeTexture()
	{
		PlayableBlockType blockType = GameManager.Instance.player.blockType;

		switch (blockType)
		{
			case PlayableBlockType.Circle:
				myMaterial.mainTexture = sprites[0].texture;
				break;
			case PlayableBlockType.Rectangle:
				myMaterial.mainTexture = sprites[1].texture;
				break;
			case PlayableBlockType.Triangle:
				myMaterial.mainTexture = sprites[2].texture;
				break;
			case PlayableBlockType.Star:
				myMaterial.mainTexture = sprites[3].texture;
				break;
			case PlayableBlockType.Male:
				myMaterial.mainTexture = sprites[4].texture;
				break;
			case PlayableBlockType.Female:
				myMaterial.mainTexture = sprites[5].texture;
				break;
		}
	}
}
