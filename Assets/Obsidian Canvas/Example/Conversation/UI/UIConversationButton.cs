using System;
using ObsidianCanvas.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Obsidian_Canvas.Example.Conversation.UI
{
	public class UIConversationButton : MonoBehaviour
	{
		private Button _button;
		private ConversationRunner _runner;
		private Port _port;
		private TMP_Text _buttonDisplayText;
		private void Awake()
		{
			_button = GetComponent<Button>();
			_button.onClick.AddListener(OnClick);
			_buttonDisplayText = GetComponentInChildren<TMP_Text>();
		}

		public void Init(ConversationRunner runner, Port port)
		{
			_runner = runner;
			_port = port;
			_buttonDisplayText.text = _port.Label;
			//resize button...
		}

		void OnClick()
		{
			_runner.OptionSelected(_port);
		}
	}
}