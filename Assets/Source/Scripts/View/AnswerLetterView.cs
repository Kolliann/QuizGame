using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class AnswerLetterView : MonoBehaviour
    {
        [SerializeField] private Text _letterText;
        
        [SerializeField] private Text _cursor;

        public Text Cursor
        {
            get { return _cursor; }
        }

        public string LetterText
        {
            get { return _letterText.text; }
            set { _letterText.text = value; }
        }

        public void OnClick()
        {
            GameController.Instance.OnClickAnswerLetter(this);
        }
    }
}