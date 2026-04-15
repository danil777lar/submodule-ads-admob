using System;
using DG.Tweening;
using GoogleMobileAds.Api;
using Larje.Core;
using Larje.Core.Services;
using UnityEngine;

[BindService(typeof(IAdsService))]
public class AdmobAdsService : Service, IAdsService
{
    [Header("Interstitial")]
    [SerializeField, Min(0f)] private float delayBetweenAds = 20f;

    [Header("App Open")]
    [SerializeField] private bool showAppOpenOnStart;
    [SerializeField] private bool showAppOpenOnResume;

    [Header("Keys")] 
    [SerializeField] private Keys androidKeys;
    [SerializeField] private Keys iosKeys;

    [Header("Editor")]
    [SerializeField] private bool disableAdsInEditor;

    [InjectService] private IDataService _dataService;

    private bool _initialized;
    private bool _delayed;
    private bool _appPaused;

    private Keys _keys;

    private AdmobAdsBanner _banner;
    private AdmobAdsInterstitial _interstitial;
    private AdmobAdsRewarded _rewarded;
    private AdmobAdsAppOpen _appOpen;

    public bool Initialized => _initialized;
    public bool InterstitialAdAvailable => _interstitial != null && _interstitial.CanShowInterstitial();
    public bool RewardedAdAvailable => _rewarded != null && _rewarded.CanShowRewarded();
    public bool BannerShowing => _banner != null && _banner.IsShowing;
    public float BannerHeight => _banner != null ? _banner.GetBannerHeight() : 0f;

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
        if (_dataService.GameData.NoAdsActive)
        {
            return false;
        }

        return _appOpen != null && _appOpen.ShowAppOpen();
    }

    public void ShowInterstitial(int interIndex = 0)
    {
        if (_dataService.GameData.NoAdsActive || _delayed)
        {
            return;
        }

        _interstitial?.ShowInterstitial();
        DelayAds();
    }

    public void SetActiveNoAdsMode(bool noAdsActive)
    {
        _dataService.GameData.NoAdsActive = noAdsActive;
        _dataService.SaveGameData();

        if (noAdsActive)
        {
            _banner?.HideBanner();
        }
        else
        {
            _banner?.ShowBanner();
        }
    }

    public void ShowRewarded(Action onAdShowStart, Action onAdShowClick, Action onAdShowComplete, Action onAdShowFailed)
    {
        Action onComplete = () =>
        {
            onAdShowComplete?.Invoke();
            DelayAds();
        };

        Action onFailed = () =>
        {
            onAdShowFailed?.Invoke();
        };

        _rewarded?.ShowRewarded(onComplete, onFailed);
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


        bool adsDisabled = false;

#if UNITY_EDITOR
        if (disableAdsInEditor)
        {
            adsDisabled = true;
            Debug.Log("Admob Ads are disabled in the editor.");
        }
#endif

        if (!adsDisabled)
        {
            _banner = new AdmobAdsBanner(_keys.AdaptiveBanner);
            _banner.ShowBanner();

            _interstitial = new AdmobAdsInterstitial(_keys.Interstitial);
            _rewarded = new AdmobAdsRewarded(_keys.RewardedAds);
            _appOpen = new AdmobAdsAppOpen(_keys.AppOpen);

            if (showAppOpenOnStart)
            {
                ShowAppOpenAd();
            }
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            _appPaused = true;
        }
        else if (_appPaused)
        {
            _appPaused = false;
            if (showAppOpenOnResume)
            {
                ShowAppOpenAd();
            }
        }
    }

    private void DelayAds()
    {
        _delayed = true;

        this.DOKill();
        DOVirtual.DelayedCall(delayBetweenAds, () => _delayed = false).SetTarget(this);
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
