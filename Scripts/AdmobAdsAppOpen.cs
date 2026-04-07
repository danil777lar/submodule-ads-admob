using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobAdsAppOpen
{
    private string _adKey = "";
    private AppOpenAd _appOpenAd;

    public AdmobAdsAppOpen(string adKey)
    {
        _adKey = adKey;
        LoadAppOpen();
    }

    public bool CanShowAppOpen()
    {
        return _appOpenAd != null && _appOpenAd.CanShowAd();
    }

    public bool ShowAppOpen()
    {
        if (!CanShowAppOpen())
        {
            return false;
        }

        _appOpenAd.Show();
        _appOpenAd = null;

        LoadAppOpen();
        return true;
    }

    private void LoadAppOpen()
    {
        if (_appOpenAd != null)
        {
            _appOpenAd.Destroy();
            _appOpenAd = null;
        }

        AdRequest request = new AdRequest();

        AppOpenAd.Load(_adKey, request, (AppOpenAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("App Open Ad load failed: " + error);
                return;
            }

            _appOpenAd = ad;
            Debug.Log("App Open Ad loaded");
        });
    }
}
