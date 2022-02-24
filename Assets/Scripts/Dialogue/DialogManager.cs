using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DialogManager : MonoBehaviour
{
	struct Dialog {
		public string sentence;
		public string[] choices;
    }

	[SerializeField]
	private GameObject choiceCanvas = null;
	[SerializeField]
	private TextMeshProUGUI contentText;

	/// <summary>
	/// Enable/Disable to give auto scale to the chat bubble depend on the text inside it
	/// </summary>
	[SerializeField] bool IsWorldCanvas = true;

	// UI Prefabs
	[SerializeField]
	private Button buttonPrefab = null;

	/// <summary>
	/// Array of dialog that will be filled in the inspector to define the story
	/// </summary>
	[SerializeField, Space(10)] Dialog[] dialogs;

	int currentDialogIndex;

    private void Start() {
		currentDialogIndex = 0;
    }

    void RefreshView() {
		// Remove all the UI on screen
		RemoveChildren();

		// Read all the content until we can't continue any more
		if (currentDialogIndex > dialogs.Length) {
			// Continue gets the next line of the story
			string text = dialogs[currentDialogIndex].sentence;
			// This removes any white space from the text.
			text = text.Trim();
			// Display the text on screen!
			CreateContentView(text);
		}

		// Display all the choices, if there are any!
		if (dialogs[currentDialogIndex].choices.Length > 0) {
			for (int i = 0; i < dialogs[currentDialogIndex].choices.Length; i++) {
				Button button = CreateChoiceView(dialogs[currentDialogIndex].choices[i].Trim());
				// Tell the button what to do when we press it
				//button.onClick.AddListener(delegate {
				//	OnClickChoiceButton(choice);
				//});
			}
		}
		// If we've read all the content and there's no choices, the story is finished!
		else {
			Button choice = CreateChoiceView("OK");
			//choice.onClick.AddListener(delegate {
			//	StartStory();
			//});
		}
	}

	//void OnClickChoiceButton(Choice choice) {
	//	story.ChooseChoiceIndex(choice.index);
	//	RefreshView();
	//}

	void CreateContentView(string text) {
		StartCoroutine(TypeSentence(contentText, text));
	}

	Button CreateChoiceView(string text) {
		// Creates the button from a prefab
		Button choice = Instantiate(buttonPrefab) as Button;
		choice.transform.SetParent(choiceCanvas.transform, false);

		// Resize collider to match with the button
		if (IsWorldCanvas) {
			float tmpWidth = choice.GetComponent<LayoutElement>().preferredWidth;
			float tmpHeight = choice.GetComponent<LayoutElement>().preferredHeight;

			choice.GetComponent<BoxCollider>().size = new Vector3(tmpWidth, tmpHeight, .1f);
		}

		// Gets the text from the button prefab
		TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();
		StartCoroutine(TypeSentence(choiceText, text));

		return choice;
	}

	// Destroys all the children of this gameobject (all the UI)
	void RemoveChildren() {
		int childCount = choiceCanvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i) {
			GameObject.Destroy(choiceCanvas.transform.GetChild(i).gameObject);
		}
	}

	IEnumerator TypeSentence(TextMeshProUGUI textObject, string sentence) {
		textObject.text = "";
		foreach (char letter in sentence.ToCharArray()) {
			textObject.text += letter;
			yield return null;
		}

		// Use animations to the Character
		//OnDialogueAnimation tempSpeaker = GameObject.FindObjectOfType<OnDialogueAnimation>();
		//if (tempSpeaker.isTalking) {
		//	SetAnimation("Idle");
		//}
		yield return null;
	}
}
