using Controller;
using Source.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.View
{
    public class LevelItemView : MonoBehaviour
    {
        [SerializeField, Tooltip("Level item button")]
        private Button _levelButton;

        [SerializeField, Tooltip("Complete stars")]
        private Image[] _starImages;

        [SerializeField, Tooltip("Level number text")]
        private Text _levelNumberText;

        [SerializeField, Tooltip("Complete questions text")]
        private Text _progressText;

        [SerializeField, Tooltip("On game object in close items")]
        private GameObject _close;

        public void Init(QuestionPackage package, int starCount, bool isOpen = true)
        {
            _levelNumberText.text = package.Id;
            _progressText.text =
                string.Format("{0}/{1}", package.CompleteQuestionsCount, package.QuestionModels.Length);

            if (starCount < 0 || starCount > 3)
                starCount = 3;

            for (int i = 0; i < starCount; i++)
            {
                _starImages[i].gameObject.SetActive(false);
            }

            if (!isOpen)
            {
                _levelButton.interactable = false;
                _levelButton.image.color = Color.grey;
                _close.SetActive(true);
            }
            else
            {
                _levelButton.onClick.RemoveAllListeners();
                _levelButton.onClick.AddListener(() =>
                {
                    
                    AudioController.Instance.PlayClickButton();
                    GameController.Instance.OnPackageClick(package);
                });
            }
        }
    }
}