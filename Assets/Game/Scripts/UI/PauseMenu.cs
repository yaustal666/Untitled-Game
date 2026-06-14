using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : UIWindow
{
    [Inject] private UIRoot _ui;

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
        _ui.CloseWindow();
    }

    private void ExitToMainMenu()
    {
        _ui.ExitToMainMenu();
    }
}