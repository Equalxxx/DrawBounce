using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using MysticLights;

public class FirebaseDBManager : Singleton<FirebaseDBManager>
{
	private DatabaseReference dbReference;

	void Awake()
    {
		if(Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://draw-bounce-61800636.firebaseio.com/");
		dbReference = FirebaseDatabase.DefaultInstance.RootReference;
	}

	public void SendFirebaseDB(string targetHead, string userId, string path, string json)
	{
		dbReference.Child(targetHead)
			.Child(userId)
			.Child(path)
			.SetRawJsonValueAsync(json);
	}

	public void CheckFirebaseDB(string targetHead, string userId, string path, Action<bool> callback)
	{
		bool result = false;
		//string userId = GooglePlayManager.Instance.GetUserId();

		FirebaseDatabase.DefaultInstance
			.GetReference(targetHead)
			.GetValueAsync().ContinueWith(task =>
			{
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
					DataSnapshot snapshot = task.Result;

					if (snapshot.HasChild(userId))
					{
						if (snapshot.Child(userId).HasChild(path))
						{
							Debug.LogFormat("Has Data : {0}", path);
							result = true;
						}
						else
						{
							Debug.LogFormat("Not has Data : {0}", path);
						}
					}
					else
					{
						Debug.LogFormat("Not has UserId : {0}", userId);
					}
				}

				callback?.Invoke(result);
			});
	}

	public void ReceiveFirebaseDB(string targetHead, string path, Action<object> callback)
	{
		Debug.Log("Receive DB");

		string userId = GooglePlayManager.Instance.GetUserId();
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

						Debug.Log(snapshot.Child(userId).Child("coin").Value);
						Debug.Log(snapshot.Child(userId).Child("lastHeight").Value);

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
