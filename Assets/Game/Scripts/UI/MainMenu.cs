using Reflex.Attributes;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Inject] private Game _game;
    [Inject] private SaveSystem _saveSystem;

    [SerializeField] private Button continueButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        if (!_saveSystem.HasSaveFile)
        {
            continueButton.gameObject.SetActive(false);
        }
        else
        {
            continueButton.gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        startButton.onClick.AddListener(OnStartClicked);
        exitButton.onClick.AddListener(OnExitClicked);
        continueButton.onClick.AddListener(OnContinueClicked);
    }

    private void OnContinueClicked()
    {
        _game.ContinueGame();
    }

    public void OnStartClicked()
    {
        _game.StartNewGame();
    }

    public void OnExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
