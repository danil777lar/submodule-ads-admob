using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobAdsBanner
{ 
    private string _adKey = "";
    private BannerView _bannerView;

    public AdmobAdsBanner(string adKey)
    {
        _adKey = adKey;
        if (_bannerView != null)
        {
            _bannerView.Destroy();
            _bannerView = null;
        }

        _bannerView = new BannerView(_adKey, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest();
        _bannerView.LoadAd(request);
    }

    public void HideBanner()
    {
        _bannerView?.Hide();
    }

    public void ShowBanner()
    {
        _bannerView?.Show();
    }

    public void DestroyBanner()
    {
        _bannerView?.Destroy();
        _bannerView = null;
    }
}
