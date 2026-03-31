using System;
using GoogleMobileAds.Api;
using Larje.Core;
using Larje.Core.Services;
using UnityEngine;

[BindService(typeof(IAdsService))]
public class AdmobAdsService : Service, IAdsService
{
    [Header("System")]
    [SerializeField, Min(0f)] private float delayBetweenAds = 20f;

    [Header("Keys")] 
    [SerializeField] private Keys androidKeys;
    [SerializeField] private Keys iosKeys;

    private bool _initialized;

    private Keys _keys;
    private AdmobAdsBanner _banner;

    public bool Initialized => _initialized;
    public bool InterstitialAdAvailable => false;
    public bool RewardedAdAvailable => false;
    public bool BannerShowing => false;
    public float BannerHeight => 0f;

    public event Action EventBannerShown;
    public event Action EventBannerHidden;

    public override void Init()
    {
        if (!_initialized)
        {
            _initialized = true;
            MobileAds.Initialize(OnAdmobInitialized);
        }
    }

    public bool ShowAppOpenAd()
    {
        return true;
    }

    public void ShowInterstitial(int interIndex = 0)
    {
    }

    public void ShowRewarded(Action onAdShowStart, Action onAdShowClick, Action onAdShowComplete, Action onAdShowFailed)
    {
    }

    private void OnAdmobInitialized(GoogleMobileAds.Api.InitializationStatus initStatus)
    {
        Debug.Log("Admob Ads Service Initialized");

#if UNITY_ANDROID
        _keys = androidKeys;
#elif UNITY_IOS
        _keys = iosKeys;
#else
        Debug.LogWarning("Admob Ads Service is not supported on this platform.");
#endif

        _banner = new AdmobAdsBanner(_keys.AdaptiveBanner);
        _banner.ShowBanner();
    }

    [Serializable]
    private class Keys
    {
        [field: SerializeField] public string AdaptiveBanner = "ca-app-pub-3940256099942544/9214589741";
        [field: SerializeField] public string FixedSizeBanner = "ca-app-pub-3940256099942544/6300978111";
        [field: Space]
        [field: SerializeField] public string Interstitial = "ca-app-pub-3940256099942544/1033173712";
        [field: SerializeField] public string RewardedAds = "ca-app-pub-3940256099942544/5224354917";
        [field: Space]
        [field: SerializeField] public string AppOpen = "ca-app-pub-3940256099942544/9257395921";
        [field: SerializeField] public string RewardedInterstitial = "ca-app-pub-3940256099942544/5354046379";
        [field: Space]
        [field: SerializeField] public string Native = "ca-app-pub-3940256099942544/2247696110";
        [field: SerializeField] public string NativeVideo = "ca-app-pub-3940256099942544/1044960115";
    }
}
