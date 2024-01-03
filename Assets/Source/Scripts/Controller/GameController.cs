using System;
using System.Collections.Generic;
using Windows;
using Analytics;
using Model;
using Source.Scripts.Libs.AdsManager;
using Source.Scripts.Model;
using Source.Scripts.Windows;
using UnityEngine;
using View;
using Random = UnityEngine.Random;

namespace Controller
{
    /// <summary>
    /// This class create game logic
    /// </summary>
    public class GameController : MonoBehaviour
    {
        #region Singleton

        private static GameController _instance;

        public static GameController Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = FindObjectOfType<GameController>();
                return _instance != null ? _instance : null;
            }
        }

        private GameController()
        {
        }
        

        #endregion

        [SerializeField, Tooltip("Count create letter buttons")]
        private int _selectLetterElementCount;

        [SerializeField, Tooltip("Game advertising controller")]
        private RewardVideoManager _advertisingManager;

        [SerializeField, Tooltip("Game window controller")]
        private GameWindow _gameWindow;

        [SerializeField, Tooltip("Select Level Window controller")]
        private SelectLevelWindow _selectLevelWindow;
        
        [SerializeField, Tooltip("Result window controller")]
        private ResultWindow _resultWindow;
        
        [SerializeField, Tooltip("Message window controller")]
        private MessageWindow _messageWindow;
        
        public MessageWindow MessageWindow
        {
            get { return _messageWindow; }
        }

        public GameWindow GameWindow
        {
            get { return _gameWindow; }
        }

        public QuestionPackage[] QuestionPackages;

        public SelectLevelWindow SelectLevelWindow
        {
            get { return _selectLevelWindow; }
        }
        /// <summary>
        /// Game not correct letters variant
        /// </summary>
        private const string Alphabet = "абвгдежзийклмнопрстуфхцчшщъыьюя";

        /// <summary>
        /// List answers letter buttons 
        /// </summary>
        private List<AnswerLetterView> _answerElements;

        /// <summary>
        /// List select letter buttons
        /// </summary>
        private List<SelectLetterView> _selectElements;

        /// <summary>
        /// Count free answer buttons (with empty text)
        /// </summary>
        private int _countFreeAnswer;
        
        /// <summary>
        /// Current quest
        /// </summary>
        private QuestionModel _currentQuestion;

        public QuestionModel CurrentQuestion
        {
            get { return _currentQuestion; }
        }

        private bool _isReplayPackage;

        private List<QuestionModel> _replaysCompleteQuests;

        private QuestionPackage _currentQuestionPackage;

        private int _completeQuestionCount;
        
        public bool IsLevelLoaded { get; private set; }

        /// <summary>
        /// Start play quest player time
        /// </summary>
        private int _startPlayQuestTime;
        
        public int SelectLetterElementCount
        {
            get { return _selectLetterElementCount; }
        }
        
        public List<AnswerLetterView> AnswerElements
        {
            get { return _answerElements; }
        }
        
        public List<SelectLetterView> SelectElements
        {
            get { return _selectElements; }
        }

        public RewardVideoManager RewardVideoManager
        {
            get { return _advertisingManager; }
        }

        public bool IsCompleteOneQuest { get; private set; }

        private void OnEnable()
        {
            _replaysCompleteQuests = new List<QuestionModel>();
            _selectLevelWindow.Init();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            RewardVideoManager.Init();
            RewardVideoManager.TryLoadVideo();
            
            if(GameState.Instance.IsCompleteQuest("q05"))
                GoogleBunnerComponent.Instance.ShowBunner();
        }

  
        /// <summary>
        /// Shuffle list values
        /// <param>
        /// asymptotic complexity = O(N)
        /// </param>
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> ShuffleList<T>(List<T> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var j = Random.Range(i, list.Count);
                
                var tmp = list[i];
                list[i] = list[j];
                list[j] = tmp;
            }

            return list;
        }

        private static void SetTextSelectLetter(IList<SelectLetterView> selectLetters, QuestionModel question)
        {
            if (selectLetters.Count < question.AnswerText.Length)
                throw new Exception();

            question.AnswerText = question.AnswerText.ToLower();
            var tmpSelectLetters = new List<SelectLetterView>(selectLetters);
            var answerIndex = 0;
            while (true)
            {
                var selectIndex = UnityEngine.Random.Range(0, tmpSelectLetters.Count);

                var character = "";
                if (answerIndex + 1 <= question.AnswerText.Length)
                {
                    character = question.AnswerText[answerIndex].ToString();
                    answerIndex++;
                }
                else
                {
                    var randomAlphabetIndex = UnityEngine.Random.Range(0, Alphabet.Length);
                    character = Alphabet[randomAlphabetIndex].ToString();
                }

                tmpSelectLetters[selectIndex].LetterText = character;

                tmpSelectLetters.RemoveAt(selectIndex);

                if (tmpSelectLetters.Count == 0)
                    break;
            }
        }

        public void OnClickSelectLetter(SelectLetterView selectLetterView)
        {
            if (_countFreeAnswer >= _answerElements.Count)
                return;

            if (string.IsNullOrEmpty(selectLetterView.LetterText))
                return;

            var letter = selectLetterView.LetterText;
            selectLetterView.LetterText = string.Empty;
            selectLetterView.LetterColor = Color.gray;

            foreach (var answerElement in _answerElements)
            {
                if (string.IsNullOrEmpty(answerElement.LetterText))
                {
                    answerElement.LetterText = letter;
                    break;
                }
            }

            _countFreeAnswer++;
            UpdateCursor();
            if (_countFreeAnswer == _answerElements.Count)
                CheckCompleteGame();
        }

        private void CheckCompleteGame()
        {
            var result = string.Empty;
            foreach (var answerElement in _answerElements)
            {
                result = string.Format("{0}{1}", result, answerElement.LetterText);
            }

            if (result == _currentQuestion.AnswerText)
            {
                if (!_isReplayPackage)
                {
                    var playedTime = DateTimeToUnixTimeStamp(DateTime.Now) - _startPlayQuestTime;
                    AnalyticController.SendCompleteQuest(_currentQuestion, playedTime);
                }
                int prestige = _isReplayPackage ? 0 : _currentQuestion.AnswerText.Length;
                
                IsCompleteOneQuest = true;
                _resultWindow.ShowWinWindow(prestige, result);
                _resultWindow.OnShowWindowAction = ShowNexQuest;
                
                if (_isReplayPackage)
                {
                    _replaysCompleteQuests.Add(_currentQuestion);
                }
                else
                {
                    GameState.Instance.AddPrestige(_currentQuestion.AnswerText.Length);
                    GameState.Instance.AddCompleteQuest(_currentQuestion.Id);
                }
                
            }
            else
            {
                
                _gameWindow.IncorrectText.gameObject.SetActive(true);
            }
        }

        public void ShowNexQuest()
        {
            _advertisingManager.TryLoadVideo();
            _gameWindow.RemoveWindowElements();
            ClearGame();

            
            ShowNextQuestion();
            
            _completeQuestionCount++;
            
            _gameWindow.ProgressText = string.Format("{1}/{0}", _currentQuestionPackage.QuestionModels.Length,
                _completeQuestionCount);
        }

        public void OnClickAnswerLetter(AnswerLetterView answerLetterView)
        {
            if (string.IsNullOrEmpty(answerLetterView.LetterText))
                return;

            var letter = answerLetterView.LetterText;
            answerLetterView.LetterText = string.Empty;
            foreach (var elem in _selectElements)
            {
                if (string.IsNullOrEmpty(elem.LetterText))
                {
                    elem.LetterText = letter;
                    elem.LetterColor = Color.white;
                    _countFreeAnswer--;
                    break;
                }
            }

            UpdateCursor();
            _gameWindow.IncorrectText.gameObject.SetActive(false);
        }

        public void ClearWindowElements()
        {
            foreach (var answerElement in _answerElements)
            {
                answerElement.LetterText = string.Empty;
            }

            foreach (var selectElement in _selectElements)
            {
                selectElement.LetterText = string.Empty;
                selectElement.LetterColor = Color.white;
            }
        }

        public void ResetGame()
        {
            _selectLevelWindow.Clear();
            GameState.Instance.Clear();
            _selectLevelWindow.Init();
        }


        public void ClearGame()
        {
            if (_answerElements != null)
                _answerElements.Clear();
            if (_selectElements != null)
                _selectElements.Clear();
            _countFreeAnswer = 0;
            
            _selectLevelWindow.Clear();
            _selectLevelWindow.Init();
            
            IsLevelLoaded = false;
        }

        public void ShowOneCorrectLetter()
        {
            for (int i = 0; i < _currentQuestion.AnswerText.Length; i++)
            {
                if (_answerElements[i].LetterText == _currentQuestion.AnswerText[i].ToString()) 
                    continue;

                var correctLetter = _currentQuestion.AnswerText[i].ToString();
                
                AnalyticController.SendOpenOneLetter(_currentQuestion.Id, correctLetter);
                OnClickAnswerLetter(_answerElements[i]);
                foreach (var selectElement in _selectElements)
                {
                    if (selectElement.LetterText != correctLetter) 
                        continue;
                    
                    OnClickSelectLetter(selectElement);
                    break;
                }
                break;
            }
        }


        public void OnPackageClick(QuestionPackage package)
        {
            _advertisingManager.TryLoadVideo();
            _currentQuestionPackage = package;
            _gameWindow.gameObject.SetActive(true);
            _selectLevelWindow.gameObject.SetActive(false);
            _selectLevelWindow.Clear();

            _isReplayPackage = package.IsComplite;
            _replaysCompleteQuests.Clear();
            
            ShowNextQuestion();

            _completeQuestionCount = 0;
            if (!_isReplayPackage)
                _completeQuestionCount = package.CompleteQuestionsCount;
            _gameWindow.ProgressText = string.Format("{1}/{0}", package.QuestionModels.Length, _completeQuestionCount);
            
        }

        private void ShowNextQuestion()
        {
            _startPlayQuestTime = DateTimeToUnixTimeStamp(DateTime.Now);
            foreach (var questionModel in _currentQuestionPackage.QuestionModels)
            {
                if (_isReplayPackage)
                {
                    if (!_replaysCompleteQuests.Contains(questionModel))
                    {
                        _currentQuestion = questionModel;
                        _gameWindow.QuestionText = _currentQuestion.QuestionText;
                        _gameWindow.QuestionImage = _currentQuestion.QuestImage;
                        _answerElements = _gameWindow.CreateAnswerElements(_currentQuestion);
                        _selectElements = _gameWindow.CreateSelectElements();
                        SetTextSelectLetter(_selectElements, _currentQuestion);
                        UpdateCursor();
                        IsLevelLoaded = true;
                        return;
                    }
                    
                    continue;
                }
                
                if(!GameState.Instance.IsCompleteQuest(questionModel.Id))
                {
                    _currentQuestion = questionModel;
                    _gameWindow.QuestionText = _currentQuestion.QuestionText;
                    _gameWindow.QuestionImage = _currentQuestion.QuestImage;
                    _answerElements = _gameWindow.CreateAnswerElements(_currentQuestion);
                    _selectElements = _gameWindow.CreateSelectElements();
                    SetTextSelectLetter(_selectElements, _currentQuestion);
                    UpdateCursor();
                    IsLevelLoaded = true;
                    return;
                }
            }

            _selectLevelWindow.Clear();
            _selectLevelWindow.Init();
            _gameWindow.gameObject.SetActive(false);
            _selectLevelWindow.gameObject.SetActive(true);
            _replaysCompleteQuests.Clear();

            IsLevelLoaded = false;
        }

        /// <summary>
        /// Convert datetime object to unix time
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int DateTimeToUnixTimeStamp(DateTime dateTime)
        {
            var dtDateTime = new DateTime(1970, 1, 1,0,0,0, DateTimeKind.Utc);
            return (int)(dateTime - dtDateTime).TotalSeconds;
        }

        private void UpdateCursor()
        {
            if(_answerElements == null || _answerElements.Count == 0)
                return;

            bool isFindCursor = false;
            foreach (var answerLetterView in _answerElements)
            {
                answerLetterView.Cursor.gameObject.SetActive(false);

                if (!string.IsNullOrEmpty(answerLetterView.LetterText) || isFindCursor) 
                    continue;
                
                isFindCursor = true;
                answerLetterView.Cursor.gameObject.SetActive(true);
            }
        }
    }
}