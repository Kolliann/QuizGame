using System;
using System.Collections.Generic;
using Controller;
using JetBrains.Annotations;
using Source.Scripts.Controller;
using Source.Scripts.Model;
using Source.Scripts.View;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

namespace Source.Scripts.Windows
{
    public class SelectLevelWindow : MonoBehaviour
    {
        [SerializeField, Tooltip("Prefab for create level item")]
        private LevelItemView _levelItemPrefab;

        [SerializeField, Tooltip("Root for create level items")]
        private Transform _rootLevelItems;

        [SerializeField, Tooltip("Player prestige count texts")]
        private Text _prestigeCountText;

        [SerializeField, Tooltip("Player name text")]
        private Text _userNameText;

        [SerializeField] private int _countShowNoneComplete;

        [SerializeField]
        private ScrollRect _scroll;

        public List<LevelItemView> Views { get; private set; }
        
        /// <summary>
        /// Set new user name on window
        /// </summary>
        public string UserName
        {
            set { _userNameText.text = value; }
        }

        private bool _showRatingGameWindow = false;

        private void OnEnable()
        {
            if (!IsStartShowRatingWindow())
            {
                TutorialController.Instance.CheckTutorials(TutorialType.SelectPack);
            }
        }

        private bool IsStartShowRatingWindow()
        {
            var ratingWindowCode = PlayerPrefs.GetInt("ShowRateWindow", 0);
            if (ratingWindowCode == 1)
            {
                // we click rate game
            }
            if (ratingWindowCode == 2)
            {
                // we click later rate
                if (!_showRatingGameWindow && GameState.Instance.IsCompleteQuest("q29") &&
                    GameController.Instance.IsCompleteOneQuest)
                {
                    _showRatingGameWindow = true;
                    WindowManager.Instance.GetRatingWindow.AnimationController.Show();
                    
                    return true;
                }
            }
            if (ratingWindowCode == 3)
            {
                // we click no rate
            }
            else
            {
                // we dont show before
                if (GameState.Instance.IsCompleteQuest("q29") && GameController.Instance.IsCompleteOneQuest)
                {
                    _showRatingGameWindow = true;
                    WindowManager.Instance.GetRatingWindow.AnimationController.Show();
                    
                    return true;
                }
            }
            
            return false;
        }
        
        public void Init()
        {
            _userNameText.text = GameState.Instance.UserName;
            var packages = GameController.Instance.QuestionPackages;

            _prestigeCountText.text = GameState.Instance.Prestige.ToString();
            
            Views = new List<LevelItemView>();
            
            int counter = 0;
            int index = 0;
            foreach (var package in packages)
            {
                index++;
                var countComplete = 0;
                foreach (var questionModel in package.QuestionModels)
                {
                    if (GameState.Instance.IsCompleteQuest(questionModel.Id))
                        countComplete++;
                }

                var view = Instantiate(_levelItemPrefab, _rootLevelItems);
                view.Init(package, GetStarCount(countComplete, package), counter < _countShowNoneComplete);
                Views.Add(view);
                if(counter < _countShowNoneComplete)
                    _scroll.content.transform.localPosition = new Vector3(0,((index / 3) - 1) * 240,0);
                
                if (counter < _countShowNoneComplete && countComplete != package.QuestionModels.Length)
                {
                    counter++;
                }
            }
        }

        
        
        public void Clear()
        {
            for (int i = 0; i < _rootLevelItems.childCount; i++)
            {
                Destroy(_rootLevelItems.GetChild(i).gameObject);
            }
        }

        private int GetStarCount(int countComplete, QuestionPackage package)
        {
            if (countComplete == 0 || countComplete < package.QuestionModels.Length / 3)
                return 3;
            if (countComplete == package.QuestionModels.Length)
                return 0;
            if (countComplete >= package.QuestionModels.Length / 3 &&
                countComplete <= package.QuestionModels.Length * 2 / 3)
                return 2;
            if (countComplete > package.QuestionModels.Length * 2 / 3)
                return 1;

            return 0;
        }
    }
}