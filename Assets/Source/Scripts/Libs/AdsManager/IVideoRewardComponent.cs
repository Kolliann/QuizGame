using System;

namespace Source.Scripts.Libs.AdsManager
{
    /// <summary>
    /// Reward video ads component for ads manager
    /// </summary>
    public interface IVideoRewardComponent
    {
        /// <summary>
        /// Is ready show reward video ?
        /// </summary>
        bool IsReady { get; set; }

        /// <summary>
        /// Is need reward user for watch reward video?
        /// </summary>
        bool IsNeedReward { get; set; }

        /// <summary>
        /// Is showing reward video in this time ?
        /// </summary>
        bool IsShowing { get; set; }

        /// <summary>
        /// Action call when video closed watch
        /// </summary>
        Action OnCloseWatch { get; set; }

        /// <summary>
        /// Initialize component
        /// </summary>
        void Init();

        /// <summary>
        /// Start try load reward video
        /// </summary>
        void TryLoad();

        /// <summary>
        /// Start showing reward video
        /// </summary>
        void Show();
    }
}