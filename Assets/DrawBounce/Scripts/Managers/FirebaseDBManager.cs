using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using MysticLights;

public class FirebaseDBManager : MonoBehaviour
{
	private DatabaseReference dbReference;

	void Awake()
    {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://draw-bounce-61800636.firebaseio.com/");
		dbReference = FirebaseDatabase.DefaultInstance.RootReference;
	}

	public void SendFirebaseDB(string json, string targetChild)
	{
		dbReference.Child("receipt")
			.Child(GooglePlayManager.Instance.GetUserId())
			.Child(targetChild)
			.SetRawJsonValueAsync(json);
	}

	public void ReceiveFirebaseDB(string productId, Action<bool> callback)
	{
		bool result = false;
		string userId = GooglePlayManager.Instance.GetUserId();
		Debug.LogFormat("Validate : {0}, {1}", productId, userId);

		FirebaseDatabase.DefaultInstance
			.GetReference("receipt")
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

					if (snapshot.HasChild(GooglePlayManager.Instance.GetUserId()))
					{
						if (snapshot.Child(GooglePlayManager.Instance.GetUserId()).HasChild(productId))
						{
							Debug.LogFormat("Has Product : {0}", productId);
							result = true;
						}
						else
						{
							Debug.LogFormat("Not has Product : {0}", productId);
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
}
