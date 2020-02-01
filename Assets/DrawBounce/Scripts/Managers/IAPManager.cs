using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using Firebase.Database;
using MysticLights;

public class IAPManager : Singleton<IAPManager>, IStoreListener
{
	public const string ProductCoin_15000 = "coin_15000";
	public const string ProductCoin_80000 = "coin_80000";
	public const string ProductNoAds = "noadspackage";

	//private const string _ios_CoinId = "coin_15000_google";
	private const string _android_CoinId1 = "코인 15000개";
	private const string _android_CoinId2 = "코인 80000개";

	//private const string _ios_SkinId = "com.MysticLights.app.coin";
	//private const string _android_SkinId = "com.MysticLights.app.coin";

	private IStoreController storeController;
	private IExtensionProvider storeExtensionProvider;

	public bool IsInitialized => storeController != null && storeExtensionProvider != null;
	
	public static Action<bool, string> PuchaseCompleteAction;

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
		builder.AddProduct(ProductNoAds, ProductType.NonConsumable, new IDs { { ProductNoAds, GooglePlay.Name } });

		UnityPurchasing.Initialize(this, builder);
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

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
	{
		bool validPurchase = true; // Presume valid for platforms with no R.V.

		// Unity IAP's validation logic is only included on these platforms.
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
		// Prepare the validator with the secrets we prepared in the Editor
		// obfuscation window.
		var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
			AppleTangle.Data(), Application.identifier);

		try
		{
			// On Google Play, result has a single product ID.
			// On Apple stores, receipts contain multiple products.
			var result = validator.Validate(e.purchasedProduct.receipt);
			// For informational purposes, we list the receipt(s)
			Debug.Log("Receipt is valid. Contents:");
			foreach (IPurchaseReceipt productReceipt in result)
			{
				Debug.Log(productReceipt.productID);
				Debug.Log(productReceipt.purchaseDate);
				Debug.Log(productReceipt.transactionID);
			}
		}
		catch (IAPSecurityException)
		{
			Debug.Log("Invalid receipt, not unlocking content");
			validPurchase = false;
		}
#endif

		if (validPurchase)
		{
			// Unlock the appropriate content here.
		}

		PuchaseCompleteAction?.Invoke(true, e.purchasedProduct.definition.id);

		return PurchaseProcessingResult.Complete;
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

	public bool HadPurchased(string productId)
	{
		if (!IsInitialized)
			return false;

		var product = storeController.products.WithID(productId);

		if (product != null)
		{
			return product.hasReceipt;
		}

		return false;
	}
}
