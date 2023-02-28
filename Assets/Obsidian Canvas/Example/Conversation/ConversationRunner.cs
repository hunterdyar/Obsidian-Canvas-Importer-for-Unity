using System.Collections.Generic;
using Obsidian_Canvas.Example.Conversation.UI;
using ObsidianCanvas;
using ObsidianCanvas.Data;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

namespace Obsidian_Canvas.Example.Conversation
{
	public class ConversationRunner : MonoBehaviour
	{
		[SerializeField] private CanvasObject testConversation;

		//UI configuration
		public UIConversationButton _buttonPrefab;
		public Transform _buttonParent;
		public TMP_Text _dialogueText;
		
		//converation data
		private Node _currentNode;
		private List<Port> _currentConnections;
		private List<Node> _conversationHistory;

		private void Start()
		{
			//Presumably you would call something like StartConversation from your code.
			if (testConversation != null)
			{
				StartConversation(testConversation);
			}
		}
		
		public void StartConversation(CanvasObject conversation)
		{
			_conversationHistory = new List<Node>();
			var startNodes = conversation.GetAllStartingNodes();
			if (startNodes.Count != 1)
			{
				Debug.LogError($"Can't start conversation {conversation.name}. Must have exactly 1 starting node.");
			}
			else
			{
				if (startNodes[0] is TextNode textNode)
				{
					DisplayConversationNode(textNode);
				}
				else
				{
					Debug.LogError("Converation Start Node must be TextNode");
				}
			}
		}

		private void DisplayConversationNode(TextNode node)
		{
			_currentNode = node;
			_conversationHistory.Add(node);
			_dialogueText.text = node.Text;
			ClearButtons();
			CreateButtonsForNode(node);
		}

		private void CreateButtonsForNode(TextNode node)
		{
			//todo: sort node.Connections by node y position. Do it with a linq statement that also filters out arrow ports (incoming) 
			foreach (var port in node.Connections)
			{
				//only the "outgoing" nodes should be displayed as options.
				if (port.PortEnd == EdgeEnd.None)
				{
					var button = Instantiate(_buttonPrefab, _buttonParent);
					button.Init(this, port);
				}
			}
		}

		private void ClearButtons()
		{
			foreach (Transform child in _buttonParent)
			{
				//runtime only. Sorry, testers.
				Destroy(child.gameObject);
			}
		}

		public void OptionSelected(Port port)
		{
			if (port.ConnectedNode is TextNode nextText)
			{
				DisplayConversationNode(nextText);
			}
		}
	}
}