using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Windows
{
    public class WindowManager : MonoBehaviour
    {
        #region Singleton

        private static WindowManager _instance;

        public static WindowManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = FindObjectOfType<WindowManager>();

                return _instance != null ? _instance : null;
            }
        }

        private WindowManager()
        {
        }

        #endregion

        [SerializeField, Tooltip("Root for create window")]
        private Transform _windowRoot;

        [SerializeField, Tooltip("Prefab for create window enter name")]
        private EnterNameWindow _enterNameWindow;

        [SerializeField, Tooltip("Prefab for create window developer games")]
        private DeveloperGamesWindow _developerGamesWindow;
        
        [SerializeField, Tooltip("Prefab for create rating window")]
        private RatingWindow _ratingWindow;
        
        [SerializeField, Tooltip("Prefab for create tutorial window")]
        private TutorialWindow _tutorialWindow;

        /// <summary>
        /// Window Cash
        /// </summary>
        private readonly Dictionary<WindowType, GameWindowBase> _windows = new Dictionary<WindowType, GameWindowBase>();

        /// <summary>
        /// Getter for enter name window
        /// </summary>
        public EnterNameWindow GetEnterNameWindow
        {
            get
            {
                if (_windows.ContainsKey(WindowType.EnterName))
                    return (EnterNameWindow) _windows[WindowType.EnterName];

                var window = Instantiate(_enterNameWindow, _windowRoot);
                _windows.Add(WindowType.EnterName, window);

                return window;
            }
        }

        /// <summary>
        /// Getter for developer games window
        /// </summary>
        public DeveloperGamesWindow GetDeveloperGamesWindow
        {
            get
            {
                if (_windows.ContainsKey(WindowType.DeveloperGames))
                    return (DeveloperGamesWindow) _windows[WindowType.DeveloperGames];

                var window = Instantiate(_developerGamesWindow, _windowRoot);
                _windows.Add(WindowType.DeveloperGames, window);

                return window;
            }
        }
        
        /// <summary>
        /// Getter for rating window
        /// </summary>
        public RatingWindow GetRatingWindow
        {
            get
            {
                if (_windows.ContainsKey(WindowType.Rating))
                    return (RatingWindow) _windows[WindowType.Rating];

                var window = Instantiate(_ratingWindow, _windowRoot);
                _windows.Add(WindowType.Rating, window);

                return window;
            }
        }
        
        /// <summary>
        /// Getter for tutorial window
        /// </summary>
        public TutorialWindow GetTutorialWindow
        {
            get
            {
                if (_windows.ContainsKey(WindowType.Tutorial))
                    return (TutorialWindow) _windows[WindowType.Tutorial];

                var window = Instantiate(_tutorialWindow, _windowRoot);
                _windows.Add(WindowType.Tutorial, window);

                return window;
            }
        }
    }

    /// <summary>
    /// Window types
    /// </summary>
    public enum WindowType
    {
        None,
        EnterName,
        DeveloperGames,
        Rating,
        Tutorial
    }
}