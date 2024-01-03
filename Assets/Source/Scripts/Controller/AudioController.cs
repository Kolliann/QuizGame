using System;
using UnityEngine;
using UnityEngine.UI;

namespace Controller
{
	public class AudioController : MonoBehaviour
	{
		#region Singleton

		private static AudioController _instance;

		public static AudioController Instance
		{
			get
			{
				if (_instance != null)
					return _instance;

				_instance = FindObjectOfType<AudioController>();

				return _instance != null ? _instance : null;
			}
		}

		private AudioController()
		{
		}

		#endregion

		[SerializeField]
		private AudioSource _mainTheme;
		
		[SerializeField]
		private AudioSource _buttonClick;
		
		[SerializeField]
		private AudioClip _buttonClip;

		[SerializeField]
		private Sprite _offSoundSprite;
		[SerializeField]
		private Sprite _onSoundSprite;

		[SerializeField]
		private Image _soundButtonImage;
		[SerializeField]
		private Text _soundButtonText;

		private bool _isOn = true;

		private void Awake()
		{
			var key = PlayerPrefs.GetInt("PlaySoundSetting");
			if (key == 1)
			{
				OnMainMusic();
			}
			else
			{
				OffMainMusic();
			}
		}

		public void PlayClickButton()
		{
			if(_isOn)
				_buttonClick.PlayOneShot(_buttonClip);
		}

		public void OffMainMusic()
		{
			_mainTheme.volume = 0;
			_soundButtonImage.sprite = _offSoundSprite;
			_soundButtonText.text = "Звук";
			
			PlayerPrefs.SetInt("PlaySoundSetting", 0);
		}

		public void OnMainMusic()
		{
			_mainTheme.volume = 1;
			_soundButtonImage.sprite = _onSoundSprite;
			_soundButtonText.text = "Звук";
			
			PlayerPrefs.SetInt("PlaySoundSetting", 1);
		}

		public void ClickSoundButton()
		{
			if (_isOn)
			{
				OffMainMusic();
				_isOn = false;
			}
			else
			{
				OnMainMusic();
				_isOn = true;
			}
		}
	
	}
}
