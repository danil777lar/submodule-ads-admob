using DG.Tweening;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobAdsInterstitial
{
    private string _adKey = "";
    private InterstitialAd _interstitial;

    public AdmobAdsInterstitial(string adKey)
    {
        _adKey = adKey;

        LoadInterstitial();
    }

    public bool CanShowInterstitial()
    {
        return _interstitial != null && _interstitial.CanShowAd();
    }

    public void ShowInterstitial()
    {
        if (CanShowInterstitial())
        {
            _interstitial.Show();
            _interstitial = null;

            LoadInterstitial();
        }
    }

    private void LoadInterstitial()
    {
        if (_interstitial != null)
        {
            _interstitial.Destroy();
            _interstitial = null;
        }

        AdRequest request = new AdRequest();

        InterstitialAd.Load(_adKey, request, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Interstitial load failed: " + error);
                return;
            }

            _interstitial = ad;
            Debug.Log("Interstitial loaded");
        });
    }
}
