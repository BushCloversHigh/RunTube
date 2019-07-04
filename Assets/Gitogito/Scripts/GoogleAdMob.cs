using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAdMob : MonoBehaviour
{
    private static BannerView bannerTop, bannerBottom;

    private void Start ()
    {
        string appId = DataStrings.ADDID;

        MobileAds.Initialize (appId);
    }

    public void RequestBannerTop ()
    {
#if UNITY_ANDROID
        string adUnitId = DataStrings.BANNERTOP;
#elif UNITY_IPHONE
            string adUnitId = DataStrings.BANNERTOP;
#else
            string adUnitId = "unexpected_platform";
#endif

        bannerTop = new BannerView (adUnitId, AdSize.Banner, AdPosition.Top);

        AdRequest request = new AdRequest.Builder ().Build ();

        bannerTop.LoadAd (request);
    }

    public void RequestBannerBottom ()
    {
#if UNITY_ANDROID
        string adUnitId = DataStrings.BANNERBOTTOM;
#elif UNITY_IPHONE
        string adUnitId = DataStrings.BANNERBOTTOM;
#else
        string adUnitId = "unexpected_platform";
#endif

        bannerBottom = new BannerView (adUnitId, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder ().Build ();

        bannerBottom.LoadAd (request);
    }

    public void DestroyBanner ()
    {
        if (bannerTop != null)
        {
            bannerTop.Destroy ();
        }
        if (bannerBottom != null)
        {
            bannerBottom.Destroy ();
        }
    }
}
