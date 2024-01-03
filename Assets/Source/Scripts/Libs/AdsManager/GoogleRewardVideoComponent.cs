using System;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Source.Scripts.Libs.AdsManager
{
    /// <inheritdoc cref="IVideoRewardComponent"/>
    /// <summary>
    /// Google Admob Reward video component
    /// </summary>
    public class GoogleRewardVideoComponent : MonoBehaviour, IVideoRewardComponent
    {
        [SerializeField, Tooltip("Video Banner Id")]
        private string _videoBannerId;
       
        [SerializeField, Tooltip("App Id")]
        private string _appId;
        
        /// <summary>
        /// Rewarder video object
        /// </summary>
        private RewardBasedVideoAd _rewardBasedVideo;

        /// <inheritdoc />
        /// <summary>
        /// Is ready show reward video ?
        /// </summary>
        public bool IsReady { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Is need reward user for watch reward video?
        /// </summary>
        public bool IsNeedReward { get; set; }
        
        /// <inheritdoc />
        /// <summary>
        /// Is showing reward video in this time ?
        /// </summary>
        public bool IsShowing { get; set; }
        
        /// <inheritdoc />
        /// <summary>
        /// Action call when video closed watch
        /// </summary>
        public Action OnCloseWatch { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Initialize component
        /// </summary>
        public void Init()
        {
            MobileAds.Initialize(_appId);

            _rewardBasedVideo = RewardBasedVideoAd.Instance;

            _rewardBasedVideo.OnAdLoaded += VideoLoaded;
            _rewardBasedVideo.OnAdFailedToLoad += VideoFailedToLoad;
            _rewardBasedVideo.OnAdRewarded += VideoRewarded;
            _rewardBasedVideo.OnAdClosed += VideoClosed;

            TryLoad();
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Start try load reward video
        /// </summary>
        public void TryLoad()
        {
            if (IsReady)
                return;

            var request = new AdRequest.Builder().Build();
            _rewardBasedVideo.LoadAd(request, _videoBannerId);
        }

        /// <inheritdoc />
        /// <summary>
        /// Start showing reward video
        /// </summary>
        public void Show()
        {
            if (!IsReady) 
                return;
            
            IsReady = false;
            IsShowing = true;
            _rewardBasedVideo.Show();
        }

        /// <summary>
        /// On video load callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void VideoLoaded(object sender, EventArgs args)
        {
            IsReady = true;
        }

        /// <summary>
        /// On video failed load callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void VideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            IsReady = false;
        }

        /// <summary>
        /// On video closed callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void VideoClosed(object sender, EventArgs args)
        {
            IsReady = false;
            IsShowing = false;
            if (OnCloseWatch != null)
                OnCloseWatch.Invoke();
        }

        /// <summary>
        /// On video complete watch callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void VideoRewarded(object sender, Reward args)
        {
            IsNeedReward = true;
        }

    }
}