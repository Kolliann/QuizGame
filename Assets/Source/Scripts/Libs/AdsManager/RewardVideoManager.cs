using System;
using System.Collections.Generic;
using System.Linq;
using Source.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Libs.AdsManager
{
    public class RewardVideoManager : MonoBehaviour
    {    
        [SerializeField] private GameObject[] _componentObjects;
        private List<IVideoRewardComponent> _components;

        public void Init()
        {
            _components = new List<IVideoRewardComponent>();
            foreach (var componentObject in _componentObjects)
            {
                var component = componentObject.GetComponent<IVideoRewardComponent>();
                if (component == null)
                {
                    Debug.LogError(string.Format("Error initialize - {0} object don't have IVideoRewardComponent", 
                        componentObject.name));
                    continue;
                }
                
                _components.Add(component);
            }

            foreach (var component in _components)
            {
                component.Init();
            }
        }

        public void TryLoadVideo()
        {
            foreach (var component in _components)
            {
                component.TryLoad();
            }
        }

        public bool IsNeedReward
        {
            get { return _components.Any(component => component.IsNeedReward); }
        }

        public void RewardComplete()
        {
            foreach (var component in _components)
            {
                component.IsNeedReward = false;
            }
        }

        public Action OnCloseWatch
        {
            set
            {
                Action onClose = () =>
                {
                    LockUiController.Instance.Unlock();
                    if (value != null)
                        value.Invoke();
                };

                foreach (var component in _components)
                {
                    component.OnCloseWatch = onClose;
                }
            }
        }

        public bool IsVideoLoaded
        {
            get { return _components.Any(component => component.IsReady); }
        }

        public void Show()
        {
            foreach (var component in _components)
            {
                if(component.IsShowing)
                    return;
            }

            foreach (var component in _components)
            {
                if (!component.IsReady) 
                    continue;
                LockUiController.Instance.Lock();
                component.Show();
                break;
            }
        }
        
    }
}