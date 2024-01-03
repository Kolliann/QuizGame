using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Windows
{
	public class EnterNameWindow : GameWindowBase
	{
		[SerializeField, Tooltip("Input field for enter user name")]	
		private InputField _enterNameField;

		[SerializeField, Tooltip("Text for show enter name error")]
		private Text _errorText;

		[SerializeField, Tooltip("Save user name button")]
		private Button _saveButton;

		public Action<string> OnChangeNameAction;

		private void Update()
		{
			if (_enterNameField.text.Length > 3 && _enterNameField.text.Length < 20)
			{
				_saveButton.image.color = Color.white;
			}
			else
			{
				_saveButton.image.color = Color.grey;
			}
				
		}

		private void OnEnable()
		{
			_errorText.text = string.Empty;
			_enterNameField.text = string.Empty;
			_saveButton.image.color = Color.grey;
			
			_saveButton.onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			if (_enterNameField.text.Length <= 1)
			{
				_errorText.text = "Имя слишком короткое";
			}
			else if (_enterNameField.text.Length >= 20)
			{
				_errorText.text = "Имя слишком длинное";
			}
			else
			{
				// save
				
				if(OnChangeNameAction != null)
					OnChangeNameAction.Invoke(_enterNameField.text);
			}
		}
	}
}
