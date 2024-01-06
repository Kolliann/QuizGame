using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameScripts : MonoBehaviour
{
    public QuestionList[] questions;
    public Text[] answearsText;
    public Text qText;

    public GameObject headPanel;

    List<object> qList;
    private QuestionList currentQ;
    private int randQ;

    public void OnClickPlay()
    {
        qList = new List<object>(questions);
        questionGenrate();
        if (!headPanel.GetComponent<Animator>().enabled)
        {
            headPanel.GetComponent<Animator>().enabled = true;

        }
        else
        {
            headPanel.GetComponent<Animator>().SetTrigger("play");
            
        }
    }

    public void OnClickClose()
    {
        
        if (!headPanel.GetComponent<Animator>().enabled)
        {
            headPanel.GetComponent<Animator>().enabled = true;

        }
        else
        {
            headPanel.GetComponent<Animator>().SetTrigger("CLose");
            
        }
        
    }

    public void questionGenrate()
    {
        if (qList.Count > 0)
        {
            randQ = Random.Range(0, qList.Count);
            currentQ = qList[randQ] as QuestionList;
            qText.text = currentQ.question;
            List<string> answears = new List<string>(currentQ.answers);
            for (int i = 0; i < currentQ.answers.Length; i++)
            {
                int rand = Random.Range(0, answears.Count);
                answearsText[i].text = answears[rand];
                answears.RemoveAt(rand);
            }
        }
        else
        {
            
            print("You Won");
        }
    }

    public void answearBtn(int index)
    {
        if (answearsText[index].text.ToString() == currentQ.answers[0])
        {
            print("верно");
        }
        else
        {
            print("no");
        }

        qList.RemoveAt(randQ);
        questionGenrate();
    }
}


[Serializable]
public class QuestionList
{
    public string question;
    public string[] answers = new string[3];
}