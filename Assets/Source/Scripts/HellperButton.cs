using System.Collections;
using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts
{
    public class HellperButton : MonoBehaviour
    {
        [SerializeField]
        private Button _helperButton;
        
        [SerializeField]
        private Text _helperText;

        [SerializeField]
        private int _timerSecondsCount;

        private bool _isHaveEthernet;

        private void OnEnable()
        {
            GameController.Instance.RewardVideoManager.OnCloseWatch = OnFinishWatch;

            _helperText.text = "Показать букву\nза просмотр рекламы";
            _helperButton.interactable = true;
            
            ShowCloseButton();
            StartCoroutine(CheckEthernetConnection());
            StartCoroutine(StartLoadVideo());
        }

        private IEnumerator StartLoadVideo()
        {
            GameController.Instance.RewardVideoManager.TryLoadVideo();
            
            while (!GameController.Instance.RewardVideoManager.IsVideoLoaded)
            {
                yield return null;
            }

            ShowOpenButton();
        }

        private void ShowOpenButton()
        {
            _helperButton.onClick.RemoveAllListeners();
            _helperButton.onClick.AddListener(GameController.Instance.RewardVideoManager.Show);
            
            _helperButton.image.color = Color.white;
        }

        private void ShowCloseButton()
        {
            _helperButton.onClick.RemoveAllListeners();
            _helperButton.onClick.AddListener(() =>
            {
                var text = _isHaveEthernet ?  "Идет загрузка рекламы" : "Нет подключения к интернету";
                GameController.Instance.MessageWindow.MessageText = text;
                GameController.Instance.MessageWindow.gameObject.SetActive(true);
            });
            
            _helperButton.image.color = Color.grey;
        }

        private void OnFinishWatch()
        {
            if (GameController.Instance.RewardVideoManager.IsNeedReward)
            {
                GameController.Instance.RewardVideoManager.RewardComplete();
                GameController.Instance.ShowOneCorrectLetter();
            }
            else
            {
                GameController.Instance.RewardVideoManager.TryLoadVideo();
            }
            ShowCloseButton();
            StartCoroutine(StartTiner());
        }

        private IEnumerator StartTiner()
        {
            _helperButton.interactable = false;
            GameController.Instance.RewardVideoManager.TryLoadVideo();
            
            var currentTimerValue = _timerSecondsCount;
            while (currentTimerValue != 0)
            {
                _helperText.text = string.Format("Подсказка будет\nдоступна через {0} ...", currentTimerValue);
                yield return new WaitForSeconds(1);
                currentTimerValue--;
            }

            _helperText.text = "Показать букву\nза просмотр рекламы";
            _helperButton.interactable = true;
            yield return StartLoadVideo();
        }

        private IEnumerator CheckEthernetConnection()
        {
            while (true)
            {
                yield return new WaitForSeconds(3);
            
                var www = new WWW("https://www.google.com/");

                yield return www;

               var isHaveEthernet = string.IsNullOrEmpty(www.error);

                if (_isHaveEthernet != isHaveEthernet)
                {
                    var text = isHaveEthernet ?  "Идет загрузка рекламы" : "Нет подключения к интернету";
                    GameController.Instance.MessageWindow.MessageText = text;
                }
                
                _isHaveEthernet = isHaveEthernet;
            }   
        }
    }
}