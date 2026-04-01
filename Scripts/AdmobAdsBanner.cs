using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobAdsBanner
{ 
    private const float DEFAULT_BANNER_DPI = 50f;
    private const float BACK_HEIGHT_MULTIPLIER = 1.15f;

    private bool _isShowing;
    private string _adKey = "";
    private BannerView _bannerView;

    public bool IsShowing => _isShowing;

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
        _isShowing = true;
    }

    public void ShowBanner()
    {
        _isShowing = true;
        _bannerView?.Show();
    }

    public void HideBanner()
    {
        _isShowing = false;
        _bannerView?.Hide();
    }

    public void DestroyBanner()
    {
        _bannerView?.Destroy();
        _bannerView = null;
    }

    public float GetBannerHeight()
    {
        float screenDpi = Screen.dpi;
        float height = Mathf.RoundToInt(DEFAULT_BANNER_DPI * screenDpi / 160);
        height *= BACK_HEIGHT_MULTIPLIER;

#if UNITY_EDITOR
        height = 100f;
#endif
        return height;
    }
}
