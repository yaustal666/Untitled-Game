using Ink.Runtime;
using Reflex.Attributes;
using System;
using System.Collections.Generic;

public class DialogueSystem : ISavable
{
    [Inject] private QuestSystem _questSystem;

    public event Action EnterDialogue;
    public event Action<string> OnTextReceived;
    public event Action<List<Choice>> OnChoicesReceived;
    public event Action ExitDialogue;

    private DialogueChannel _channel;

    private Story _story;
    private bool _isDialogueActive = false;

    public DialogueSystem(GameSettings settings, DialogueChannel channel, ISaveRegistry saveRegistry)
    {
        _channel = channel;
        _story = new Story(settings.Dialogs.text);
        _channel.DialogRequest += StartDialogue;

        _story.BindExternalFunction("startQuest", (string questId) =>
        {
            _questSystem.ActivateQuest(questId);
        });

        _story.BindExternalFunction("getQuestStatus", (string questId) =>
        {
            return _questSystem.GetQuestStatus(questId);
        });

        _story.BindExternalFunction("completeQuest", (string questId) =>
        {
            _questSystem.CompleteQuest(questId);
        });

        saveRegistry.Register(this);
    }

    public void StartDialogue(string knotName)
    {
        if (_isDialogueActive) return;

        _isDialogueActive = true;
        EnterDialogue?.Invoke();
        _story.ChoosePathString(knotName);
        AdvanceDialogue();
    }

    private void AdvanceDialogue()
    {
        if (_story.canContinue)
        {
            string currentText = _story.Continue();
            OnTextReceived?.Invoke(currentText);

            if (_story.currentChoices.Count > 0)
            {
                OnChoicesReceived?.Invoke(_story.currentChoices);
            }
        }
        else
        {
            EndDialogue();
        }
    }

    public void OnPlayerPressedNext()
    {
        if (_story.currentChoices.Count > 0) return;
        AdvanceDialogue();
    }

    public void OnPlayerSelectedChoice(int choiceIndex)
    {
        _story.ChooseChoiceIndex(choiceIndex);
        AdvanceDialogue();
    }

    private void EndDialogue()
    {
        _isDialogueActive = false;
        ExitDialogue?.Invoke();
    }

    public void Save(GameSaveData saveData)
    {
        saveData.DialogueData.StoryState = _story.state.ToJson();
    }

    public void Load(GameSaveData saveData)
    {
        _story.state.LoadJson(saveData.DialogueData.StoryState);
    }
}
