using System;
using System.Linq;
using Model;
using UnityEngine;

namespace Source.Scripts.Model
{
    [Serializable]
    public class QuestionPackage
    {
        [SerializeField] private string _id;
        [SerializeField] private QuestionModel[] _questionModels;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public QuestionModel[] QuestionModels
        {
            get { return _questionModels; }
            set { _questionModels = value; }
        }

        public bool IsComplite
        {
            get { return _questionModels.All(questionModel => GameState.Instance.IsCompleteQuest(questionModel.Id)); }
        }

        public QuestionModel GetFirstNoneComplete
        {
            get
            {
                return _questionModels.FirstOrDefault(questionModel =>
                    !GameState.Instance.IsCompleteQuest(questionModel.Id));
            }
        }

        public int CompleteQuestionsCount
        {
            get
            {
                return _questionModels.Count(questionModel => GameState.Instance.IsCompleteQuest(questionModel.Id));
            }
        }
    }
}