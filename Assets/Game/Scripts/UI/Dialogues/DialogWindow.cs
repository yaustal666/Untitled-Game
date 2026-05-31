using Ink.Runtime;
using Reflex.Attributes;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueWindow : MonoBehaviour
{
    public event Action DialogueStarted;
    public event Action DialogueEnded;

    [Inject] DialogueSystem _dialogueSystem;

    [SerializeField] private GameObject windowContainer;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private Transform choicesContainer;
    [SerializeField] private Button choiceButtonPrefab;

    private List<Button> _activeButtons = new List<Button>();

    private void Start()
    {
        _dialogueSystem.EnterDialogue += () =>
        {
            windowContainer.SetActive(true);
            DialogueStarted?.Invoke();
        };

        _dialogueSystem.OnTextReceived += ShowText;
        _dialogueSystem.OnChoicesReceived += ShowChoices;
        _dialogueSystem.ExitDialogue += CloseWindow;
    }

    private void Update()
    {
        if (windowContainer.activeSelf && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            _dialogueSystem.OnPlayerPressedNext();
        }
    }

    public void ShowText(string text)
    {
        dialogueText.text = text;

        ClearChoices();
    }

    public void ShowChoices(List<Choice> choices)
    {
        ClearChoices();

        for (int i = 0; i < choices.Count; i++)
        {
            Choice choice = choices[i];
            int index = i;

            Button buttonInstance = Instantiate(choiceButtonPrefab, choicesContainer);
            _activeButtons.Add(buttonInstance);

            TextMeshProUGUI buttonText = buttonInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = choice.text;
            }

            buttonInstance.onClick.AddListener(() =>
            {
                _dialogueSystem.OnPlayerSelectedChoice(index);
            });
        }
    }

    public void CloseWindow()
    {
        dialogueText.text = " ";
        ClearChoices();
        windowContainer.SetActive(false);
        DialogueEnded?.Invoke();
    }

    private void ClearChoices()
    {
        foreach (Button button in _activeButtons)
        {
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                Destroy(button.gameObject);
            }
        }
        _activeButtons.Clear();
    }
}
