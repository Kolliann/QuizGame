using System;
using Analytics;
using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Windows
{
	public class RatingWindow : GameWindowBase
	{
		[SerializeField, Tooltip("No action button")]
		private Button _noButton;
		
		[SerializeField, Tooltip("Later action button")]
		private Button _laterButton;
		
		[SerializeField, Tooltip("Yes action button")]
		private Button _yesButton;


		private void Awake()
		{
			_noButton.onClick.AddListener(NoClick);
			_laterButton.onClick.AddListener(LaterClick);
			_yesButton.onClick.AddListener(RateClick);
		}

		private void NoClick()
		{
			AnimationController.Hide();
			AudioController.Instance.PlayClickButton();
			PlayerPrefs.SetInt("ShowRateWindow", 3);
			AnalyticController.SendRatingAction("NoShowRating", GameState.Instance.CompleteQuests.Count.ToString());
		}
		
		private void LaterClick()
		{
			AnimationController.Hide();
			PlayerPrefs.SetInt("ShowRateWindow", 2);
			AudioController.Instance.PlayClickButton();
			AnalyticController.SendRatingAction("ShowLaterRating", GameState.Instance.CompleteQuests.Count.ToString());
		}
		
		private void RateClick()
		{
			AnimationController.Hide();
			PlayerPrefs.SetInt("ShowRateWindow", 1);
			AudioController.Instance.PlayClickButton();
			Application.OpenURL ("market://details?id=com.Pinchukov.QuizWord");
			AnalyticController.SendRatingAction("ShowRatingRating", GameState.Instance.CompleteQuests.Count.ToString());
		}
	}
}
