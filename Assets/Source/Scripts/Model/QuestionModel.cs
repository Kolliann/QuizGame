using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Model
{
    [Serializable]
    public class QuestionModel
    {
        [SerializeField]
        private string _questionId;
        
        [SerializeField]
        private string _questionText;
        
        [SerializeField]
        private string _answerText;
        
        [SerializeField]
        private string _questImage;

        public string QuestionText
        {
            get { return _questionText; }
            set { _questionText = value; }
        }

        public string AnswerText
        {
            get { return _answerText; }
            set { _answerText = value; }
        }
        
        public string Id
        {
            get { return _questionId; }
            set { _questionId = value; }
        }

        public string QuestImage
        {
            get { return _questImage; }
            set { _questImage = value; }
        }
    }
}