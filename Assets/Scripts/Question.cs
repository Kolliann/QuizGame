using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Question : IQuestion
{
    [SerializeField]
    private string _question;

    [SerializeField]
    private List<Answear> _answers; 


    string IQuestion.Question()
    {
        return _question;
    }

    List<IAnswear> IQuestion.GetAnswears()
    {
        return  new List<IAnswear>(_answers);
    }

    string IQuestion.GetCorrectAnswear() { 
        foreach (IAnswear answer in _answers) {
            if (answer.IsCorrectAnswer()) {
                return answer.GetAnswear();
            }
         
        }
        return "";
    }  


}
