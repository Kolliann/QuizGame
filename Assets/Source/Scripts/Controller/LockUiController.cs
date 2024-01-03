using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Controller
{
    public class LockUiController : MonoBehaviour
    {
        [SerializeField, Tooltip("Image for lock UI")]
        private Image _lockImage;

        #region Singleton

        private static LockUiController _instance;

        public static LockUiController Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = FindObjectOfType<LockUiController>();

                return _instance != null ? _instance : null;
            }
        }

        private LockUiController()
        {
        }

        #endregion


        public void Lock()
        {
            _lockImage.raycastTarget = true;
        }
        
        public void Unlock()
        {
            _lockImage.raycastTarget = false;
        }
    }
}