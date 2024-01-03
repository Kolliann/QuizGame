using System;
using System.Collections;
using Windows;
using Controller;
using Source.Scripts.Windows;
using UnityEngine;
using View;

namespace Source.Scripts.Controller
{
    public class TutorialController : Singleton<TutorialController>
    {
        public void CheckTutorials(TutorialType type)
        {
            switch (type)
            {
                case TutorialType.StartGame:
                    CheckMenuTutorial();
                    break;
                case TutorialType.SelectPack:
                    CheckSelectPackTutorial();
                    break;
                case TutorialType.FirstLevel:
                    CheckFirstLevelTutorial();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }
        }


        private void CheckSelectPackTutorial()
        {
            if(GameState.Instance.Prestige > 0)
                return;

            if (GameState.Instance.IsTutorialComplete(TutorialType.SelectPack.ToString())) 
                return;
            
            var item = GameController.Instance.SelectLevelWindow.Views[0];
                
            var tutorWindow = WindowManager.Instance.GetTutorialWindow;
            tutorWindow.TransformFollow = item.transform;
                
            var boxSetting = new TutorialWindow.DialogBoxSetting
            {
                IsShowed = true, PositionOffset = new Vector3(200, -400), Scale = Vector2.one,
                Text = "Выберите сборник уровней"
            };
                
            var maskSetting = new TutorialWindow.MaskSetting
            {
                IsShowed = true, PositionOffset = new Vector3(0, 0), Scale = new Vector2(1.5f, 1.5f)
            };

            var arrowSetting = new TutorialWindow.ArrowSetting
            {
                IsShowed = true, PositionOffset = new Vector3(100, 0)
            };

            tutorWindow.AnimationController.FinishShowAction = () =>
            {
                tutorWindow.Init(boxSetting, maskSetting, arrowSetting, () =>
                {
                    GameState.Instance.CompleteTutorial(TutorialType.SelectPack.ToString());
                    WindowManager.Instance.GetTutorialWindow.AnimationController.Hide();
                });
            };

            tutorWindow.AnimationController.StartHideAction = () => { tutorWindow.Clear(); };
            tutorWindow.AnimationController.FinishHideAction = () =>
            {
                GameController.Instance.SelectLevelWindow.gameObject.SetActive(false);
                AudioController.Instance.PlayClickButton();
                GameController.Instance.OnPackageClick(GameController.Instance.QuestionPackages[0]);
            };

            tutorWindow.AnimationController.Show();
        }

        private void CheckMenuTutorial()
        {
            if(GameState.Instance.Prestige > 0)
                return;

            if (GameState.Instance.IsTutorialComplete(TutorialType.StartGame.ToString())) 
                return;
            
            var tutorWindow = WindowManager.Instance.GetTutorialWindow;
            tutorWindow.TransformFollow = MenuWindow.Instance.PlayButton.transform;

            var boxSetting = new TutorialWindow.DialogBoxSetting
            {
                IsShowed = true, PositionOffset = new Vector3(0, -200), Scale = Vector2.one,
                Text = "Добро пожаловать! Запустим скорее игру"
            };

            var maskSetting = new TutorialWindow.MaskSetting
            {
                IsShowed = true, PositionOffset = new Vector3(0, 70), Scale = new Vector2(3.5f, 1.3f)
            };

            var arrowSetting = new TutorialWindow.ArrowSetting
            {
                IsShowed = true, PositionOffset = new Vector3(300, 70)
            };


            tutorWindow.AnimationController.FinishShowAction = () =>
            {
                tutorWindow.Init(boxSetting, maskSetting, arrowSetting, () =>
                {
                    GameState.Instance.CompleteTutorial(TutorialType.StartGame.ToString());
                    WindowManager.Instance.GetTutorialWindow.AnimationController.Hide();
                });
            };

            tutorWindow.AnimationController.StartHideAction = () => { tutorWindow.Clear(); };
            tutorWindow.AnimationController.FinishHideAction = () =>
            {
                MenuWindow.Instance.gameObject.SetActive(false);
                GameController.Instance.SelectLevelWindow.gameObject.SetActive(true);
            };

            tutorWindow.AnimationController.Show();
        }

        private void CheckFirstLevelTutorial()
        {
            StartCoroutine(FirstLevelPart1());
        }

        private IEnumerator FirstLevelPart1()
        {
            while (true)
            {
                if (!GameController.Instance.IsLevelLoaded)
                    yield return null;
                else
                {
                    break;
                }
            }

            if (GameController.Instance.CurrentQuestion.Id != "q00")
                yield break;
            
            
            ShuffledLetter(1, "н");
            ShuffledLetter(2, "к");
            ShuffledLetter(3, "а");
            ShuffledLetter(4, "у");

            var tutorWindow = WindowManager.Instance.GetTutorialWindow;
            tutorWindow.TransformFollow = GameController.Instance.GameWindow.QuestionTransform;

            var boxSetting = new TutorialWindow.DialogBoxSetting
            {
                IsShowed = true, PositionOffset = new Vector2(0, -500), Scale = Vector2.one,
                Text = "Хм, какой же ответ на загадку?"
            };

            var maskSetting = new TutorialWindow.MaskSetting
            {
                IsShowed = true, PositionOffset = new Vector2(0, -100), Scale = new Vector2(9.5f, 3.2f)
            };

            var arrowSetting = new TutorialWindow.ArrowSetting
            {
                IsShowed = false, PositionOffset = Vector2.one
            };

            tutorWindow.AnimationController.FinishShowAction = () =>
            {
                tutorWindow.Init(boxSetting, maskSetting, arrowSetting, FirstLevelPart2, FirstLevelPart2, FirstLevelFinish);
            };

            tutorWindow.AnimationController.StartHideAction = () => { tutorWindow.Clear(); };
            tutorWindow.AnimationController.FinishHideAction = () => { };

            tutorWindow.AnimationController.Show();
        }

        private void FirstLevelPart2()
        {
            var tutorWindow = WindowManager.Instance.GetTutorialWindow;
            tutorWindow.TransformFollow = GameController.Instance.SelectElements[1].transform;
            
            var boxSetting = new TutorialWindow.DialogBoxSetting
            {
                IsShowed = true, PositionOffset = new Vector2(-200, 200), Scale = Vector2.one,
                Text = "Давай попробуем букву Н"
            };

            var maskSetting = new TutorialWindow.MaskSetting
            {
                IsShowed = true, PositionOffset = new Vector2(-60, -60), Scale = Vector2.one
            };

            var arrowSetting = new TutorialWindow.ArrowSetting
            {
                IsShowed = true, PositionOffset = new Vector2(60, -60)
            };

            tutorWindow.Init(boxSetting, maskSetting, arrowSetting, FirstLevelPart3, null, FirstLevelFinish);
        }

        private void FirstLevelPart3()
        {
            GameController.Instance.OnClickSelectLetter(GameController.Instance.SelectElements[1]);

            var tutorWindow = WindowManager.Instance.GetTutorialWindow;
            tutorWindow.TransformFollow = GameController.Instance.SelectElements[2].transform;
            
            var boxSetting = new TutorialWindow.DialogBoxSetting
            {
                IsShowed = true, PositionOffset = new Vector2(-50, 200), Scale = Vector2.one,
                Text = "Давай попробуем букву К"
            };

            var maskSetting = new TutorialWindow.MaskSetting
            {
                IsShowed = true, PositionOffset = new Vector2(-60, -60), Scale = Vector2.one
            };

            var arrowSetting = new TutorialWindow.ArrowSetting
            {
                IsShowed = true, PositionOffset = new Vector2(60, -60)
            };

            tutorWindow.Init(boxSetting, maskSetting, arrowSetting, FirstLevelPart4, null, FirstLevelFinish);
        }

        private void FirstLevelPart4()
        {
            GameController.Instance.OnClickSelectLetter(GameController.Instance.SelectElements[2]);

            var tutorWindow = WindowManager.Instance.GetTutorialWindow;
            tutorWindow.TransformFollow = GameController.Instance.AnswerElements[1].transform;
            
            var boxSetting = new TutorialWindow.DialogBoxSetting
            {
                IsShowed = true, PositionOffset = new Vector2(200, -400), Scale = Vector2.one,
                Text = "Упс, мы ошиблись, давай уберем неправильную букву К"
            };

            var maskSetting = new TutorialWindow.MaskSetting
            {
                IsShowed = true, PositionOffset = new Vector2(0, 0), Scale = Vector2.one
            };

            var arrowSetting = new TutorialWindow.ArrowSetting
            {
                IsShowed = true, PositionOffset = new Vector2(150, 0)
            };

            tutorWindow.Init(boxSetting, maskSetting, arrowSetting, FirstLevelPart5, null, FirstLevelFinish);
        }

        private void FirstLevelPart5()
        {
            GameController.Instance.OnClickAnswerLetter(GameController.Instance.AnswerElements[1]);

            var tutorWindow = WindowManager.Instance.GetTutorialWindow;
            tutorWindow.TransformFollow = GameController.Instance.SelectElements[3].transform;
            
            var boxSetting = new TutorialWindow.DialogBoxSetting
            {
                IsShowed = true, PositionOffset = new Vector2(100, 200), Scale = Vector2.one,
                Text = "Давай попробуем букву A"
            };

            var maskSetting = new TutorialWindow.MaskSetting
            {
                IsShowed = true, PositionOffset = new Vector2(-60, -60), Scale = Vector2.one
            };

            var arrowSetting = new TutorialWindow.ArrowSetting
            {
                IsShowed = true, PositionOffset = new Vector2(60, -60)
            };

            tutorWindow.Init(boxSetting, maskSetting, arrowSetting, FirstLevelPart6, null, FirstLevelFinish);
        }

        private void FirstLevelPart6()
        {
            GameController.Instance.OnClickSelectLetter(GameController.Instance.SelectElements[3]);

            var tutorWindow = WindowManager.Instance.GetTutorialWindow;
            tutorWindow.TransformFollow = GameController.Instance.SelectElements[4].transform;
            
            var boxSetting = new TutorialWindow.DialogBoxSetting
            {
                IsShowed = true, PositionOffset = new Vector2(250, 200), Scale = Vector2.one,
                Text = "Давай попробуем букву У"
            };

            var maskSetting = new TutorialWindow.MaskSetting
            {
                IsShowed = true, PositionOffset = new Vector2(-60, -60), Scale = Vector2.one
            };

            var arrowSetting = new TutorialWindow.ArrowSetting
            {
                IsShowed = true, PositionOffset = new Vector2(60, -60)
            };

            tutorWindow.Init(boxSetting, maskSetting, arrowSetting, FirstLevelPart7, null, FirstLevelFinish);
        }

        private void FirstLevelPart7()
        {
            GameController.Instance.OnClickSelectLetter(GameController.Instance.SelectElements[4]);

            var tutorWindow = WindowManager.Instance.GetTutorialWindow;
            tutorWindow.TransformFollow = GameController.Instance.GameWindow.QuestionTransform;
            
            var boxSetting = new TutorialWindow.DialogBoxSetting
            {
                IsShowed = true, PositionOffset = new Vector2(0, -800), Scale = Vector2.one,
                Text = "Теперь твоя очередь :)"
            };

            var maskSetting = new TutorialWindow.MaskSetting
            {
                IsShowed = true, PositionOffset = new Vector2(0, -200), Scale = new Vector2(10, 5)
            };

            var arrowSetting = new TutorialWindow.ArrowSetting
            {
                IsShowed = false, PositionOffset = Vector2.one
            };

            tutorWindow.Init(boxSetting, maskSetting, arrowSetting, FirstLevelFinish,FirstLevelFinish, FirstLevelFinish);
        }

        private void FirstLevelFinish()
        {
            var tutorWindow = WindowManager.Instance.GetTutorialWindow;
            tutorWindow.AnimationController.Hide();
        }

        private void ShuffledLetter(int index, string latter)
        {
            SelectLetterView letter = null;
            foreach (var selectLetter in GameController.Instance.SelectElements)
            {
                if (selectLetter.LetterText != latter)
                    continue;

                letter = selectLetter;
            }

            if (letter != null)
            {
                var tmp = GameController.Instance.SelectElements[index].LetterText;
                GameController.Instance.SelectElements[index].LetterText = letter.LetterText;
                letter.LetterText = tmp;
            }
        }
        
        
    }


    public enum TutorialType
    {
        StartGame,
        SelectPack,
        FirstLevel
    }
}