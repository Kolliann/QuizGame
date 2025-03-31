using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameScripts : MonoBehaviour
{

    public Question[] questions;
    public Text[] answearsText;
    public Text qText;

    public GameObject headPanel;

    List<IQuestion> qList;
    private IQuestion currentQ;
    private int randQ;

    public void OnClickPlay()
    {
        qList = new List<IQuestion>(questions);
        QuestionGenrate();
        if (!headPanel.GetComponent<Animator>().enabled)
        {
            headPanel.GetComponent<Animator>().enabled = true;
        }
        else
        {
            headPanel.GetComponent<Animator>().SetTrigger("Play");
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
            headPanel.GetComponent<Animator>().SetTrigger("Close");
        }
        
    }

    public void QuestionGenrate()
    {
        if (qList.Count > 0)
        {
            randQ = Random.Range(0, qList.Count);
            currentQ = qList[randQ] as IQuestion;
            qText.text = currentQ.Question();
            List<IAnswear> answears = new List<IAnswear>(currentQ.GetAnswears());
            int a = currentQ.GetAnswears().Count;
            for (int i = 0; i < a; ++i)
            {
                int rand = Random.Range(0, answears.Count);
                answearsText[i].text = answears[rand].GetAnswear();
                answears.RemoveAt(rand);
            }
        }
        else
        {
            print("You Won");
        }
    }

    public void AnswearBtn(int index)
    {
        if ( answearsText[index].text.ToString() == currentQ.GetCorrectAnswear() )
        {
            print("верно");
            qList.RemoveAt(randQ);
        }
        else
        {
            print("no");
        }
        QuestionGenrate();
    }
}
