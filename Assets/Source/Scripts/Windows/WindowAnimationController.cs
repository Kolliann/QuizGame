using System;
using Source.Scripts.Controller;
using UnityEngine;

namespace Source.Scripts.Windows
{
    public class WindowAnimationController : MonoBehaviour
    {
        [SerializeField, Tooltip("Window Animations")]
        private Animator _windowAnimator;

        public Action StartHideAction { get; set; }
        
        public Action FinishHideAction { get; set; }
        public Action FinishShowAction { get; set; }
        
        /// <summary>
        /// Current window animation state
        /// </summary>
        public AnimationState CurrentState { get; private set; }

        #region Animation Triggers

        public void StartShow()
        {
            CurrentState = AnimationState.Showing;
            LockUiController.Instance.Lock();
        }

        public void FinishShow()
        {
            CurrentState = AnimationState.Show;
            LockUiController.Instance.Unlock();
            
            if(FinishShowAction != null)
                FinishShowAction.Invoke();

            //FinishShowAction = null;
        }

        public void StartHide()
        {
            CurrentState = AnimationState.Hiding;
            LockUiController.Instance.Lock();
            
            if(StartHideAction != null)
                StartHideAction.Invoke();

            //StartHideAction = null;
        }

        public void FinishHide()
        {
            CurrentState = AnimationState.Hide;
            LockUiController.Instance.Unlock();
            
            if(FinishHideAction != null)
                FinishHideAction.Invoke();

            //FinishHideAction = null;
        }

        #endregion

        /// <summary>
        /// Start show window 
        /// </summary>
        public void Show()
        {
            if(CurrentState == AnimationState.Show || CurrentState == AnimationState.Showing)
                return;
            
            _windowAnimator.SetTrigger("Show");
        }

        /// <summary>
        /// Start hide window
        /// </summary>
        public void Hide()
        {
            if(CurrentState == AnimationState.Hide || CurrentState == AnimationState.Hiding)
                            return;
                        
            _windowAnimator.SetTrigger("Hide");
        }
    }


    /// <summary>
    /// Window Animation States
    /// </summary>
    public enum AnimationState
    {
        None,
        Showing,
        Show,
        Hiding,
        Hide
    }
}