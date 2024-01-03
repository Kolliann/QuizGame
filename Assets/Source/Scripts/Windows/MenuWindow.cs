using System;
using Controller;
using JetBrains.Annotations;
using Source.Scripts.Controller;
using Source.Scripts.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    /// <inheritdoc />
    /// <summary>
    /// This class controlled menu window view state
    /// </summary>
    public class MenuWindow : Singleton<MenuWindow>
    {
        [SerializeField, Tooltip("Text count completed quests")]
        private Text _progressText;

        [SerializeField, Tooltip("Text app version")]
        private Text _versionText;

        [SerializeField]
        private Button _playButton;


        public Button PlayButton
        {
            get { return _playButton; }
        }

        /// <summary>
        /// Initialize view
        /// </summary>
        private void OnEnable()
        {
            UpdateProgress();
            _versionText.text = Application.version;
            CheckName(() => { TutorialController.Instance.CheckTutorials(TutorialType.StartGame); });
        }

        /// <summary>
        /// Updated progress text
        /// </summary>
        private void UpdateProgress()
        {
            _progressText.text = GameState.Instance.CompleteQuests.Count.ToString();
        }

        private void CheckName(Action onComplete)
        {
            if (GameState.Instance.UserName.Equals("User"))
            {
                WindowManager.Instance.GetEnterNameWindow.AnimationController.Show();
                WindowManager.Instance.GetEnterNameWindow.OnChangeNameAction += (userName) =>
                {
                    WindowManager.Instance.GetEnterNameWindow.AnimationController.Hide();
                    GameState.Instance.SetNewNameValue(userName);
                    GameController.Instance.SelectLevelWindow.UserName = userName;

                    onComplete.Invoke();
                };
            }
            else
            {
                onComplete.Invoke();
            }
        }
        

        /// <summary>
        /// On click games button action
        /// </summary>
        [UsedImplicitly]
        public void OnClickDevelopGames()
        {
            WindowManager.Instance.GetDeveloperGamesWindow.AnimationController.Show();
        }


        /// <summary>
        /// On click exit button action
        /// </summary>
        [UsedImplicitly]
        public void OnClickAppExit()
        {
            Application.Quit();
        }
    }
}