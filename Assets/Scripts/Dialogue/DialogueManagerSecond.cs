using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using System;
using TMPro;

public class DialogueManagerSecond : MonoBehaviour
{
	[SerializeField]
	private TextAsset inkJSONAsset = null;
	public Story story;

	[SerializeField]
	private GameObject choiceCanvas = null;
	[SerializeField]
	private TextMeshProUGUI contentText;

	// UI Prefabs
	[SerializeField]
	private Button buttonPrefab = null;

	public static event Action<Story> OnCreateStory;
	[SerializeField] bool IsWorldCanvas = true;

    private void OnEnable() {
		// Remove the default message
		RemoveChildren();
		StartStory();
	}

    // Creates a new Story object with the compiled story which we can then play!
    void StartStory() {
		story = new Story(inkJSONAsset.text);
		if (OnCreateStory != null) OnCreateStory(story);
		RefreshView();
	}

	// This is the main function called every time the story changes. It does a few things:
	// Destroys all the old content and choices.
	// Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
	void RefreshView() {
		// Remove all the UI on screen
		RemoveChildren();

		// Read all the content until we can't continue any more
		while (story.canContinue) {
			// Continue gets the next line of the story
			string text = story.Continue();
			// This removes any white space from the text.
			text = text.Trim();
			// Display the text on screen!
			CreateContentView(text);
		}

		// Display all the choices, if there are any!
		if (story.currentChoices.Count > 0) {
			for (int i = 0; i < story.currentChoices.Count; i++) {
				Choice choice = story.currentChoices[i];
				Button button = CreateChoiceView(choice.text.Trim());
				// Tell the button what to do when we press it
				button.onClick.AddListener(delegate {
					OnClickChoiceButton(choice);
				});
			}
		}
		// If we've read all the content and there's no choices, the story is finished!
		else {
			Button choice = CreateChoiceView("OK");
			choice.onClick.AddListener(delegate {
				StartStory();
			});
		}
	}

	// When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton(Choice choice) {
		story.ChooseChoiceIndex(choice.index);
		RefreshView();
	}

	// Creates a textbox showing the the line of text
	void CreateContentView(string text) {
		StartCoroutine(TypeSentence(contentText, text));
	}

	// Creates a button showing the choice text
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

	IEnumerator TypeSentence(TextMeshProUGUI textObject ,string sentence) {
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
