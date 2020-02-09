using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Reflection;

public class TestDBLoad : MonoBehaviour
{
	private DatabaseReference dbReference;
	public GameInfo gameInfo;

	// Start is called before the first frame update
	void Start()
    {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://draw-bounce-61800636.firebaseio.com/");
		dbReference = FirebaseDatabase.DefaultInstance.RootReference;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			ReceiveFirebaseDB("gamedata", "", LoadCompleteCallback);
	}

	void LoadCompleteCallback(object loadData)
	{
		Dictionary<string, object> dataDic = loadData as Dictionary<string, object>;

		gameInfo.coin = int.Parse(dataDic["coin"].ToString());
		gameInfo.lastHeight = float.Parse(dataDic["lastHeight"].ToString());
		gameInfo.playerHP = int.Parse(dataDic["playerHP"].ToString());
		gameInfo.playerMaxHP = int.Parse(dataDic["playerMaxHP"].ToString());
		gameInfo.startHeight = float.Parse(dataDic["startHeight"].ToString());
	}

	public void ReceiveFirebaseDB(string targetHead, string path, Action<object> callback)
	{
		Debug.Log("Receive DB");

		string userId = "r7ymUpxJjGcntPkkpD0uPrfz9ud2";
		object data = null;

		Debug.Log(FirebaseDatabase.DefaultInstance
			.GetReference(targetHead).Key);

		FirebaseDatabase.DefaultInstance
			.GetReference(targetHead)
			.GetValueAsync().ContinueWith(task =>
			{
				Debug.Log("Receive task");

				if (task.IsFaulted)
				{
					Debug.LogErrorFormat("DB GetValueAsync encountered an error: {0}", task.Exception);
				}
				else if (task.IsCanceled)
				{
					Debug.LogError("DB GetValueAsync was canceled");
				}
				else if (task.IsCompleted)
				{
					Debug.Log("Task Completed");

					DataSnapshot snapshot = task.Result;

					if (snapshot.HasChild(userId))
					{
						Debug.LogFormat("Has UserId : {0}", userId);

						if (string.IsNullOrEmpty(path))
						{
							data = snapshot.Child(userId).Value;
							Debug.LogFormat("Get DB Data : {0}", userId);
						}
						else
						{
							if (snapshot.Child(userId).HasChild(path))
							{
								data = snapshot.Child(userId).Child(path).Value;
							}
							else
							{
								Debug.LogFormat("Not has Data : {0}", path);
							}
						}
					}
					else
					{
						Debug.LogFormat("Not has UserId : {0}", userId);
					}
				}

				callback?.Invoke(data);
			});
	}
}
