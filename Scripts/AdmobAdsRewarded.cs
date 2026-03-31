using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobAdsRewarded
{
    private string _adKey = "";

    private RewardedAd _rewardedAd;

    public AdmobAdsRewarded(string adKey)
    {
        _adKey = adKey;
        LoadRewarded();
    }

    public bool CanShowRewarded()
    {
        return _rewardedAd != null && _rewardedAd.CanShowAd();
    }

    public void ShowRewarded(Action onAdShowComplete, Action onAdShowFailed)
    {
        if (!CanShowRewarded())
        {
            onAdShowFailed?.Invoke();
            return;
        }

        _rewardedAd.Show(reward => onAdShowComplete?.Invoke());
        _rewardedAd = null;

        LoadRewarded();
    }

    private void LoadRewarded()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        AdRequest request = new AdRequest();

        RewardedAd.Load(_adKey, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded load failed: " + error);
                return;
            }

            _rewardedAd = ad;
            Debug.Log("Rewarded loaded");
        });
    }
}
