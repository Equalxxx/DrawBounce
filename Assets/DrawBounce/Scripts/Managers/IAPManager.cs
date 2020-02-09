using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Analytics;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using MLFramework;

public class IAPManager : Singleton<IAPManager>, IStoreListener
{
	public class Receipt
	{
		public string index;
		public string uid;
		public string email;
		public string transaction_id;
		public string product_id;
		public string platform;
		public string price;
		public string date;
	}

	public const string ProductCoin_15000 = "coin_15000";
	public const string ProductCoin_80000 = "coin_80000";
	public const string ProductNoAds = "noadspackage";
	
	private const string _android_CoinId1 = "코인 15000개";
	private const string _android_CoinId2 = "코인 80000개";

	private IStoreController storeController;
	private IExtensionProvider storeExtensionProvider;

	public bool IsInitialized => storeController != null && storeExtensionProvider != null;

	public static Action<bool, string> PuchaseCompleteAction;

	private DatabaseReference dbReference;

	private void Awake()
	{
		if(Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		if(storeController == null)
			InitUnityIAP();
	}

	void InitUnityIAP()
	{
		if (IsInitialized)
			return;

		Debug.Log("Start initialized IAP");

		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

		builder.AddProduct(ProductCoin_15000, ProductType.Consumable, new IDs { { ProductCoin_15000, GooglePlay.Name } });
		builder.AddProduct(ProductCoin_80000, ProductType.Consumable, new IDs { { ProductCoin_80000, GooglePlay.Name } });
		builder.AddProduct(ProductNoAds, ProductType.Consumable, new IDs { { ProductNoAds, GooglePlay.Name } });

		UnityPurchasing.Initialize(this, builder);

		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://draw-bounce-61800636.firebaseio.com/");
		dbReference = FirebaseDatabase.DefaultInstance.RootReference;
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		Debug.Log("IAP Initialized success");
		storeController = controller;
		storeExtensionProvider = extensions;
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.LogWarningFormat("IAP Initialized failed : {0}", error);
	}

	public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
	{
		Debug.LogWarningFormat("Purchase failed : {0}, {1}", i.definition.id, p);
		PuchaseCompleteAction?.Invoke(false, i.definition.id);
	}

	public void Purchase(string productId)
	{
		if (!IsInitialized)
		{
			Debug.LogWarning("Not Initialized IAP");
			return;
		}

		var product = storeController.products.WithID(productId);
		
		if(product != null && product.availableToPurchase)
		{
			Debug.LogFormat("Try purchase : {0}", product.definition.id);
			storeController.InitiatePurchase(product);
		}
		else
		{
			Debug.LogFormat("Try purchase failed : {0}", productId);
		}
	}

	public void RestorePurchase()
	{
		if (!IsInitialized)
			return;

		if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
		{
			Debug.Log("Restore purchase");

			var appleExt = storeExtensionProvider.GetExtension<IAppleExtensions>();

			appleExt.RestoreTransactions(
				result => Debug.LogFormat("Result restore purchase : {0}", result)
				);
		}
	}

	public void ProductValidate(string productId, Action<bool> callback)
	{
		if (!IsInitialized)
			return;

		//string userId = GooglePlayManager.Instance.GetUserId();
		string userId = Social.localUser.id;
		Debug.LogFormat("Validate : {0}, {1}", productId, userId);

		FirebaseDBManager.Instance.CheckFirebaseDB("receipt", userId, productId, callback);
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
	{
		bool isSuccess = true;

		CrossPlatformValidator validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);

		try
		{
			// 클라이언트 영수증 검산
			IPurchaseReceipt[] result = validator.Validate(e.purchasedProduct.receipt);

			// 클라이언트 검산 후 클라이언트 검산 데이터를 등록한다.
			foreach (var productReceipt in result)
			{
				// 영수증을 서버에 저장한다.

				string json = JsonUtility.ToJson(CreateReceipt(productReceipt, e.purchasedProduct.metadata.localizedPrice.ToString()));
				string transactionid = productReceipt.transactionID.Replace('.', '-');
				string path = string.Format("{0}/{1}", productReceipt.productID, transactionid);
				string userId = Social.localUser.id;
				FirebaseDBManager.Instance.SendFirebaseDB("receipt", userId, path, json);

				// OS 별로 특정 세부 사항이 있다
				//var google = productReceipt as GooglePlayReceipt;
				//var apple = productReceipt as AppleInAppPurchaseReceipt;

			}
		}
		catch (IAPSecurityException)
		{
			isSuccess = false;
		}

		PuchaseCompleteAction?.Invoke(isSuccess, e.purchasedProduct.definition.id);

		// 결제 완료
		return PurchaseProcessingResult.Complete;
	}

	// 서버에 보낼 영수증 데이터를 가공
	public Receipt CreateReceipt(IPurchaseReceipt rec, string price)
	{
		var userReceipt = new Receipt();

		// firebase uid를 입력해주거나 각 유저의 고유 ID를 입력한다.
		//userReceipt.uid = GooglePlayManager.Instance.GetUserId();
		userReceipt.uid = Social.localUser.id;
		userReceipt.email = GooglePlayManager.Instance.GetUserEmail();
		userReceipt.transaction_id = rec.transactionID;
		userReceipt.product_id = rec.productID;
		userReceipt.platform = SystemInfo.operatingSystem;
		userReceipt.price = price;
		userReceipt.date = rec.purchaseDate.ToString("yyyy/MM/dd HH:mm:ss");

		return userReceipt;
	}

	public Product GetProductData(string productId)
	{
		return storeController.products.WithID(productId);
	}
}
