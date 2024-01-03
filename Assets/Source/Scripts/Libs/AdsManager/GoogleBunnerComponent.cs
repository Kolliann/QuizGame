using System;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Source.Scripts.Libs.AdsManager
{
    public class GoogleBunnerComponent : Singleton<GoogleBunnerComponent>
    {
        [SerializeField] private string _bottomBannerId;

        private BannerView _bottomBunner;

        private void TryLoadBunner()
        {
            _bottomBunner = new BannerView(_bottomBannerId, AdSize.Banner, AdPosition.Bottom);

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();

            // Load the banner with the request.
            _bottomBunner.LoadAd(request);


            // Called when an ad request has successfully loaded.
            _bottomBunner.OnAdLoaded += HandleOnAdLoaded;
            // Called when an ad request failed to load.
            _bottomBunner.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            _bottomBunner.OnAdOpening += HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            _bottomBunner.OnAdClosed += HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            _bottomBunner.OnAdLeavingApplication += HandleOnAdLeavingApplication;
        }

        public bool IsOnBunner { get; private set; }

        public void ShowBunner()
        {
            IsOnBunner = true;
            if (_bottomBunner != null)
                _bottomBunner.Show();
            else
            {
                TryLoadBunner();
            }
        }

        public void HideBunner()
        {
            IsOnBunner = false;
            if (_bottomBunner != null)
                _bottomBunner.Hide();
        }

        public void HandleOnAdLoaded(object sender, EventArgs args)
        {
            if (IsOnBunner)
                ShowBunner();
            else
                HideBunner();
        }

        public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            TryLoadBunner();
        }

        public void HandleOnAdOpened(object sender, EventArgs args)
        {
            Analytics.AnalyticController.SendClickBunner();
        }

        public void HandleOnAdClosed(object sender, EventArgs args)
        {
        }

        public void HandleOnAdLeavingApplication(object sender, EventArgs args)
        {
        }
    }
}