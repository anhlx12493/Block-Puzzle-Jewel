using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdAdmob : AD
{
    private BannerView bannerView;
    private RewardedAd rewardedAd;
    public UnityEvent OnAdLoadedEvent = new UnityEvent();
    public UnityEvent OnAdFailedToLoadEvent = new UnityEvent();
    public UnityEvent OnAdOpeningEvent = new UnityEvent();
    public UnityEvent OnAdFailedToShowEvent = new UnityEvent();
    public UnityEvent OnUserEarnedRewardEvent = new UnityEvent();
    public UnityEvent OnAdClosedEvent = new UnityEvent();
    private InterstitialAd interstitialAd;

    public AdAdmob()
    {
        MobileAds.SetiOSAppPauseOnBackground(true);
        MobileAds.Initialize(HandleInitCompleteAction);
    }
    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // main thread.
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            LoadBanner();
        });
    }
    public override void LoadBanner()
    {
        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

        // Add Event Handlers
        bannerView.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
        bannerView.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
        bannerView.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
        bannerView.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();

        // Load a banner ad
        bannerView.LoadAd(CreateAdRequest());
    }
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddKeyword("unity-admob-sample")
            .Build();
    }

    public override void LoadInter(Action onLoaded, Action onFailToLoad)
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
        interstitialAd = new InterstitialAd(adUnitId);

        // Add Event Handlers
        interstitialAd.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
        interstitialAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
        interstitialAd.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
        interstitialAd.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();


        OnAdLoadedEvent.RemoveAllListeners();
        UnityAction unityActionOnLoad = () =>
        {
            onLoaded?.Invoke();
        };
        OnAdLoadedEvent.AddListener(unityActionOnLoad);

        OnAdFailedToLoadEvent.RemoveAllListeners();
        UnityAction unityActionOnFailToLoad = () =>
        {
            onFailToLoad?.Invoke();
        };
        OnAdFailedToLoadEvent.AddListener(unityActionOnFailToLoad);

        // Load an interstitial ad
        interstitialAd.LoadAd(CreateAdRequest());
    }

    public override bool IsInterReady()
    {
        return interstitialAd.IsLoaded();
    }

    public override bool IsVideoReady()
    {
        return rewardedAd.IsLoaded();
    }


    public override void LoadVideo(Action onLoaded, Action onFailToLoad)
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        string adUnitId = "unexpected_platform";
#endif

        // create new rewarded ad instance
        rewardedAd = new RewardedAd(adUnitId);

        // Add Event Handlers
        rewardedAd.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
        rewardedAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
        rewardedAd.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
        rewardedAd.OnAdFailedToShow += (sender, args) => OnAdFailedToShowEvent.Invoke();
        rewardedAd.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();
        rewardedAd.OnUserEarnedReward += (sender, args) => OnUserEarnedRewardEvent.Invoke();


        OnAdLoadedEvent.RemoveAllListeners();
        UnityAction unityActionOnLoad = () =>
        {
            onLoaded?.Invoke();
        };
        OnAdLoadedEvent.AddListener(unityActionOnLoad);

        OnAdFailedToLoadEvent.RemoveAllListeners();
        UnityAction unityActionOnFailToLoad = () =>
        {
            onFailToLoad?.Invoke();
        };
        OnAdFailedToLoadEvent.AddListener(unityActionOnFailToLoad);

        // Create empty ad request
        rewardedAd.LoadAd(CreateAdRequest());
    }

    public override void ShowInter(Action onClose)
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
            OnAdClosedEvent.RemoveAllListeners();
            UnityAction unityAction = () =>
            {
                onClose?.Invoke();
            };
            OnAdClosedEvent.AddListener(unityAction);
        }
        else
        {
            onClose?.Invoke();
        }
    }

    public override void ShowVideo(Action onRewarded, Action onDidNotReward, Action onClose)
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
            OnUserEarnedRewardEvent.RemoveAllListeners();
            bool isRewarded = false;
            UnityAction unityActionOnRewarded = () =>
            {
                isRewarded = true;
                onRewarded?.Invoke();
            };
            OnUserEarnedRewardEvent.AddListener(unityActionOnRewarded);
            OnAdClosedEvent.RemoveAllListeners();
            UnityAction unityActionOnClose = () =>
            {

                if (!isRewarded)
                {
                    onDidNotReward?.Invoke();
                }
                onClose?.Invoke();
            };
            OnAdClosedEvent.AddListener(unityActionOnClose);
        }
        else
        {
            onClose?.Invoke();
        }
    }
    public override void DestroyBanner()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }
    public override void DestroyInter()
    {

    }
}
