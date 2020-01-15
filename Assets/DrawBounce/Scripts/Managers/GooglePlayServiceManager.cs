using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

public class GooglePlayServiceManager : MonoBehaviour
{
	public TextMeshProUGUI testText;

    // Start is called before the first frame update
    void Start()
    {
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).Build();
		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.Activate();
	}

    public void CheckLogin()
	{
		if (Social.localUser.authenticated) // GPGS 로그인 되어 있는 경우
		{
			testText.text = string.Format("Success : {0}", Social.localUser.id);
		}
		else // GPGS 로그인이 되어 있지 않은 경우
		{
			Social.localUser.Authenticate((bool Success) =>
			{
				if (Success) //로그인 시도 성공
				{
					testText.text = "Wait...";
				}
				else //로그인 실패
				{
					testText.text = string.Format("Failed : {0}", Social.localUser.id);
				}
			});
		}
	}
}
