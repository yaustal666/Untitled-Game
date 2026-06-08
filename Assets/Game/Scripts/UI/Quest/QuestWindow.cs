using Reflex.Attributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestWindow : MonoBehaviour
{
    [Inject] private QuestSystem _questSystem;
    [Inject] private LocalizationSystem _loc;

    public Button QuestButtonPrefab;
    public GameObject _content;
    public TMP_Text questTitle;
    public TMP_Text questDescription;
    public TMP_Text questObjectives;
    public TMP_Text questRewards;

    private List<Button> _buttons = new();
    private List<Quest> _quests = new();


    private void Start()
    {
        _quests = _questSystem.GetAllQuests();

        for (int i = 0; i < _quests.Count; i++)
        {
            int index = i;

            var button = Instantiate(QuestButtonPrefab, _content.transform);
            button.onClick.AddListener(() =>
            {
                ShowQuestDetails(index);
            });

            _buttons.Add(button);

            var questInfo = _loc.GetQuestInfo(_quests[i].Id);
            button.GetComponentInChildren<TMP_Text>().text = questInfo.Title;
        }

        UpdateQuestView();
    }

    public void ShowQuestDetails(int index)
    {
        var questInfo = _loc.GetQuestInfo(_quests[index].Id);
        questTitle.text = questInfo.Title;
        questDescription.text = questInfo.Description;

        questObjectives.text = "";
        foreach (var objective in _quests[index].Objectives)
        {
            questObjectives.text += questInfo.Objectives[0] + " " + objective.CurrentAmount.ToString() + "/" + objective.RequiredAmount.ToString() + "\n";
        }
    }

    private void UpdateQuestView()
    {

    }

}