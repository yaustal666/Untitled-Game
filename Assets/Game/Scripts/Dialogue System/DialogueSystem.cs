using Cysharp.Threading.Tasks;
using Ink.Runtime;
using Reflex.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : ISavable, IDisposable
{
    [Inject] private QuestSystem _questSystem;

    public event Action<WindowType> EnterDialogue;
    public event Action<string> OnTextReceived;
    public event Action<List<Choice>> OnChoicesReceived;
    public event Action ExitDialogue;

    private Story _story;
    private LocalizationSystem _loc;
    private DialogueChannel _channel;
    private bool _isDialogueActive = false;

    public DialogueSystem(DialogueChannel channel, ISaveRegistry saveRegistry, LocalizationSystem loc)
    {
        _loc = loc;
        _channel = channel;
        _channel.DialogRequest += StartDialogue;
        saveRegistry.Register(this);
    }

    public void Dispose()
    {
        UnbindExternalFunctions();
        _channel.DialogRequest -= StartDialogue;
    }

    public async UniTask Initialize()
    {
        var storyText = await _loc.GetStoryAssetAsync();
        _story = new Story(storyText.text);
        BindExternalFunctions();
    }

    public async UniTask ReloadStory()
    {
        UnbindExternalFunctions();
        var save = _story.state.ToJson();
        var storyText = await _loc.GetStoryAssetAsync();
        _story = new Story(storyText.text);
        _story.state.LoadJson(save);
        BindExternalFunctions();
    }

    public void StartDialogue(string knotName)
    {
        Debug.Log("START DIALOGUE");
        if (_isDialogueActive) return;
        _isDialogueActive = true;

        EnterDialogue?.Invoke(WindowType.Dialogue);
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

    private void EndDialogue()
    {
        _isDialogueActive = false;
        ExitDialogue?.Invoke();
    }

    private void BindExternalFunctions()
    {
        _story.BindExternalFunction("startQuest", (string questId) =>
        {
            _questSystem.ActivateQuest(questId);
        });

        _story.BindExternalFunction("getQuestStatus", (string questId) =>
        {
            return _questSystem.GetQuestStatus(questId);
        });
    }

    private void UnbindExternalFunctions()
    {
        _story?.UnbindExternalFunction("startQuest");
        _story?.UnbindExternalFunction("getQuestStatus");
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

    public void Save(GameSaveData saveData)
    {
        saveData.DialogueData.StoryState = _story.state.ToJson();
    }

    public void Load(GameSaveData saveData)
    {
        _story.state.LoadJson(saveData.DialogueData.StoryState);
    }
}
