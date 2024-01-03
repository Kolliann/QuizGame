using UnityEngine;

namespace Source.Scripts.Windows
{
    public class GameWindowBase : MonoBehaviour
    {
        [SerializeField, Tooltip("Window Hide and Show animation controller")]
        private WindowAnimationController _animationController;

        /// <summary>
        /// Get For Amimation Controller
        /// </summary>
        public WindowAnimationController AnimationController
        {
            get { return _animationController; }
        }
    }
}