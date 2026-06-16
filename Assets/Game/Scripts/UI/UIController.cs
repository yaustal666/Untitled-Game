using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class UIController : IDisposable
{
    private readonly Game _game;
    private readonly InputReader _input;
    private readonly DialogueSystem _dialogueSystem;
    private readonly UIRoot _ui;
    private GameEvents _events;

    public UIController(Game game, InputReader inputReader, DialogueSystem dialogueSystem, UIRoot uiRoot, GameEvents events)
    {
        _game = game;
        _input = inputReader;
        _dialogueSystem = dialogueSystem;
        _ui = uiRoot;
        _events = events;

        _input.WindowOpenPressed += OpenWindow;
        _input.CloseWindowPressed += CloseWindow;
        _dialogueSystem.ExitDialogue += CloseDialogue;
        _dialogueSystem.EnterDialogue += OpenWindow;
        _ui.WindowClosed += Unpause;
        _ui.MainMenuPressed += ExitToMainMenu;

        _events.Subscribe<HideHUDMessage>(HideHUD);
    }

    private void HideHUD(HideHUDMessage message)
    {
        HUD hud = (HUD) _ui.GetWindow<HUD>();
        HideAndShowHUD(hud, message.milliseconds).Forget();
    }

    public async UniTask HideAndShowHUD(HUD hud, int ms)
    {
        hud.gameObject.SetActive(false);
        await UniTask.Delay(ms, ignoreTimeScale: true);
        hud.gameObject.SetActive(true);
    }

    public void Dispose()
    {
        _input.WindowOpenPressed -= OpenWindow;
        _input.CloseWindowPressed -= CloseWindow;
        _dialogueSystem.ExitDialogue -= CloseDialogue;
        _dialogueSystem.EnterDialogue -= OpenWindow;
        _ui.WindowClosed -= Unpause;
        _ui.MainMenuPressed -= ExitToMainMenu;
    }

    private void OpenWindow(WindowType type)
    {
        if (_ui.IsWindowOpened)
        {
            return;
        }

        switch (type)
        {
            case WindowType.Inventory:
                _ui.OpenWindow<PlayerMenu>(window =>
                {
                    Debug.Log("INVENTORY");
                    window.ShowInventory();
                });
                break;
            case WindowType.Quest:
                _ui.OpenWindow<PlayerMenu>(window =>
                {
                    window.ShowQuestWindow();
                });
                break;
            case WindowType.MaskUpgrade:
                _ui.OpenWindow<PlayerMenu>(window =>
                {
                    window.ShowUpgradeMask();
                });
                break;
            case WindowType.Dialogue:
                _ui.OpenWindow<DialogueWindow>(window =>
                {

                });
                break;
            case WindowType.Pause:
                _ui.OpenWindow<PauseMenu>(window =>
                {

                });
                break;
            default:
                break;
        }

        if (_ui.IsWindowOpened)
        {
            _game.Pause(true);
        }
    }

    private void CloseWindow()
    {
        if (_ui.ActiveWindow is DialogueWindow) return;
        _ui.CloseWindow();
    }

    private void CloseDialogue()
    {
        Debug.Log("EndDial");
        if (_ui.ActiveWindow is DialogueWindow)
        {
            _ui.CloseWindow();
        }
    }

    private void Unpause()
    {
        _game.Pause(false);
    }

    public void ExitToMainMenu()
    {
        _game.ExitToMainMenu();
    }
}