using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Answear : IAnswear
{
    [SerializeField]
    private string _answear;

    [SerializeField]
    private bool _correctAnswer;

    public Answear( string answear, bool correctAnswer ) {
        _answear = answear;
        _correctAnswer = correctAnswer;
    }

    bool IAnswear.IsCorrectAnswer()
    {
        return _correctAnswer;
    }

    string IAnswear.GetAnswear()
    {
        return _answear;
    }
}
