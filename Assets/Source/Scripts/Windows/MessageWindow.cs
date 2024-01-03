using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Windows
{
    public class MessageWindow : MonoBehaviour
    {
        [SerializeField]
        private Text _messageText;

        public string MessageText
        {
            set { _messageText.text = value; }
        }
    }
}