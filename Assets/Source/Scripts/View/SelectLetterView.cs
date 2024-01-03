using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class SelectLetterView : MonoBehaviour
    {

        [SerializeField]
        private Text _letterText;

        [SerializeField]
        private Image _letterImage;

        public string LetterText
        {
            set { _letterText.text = value; }
            get { return _letterText.text; }
        }

        public Color LetterColor
        {
            set { _letterImage.color = value; }
        }

        public void OnClick()
        {
            GameController.Instance.OnClickSelectLetter(this);
        }
    }
}