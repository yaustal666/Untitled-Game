using Reflex.Attributes;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public event Action ResumePressed;
    [Inject] private Game _game;

    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _mainMenuButton;

    private void Awake()
    {
        _resumeButton.onClick.AddListener(ResumeGame);
        _mainMenuButton.onClick.AddListener(ExitToMainMenu);
    }

    private void ResumeGame()
    {
        ResumePressed?.Invoke();
    }

    private void ExitToMainMenu()
    {
        _game.ExitToMainMenu();
    }

}