using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using MysticLights;

public class IAPManager : Singleton<IAPManager>, IStoreListener
{
	public const string ProductCoin_15000 = "coin_15000";
	public const string ProductCoin_160000 = "coin_160000";
	public const string ProductSkin = "skin";

	//private const string _ios_CoinId = "coin_15000_google";
	private const string _android_CoinId1 = "코인 15000개";
	private const string _android_CoinId2 = "코인 160000개";

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

		builder.AddProduct(ProductCoin_15000, ProductType.Consumable);
		builder.AddProduct(ProductCoin_160000, ProductType.Consumable);

		//builder.AddProduct(
		//	ProductCoin_15000, ProductType.Consumable, new IDs() {
		//		{ _android_CoinId1, GooglePlay.Name}
		//	}
		//);

		//builder.AddProduct(
		//	ProductCoin_160000, ProductType.Consumable, new IDs() {
		//		{ _android_CoinId2, GooglePlay.Name}
		//	}
		//);

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
		Debug.LogFormat("Purchase success : {0}", e.purchasedProduct.definition.id);

		if(e.purchasedProduct.definition.id == ProductCoin_15000)
		{
			Debug.LogFormat("Increase coin : {0}", 15000);
		}
		else if (e.purchasedProduct.definition.id == ProductCoin_160000)
		{
			Debug.LogFormat("Increase coin : {0}", 160000);
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
