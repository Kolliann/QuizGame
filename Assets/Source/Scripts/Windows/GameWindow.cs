using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Model;
using Source.Scripts.Controller;
using Source.Scripts.Libs.AdsManager;
using UnityEngine;
using UnityEngine.UI;
using View;

namespace Windows
{
    public class GameWindow : MonoBehaviour
    {
        [SerializeField] private Text _questionText;

        [SerializeField] private Transform _answerLetterRoot;

        [SerializeField] private GameObject _answerLetterPrefab;

        [SerializeField] private GameObject _answerSelectLetterPrefab;

        [SerializeField] private Transform _answerSelectLetterRoot;

        [SerializeField] private Text _progressText;
        
        [SerializeField] private Text _answerIncorrectText;
        
        
        [SerializeField] private Image _questImage;
        
        public Text IncorrectText
        {
            set { _answerIncorrectText = value; }
            get { return _answerIncorrectText; }
        }
        
        public string ProgressText
        {
            set { _progressText.text = value; }
        }

        public Transform QuestionTransform
        {
            get { return _questionText.transform; }
        }
        
        
        public string QuestionText
        {
            set
            {
                _questionText.text = value;
            }
        }
        
        public string QuestionImage
        {
            set
            {
                _questImage.sprite = Resources.Load<Sprite>("QuestionsImages/" + value);
            }
        }
        

        public List<SelectLetterView> CreateSelectElements()
        {
            var selectLetters = new List<SelectLetterView>();
            for (var i = 0; i < GameController.Instance.SelectLetterElementCount; i++)
            {
                var elem = Instantiate(_answerSelectLetterPrefab, _answerSelectLetterRoot);
                selectLetters.Add(elem.GetComponent<SelectLetterView>());
            }

            return selectLetters;
        }

        public List<AnswerLetterView> CreateAnswerElements(QuestionModel question)
        {
            var selectLetters = new List<AnswerLetterView>();
            for (var i = 0; i < question.AnswerText.Length; i++)
            {
                var elem = Instantiate(_answerLetterPrefab, _answerLetterRoot);
                selectLetters.Add(elem.GetComponent<AnswerLetterView>());
            }

            return selectLetters;
        }


        private void OnDisable()
        {
            RemoveWindowElements();
            if(GameState.Instance.IsCompleteQuest("q05"))
                GoogleBunnerComponent.Instance.ShowBunner();
        }


        public void RemoveWindowElements()
        {
            for (var i = 0; i < _answerLetterRoot.childCount; i++)
            {
                Destroy(_answerLetterRoot.GetChild(i).gameObject);
            }

            for (var i = 0; i < _answerSelectLetterRoot.childCount; i++)
            {
                Destroy(_answerSelectLetterRoot.GetChild(i).gameObject);
            }
        }

        private void OnEnable()
        {
            _answerIncorrectText.gameObject.SetActive(false);
            GoogleBunnerComponent.Instance.HideBunner();
            TutorialController.Instance.CheckTutorials(TutorialType.FirstLevel);
        }

        private IEnumerator WaitOneSeconds(Action onCompleted)
        {
            yield return new WaitForSeconds(1.0f);

            if (onCompleted != null)
                onCompleted.Invoke();
        }
    }
}