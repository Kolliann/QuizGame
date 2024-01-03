using System;
using Controller;
using JetBrains.Annotations;
using Source.Scripts.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class ResultWindow : MonoBehaviour
    {
        [SerializeField] private Animator _winWindowAnimator;
        [SerializeField] private Animator _cupAnimator;

        [SerializeField] private Text _prestigeCount;
        [SerializeField] private Text _answerText;

        [SerializeField] private Image _prestigeImage;

        [SerializeField] private GameObject _particlesEffect;

        public Action OnShowWindowAction { get; set; }

        public void ShowWinWindow(int prestigeCount, string answerText)
        {
            _answerText.text = answerText;
            gameObject.SetActive(true);
            if (prestigeCount > 0)
            {
                _cupAnimator.gameObject.SetActive(true);
               _prestigeCount.text = prestigeCount.ToString();
            }
            else
            {
                _cupAnimator.gameObject.SetActive(false);
            }

            _winWindowAnimator.SetTrigger("Show");
        }

        public void HideWimdowClick()
        {
            if (OnShowWindowAction != null)
                OnShowWindowAction.Invoke();

            _cupAnimator.SetTrigger("Void");
            _winWindowAnimator.SetTrigger("Hide");
        }

        public void OnFinishHide()
        {
            gameObject.SetActive(false);
        }

        public void OnFinishShow()
        {
            _cupAnimator.SetTrigger("Show");
        }

        private void OnDisable()
        {
            _particlesEffect.transform.localScale = Vector3.one;
        }
    }
}