using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLFramework;

public class TestSound : MonoBehaviour
{
	public bool bounceSound;
	public bool testSound;
	public string soundTag;
	public AudioSource audio;

	private IEnumerator Start()
	{
		while(true)
		{
			if(testSound)
			{
				yield return new WaitForSeconds(1.5f);
				if(bounceSound)
				{
					int rnd = Random.Range(1, 5);
					SoundManager.Instance.PlaySound2D(string.Format("Bounce_{0}", rnd));
				}
				else
				{
					SoundManager.Instance.PlaySound2D(soundTag);
				}
			}
			else
			{
				if (Input.GetKeyDown(KeyCode.Space))
					audio.Play();

				yield return null;
			}
		}
	}
}
