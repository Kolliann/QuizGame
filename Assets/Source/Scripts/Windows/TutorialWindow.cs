using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Source.Scripts.Windows
{
    public class TutorialWindow : GameWindowBase
    {
        [SerializeField, Tooltip("On click mask button")]
        private Button _maskButton;

        [SerializeField, Tooltip("On click mask button")]
        private Button _fadeButton;
        
        [SerializeField, Tooltip("On click mask button")]
        private Button _skipTutorialButton;
        
        [SerializeField, Tooltip("Tutorial arrow")]
        private Transform _arrowObject;

        [SerializeField, Tooltip("Tutorial dialog box object")]
        private Transform _dialogBoxObject;

        [SerializeField, Tooltip("Tutorial mask object")]
        private Transform _maskObject;

        [SerializeField, Tooltip("Tutorial dialog text")]
        private Text _dialogText;

        public Transform TransformFollow { get; set; }

        private void Update()
        {
            if (TransformFollow == null) 
                return;
            var position = TransformFollow.position;
            
            _maskObject.position = position;
            _maskObject.localPosition += _offsetMask;

            var transform1 = _maskButton.transform;
            transform1.position = position;
            transform1.localPosition += _offsetMask;
            
            _dialogBoxObject.position = position;
            _dialogBoxObject.localPosition += _offsetBox;
            
            
            _arrowObject.position = position;
            _arrowObject.localPosition += _offsetArrow;
        }

        private Vector3 _offsetBox;
        private Vector3 _offsetMask;
        private Vector3 _offsetArrow;
        
        public void Init(DialogBoxSetting boxSetting, MaskSetting maskSetting, ArrowSetting arrowSetting, UnityAction onClick, UnityAction onFadeClick = null, UnityAction onSkipClick = null)
        {
            if (boxSetting.IsShowed)
            {
                _dialogBoxObject.gameObject.SetActive(true);
                _offsetBox = boxSetting.PositionOffset;
                _dialogBoxObject.localScale = boxSetting.Scale;
                _dialogText.text = boxSetting.Text;
            }
            else
            {
                _dialogBoxObject.gameObject.SetActive(false);
            }
            
            if (maskSetting.IsShowed)
            {
                _maskObject.gameObject.SetActive(true);
                _offsetMask = maskSetting.PositionOffset;
                _maskObject.localScale = maskSetting.Scale;

                var btnTransform = _maskButton.transform;
                btnTransform.localPosition = maskSetting.PositionOffset;
                btnTransform.localScale = maskSetting.Scale;
            }
            else
            {
                _maskObject.gameObject.SetActive(false);
                _maskButton.gameObject.SetActive(false);
            }
            
            if (arrowSetting.IsShowed)
            {
                _arrowObject.gameObject.SetActive(true);
                _offsetArrow = arrowSetting.PositionOffset;
            }
            else
            {
                _arrowObject.gameObject.SetActive(false);
            }

            if (onFadeClick == null)
            {
                _fadeButton.gameObject.SetActive(false);
            }
            else
            {
                _fadeButton.gameObject.SetActive(true);
                _fadeButton.onClick.RemoveAllListeners();
                _fadeButton.onClick.AddListener(onFadeClick);
            }
            
            if (onSkipClick == null)
            {
                _skipTutorialButton.gameObject.SetActive(false);
            }
            else
            {
                _skipTutorialButton.gameObject.SetActive(true);
                _skipTutorialButton.onClick.RemoveAllListeners();
                _skipTutorialButton.onClick.AddListener(onSkipClick);
            }
            
            _maskButton.onClick.RemoveAllListeners();
            _maskButton.onClick.AddListener(onClick);
            Update();
        }
        
        
        public void Clear()
        {
            _arrowObject.gameObject.SetActive(false);
            _maskObject.gameObject.SetActive(false);
            _dialogBoxObject.gameObject.SetActive(false);
            TransformFollow = null;
        }

        public struct DialogBoxSetting
        {
            public bool IsShowed;
            public Vector2 PositionOffset;
            public Vector2 Scale;
            public string Text;
        }

        public struct MaskSetting
        {
            public bool IsShowed;
            public Vector2 PositionOffset;
            public Vector2 Scale;
        }
        
        public struct ArrowSetting
        {
            public bool IsShowed;
            public Vector2 PositionOffset;
        }

        
    }
}