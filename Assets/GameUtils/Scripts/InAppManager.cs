using UnityEngine;
using UnityEngine.Events;

#if IN_APP_PURCHASING
using UnityEngine.Purchasing;
#endif

namespace YsoCorp {

    namespace GameUtils {

        [DefaultExecutionOrder(-10)]
        public class InAppManager : BaseManager
#if IN_APP_PURCHASING
            , IStoreListener
#endif
            {

            public class UnityEventInAppBuy : UnityEvent<string> { }

            public UnityEventInAppBuy onPurchased { get; set; } = new UnityEventInAppBuy();

#if IN_APP_PURCHASING
            private IStoreController _StoreController = null;
            private IExtensionProvider _StoreExtensionProvider = null;
#endif
            protected override void Awake() {
                base.Awake();
            }

#if IN_APP_PURCHASING
            void Start() {
                this.Init();
            }

            public void Init() {
                if (this.IsInitialized()) {
                    return;
                }
                var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
                builder.AddProduct(this.ycManager.ycConfig.InAppRemoveAds, ProductType.NonConsumable);
                foreach (string key in this.ycManager.ycConfig.InAppConsumables) {
                    builder.AddProduct(key, ProductType.Consumable);
                }
                UnityPurchasing.Initialize(this, builder);
            }

            private bool IsInitialized() {
                return this._StoreController != null && this._StoreExtensionProvider != null;
            }

            public Product GetProductById(string productId) {
                if (this._StoreController != null && this._StoreController.products != null) {
                    return this._StoreController.products.WithID(productId);
                }
                return null;
            }
#endif
            public string GetProductPrice(string productId) {
#if IN_APP_PURCHASING
                Product p = this.GetProductById(productId);
                if (p != null) {
                    return p.metadata.localizedPriceString;
                }
#endif
                return "";
            }

            public void BuyProductID(string productId) {
#if IN_APP_PURCHASING
                if (this.IsInitialized()) {
                    Product product = this.GetProductById(productId);
                    if (product != null && product.availableToPurchase) {
                        Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                        this._StoreController.InitiatePurchase(product);
                    } else {
                        Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                    }
                } else {
                    Debug.Log("BuyProductID FAIL. Not initialized.");
                }
#endif
            }

            public void RestorePurchases() {
#if IN_APP_PURCHASING
                if (!this.IsInitialized()) {
                    Debug.Log("RestorePurchases FAIL. Not initialized.");
                    return;
                }

                if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer) {
                    Debug.Log("RestorePurchases started ...");
                    var apple = this._StoreExtensionProvider.GetExtension<IAppleExtensions>();
                    apple.RestoreTransactions((result) => {
                        Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                    });
                } else {
                    Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
                }
#endif
            }

#if IN_APP_PURCHASING
            public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
                Debug.Log("OnInitialized: PASS");
                this._StoreController = controller;
                this._StoreExtensionProvider = extensions;
            }

            public void OnInitializeFailed(InitializationFailureReason error) {
                Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
            }

            public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
                string productId = args.purchasedProduct.definition.id;
                Product p = this.GetProductById(productId);
                if (p != null) {
                    float price = (float)p.metadata.localizedPrice;
                    string isoCurrencyCode = p.metadata.isoCurrencyCode;
                    this.ycManager.analyticsManager.InAppBought(productId, price, isoCurrencyCode);
                }
                this.onPurchased.Invoke(productId);
                return PurchaseProcessingResult.Complete;
            }

            public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
                Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
            }
#endif

            public void BuyProductIDAdsRemove() {
                this.BuyProductID(this.ycManager.ycConfig.InAppRemoveAds);
            }
        }
    }
}