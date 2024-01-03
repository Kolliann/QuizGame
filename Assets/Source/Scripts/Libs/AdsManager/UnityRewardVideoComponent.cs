using System;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Source.Scripts.Libs.AdsManager
{
    public class UnityRewardVideoComponent : MonoBehaviour, IVideoRewardComponent
    {
        [SerializeField, Tooltip("Unity game id")]
        private string _gameId = "3220349";
        
        [SerializeField, Tooltip("Unity placement id")]
        private string _placementId = "OpenLater";

        /// <inheritdoc />
        /// <summary>
        /// Is ready show reward video ?
        /// </summary>
        public bool IsReady
        {
            get { return Advertisement.IsReady(_placementId) && Advertisement.GetPlacementState(_placementId) == PlacementState.Ready; }
            set { }
        }

        /// <inheritdoc />
        /// <summary>
        /// Is showing reward video in this time ?
        /// </summary>
        public bool IsShowing { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Is need reward user for watch reward video?
        /// </summary>
        public bool IsNeedReward { get; set; }

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
            Advertisement.Initialize(_gameId);
        }

        /// <inheritdoc />
        /// <summary>
        /// Start try load reward video
        /// </summary>
        public void TryLoad()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Start showing reward video
        /// </summary>
        public void Show()
        {
            if (!IsReady)
                return;

            IsShowing = true;
            var options = new ShowOptions {resultCallback = OnUnityAdsDidFinish};
            Advertisement.Show(_placementId, options);
        }

        /// <summary>
        /// On video closed callback
        /// </summary>
        private void OnUnityAdsDidFinish(ShowResult showResult)
        {
            IsShowing = false;
            switch (showResult)
            {
                case ShowResult.Failed:
                    IsNeedReward = false;
                    break;
                case ShowResult.Skipped:
                    IsNeedReward = false;
                    break;
                case ShowResult.Finished:
                    IsNeedReward = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("showResult", showResult, null);
            }

            if (OnCloseWatch != null)
                OnCloseWatch.Invoke();
        }
    }
}