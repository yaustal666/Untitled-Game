using Reflex.Attributes;
using UnityEngine;

public class UIRoot : MonoBehaviour
{
    [Inject] private InputReader _inputReader;
    [Inject] private Game _game;

    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private HUD _HUD;
    [SerializeField] private PlayerMenu _playerMenu;
    [SerializeField] private DialogueWindow _dialodueWindow;

    private bool _windowOpened;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _dialodueWindow.DialogueStarted += OpenDialogue;
        _dialodueWindow.DialogueEnded += CloseWindow;
    }

    private void Start()
    {
        Debug.Log(_game);
        _inputReader.PausePressed += OnPausePressed;
        _inputReader.InventoryPressed += OpenInventory;
        _inputReader.QuestPressed += OpenQuestWindow;
        _inputReader.MaskPressed += OpenUpgradeMask;

        _pauseMenu.ResumePressed += CloseWindow;
        _inputReader.UnPausePressed += CloseWindow;
    }

    private void OnPausePressed()
    {
        if (_windowOpened)
        {
            CloseWindow();
        }
        else
        {
            OpenPause();
        }
    }

    public void OpenPause()
    {
        if (!_windowOpened)
        {
            _game.Pause(true);
            _pauseMenu.gameObject.SetActive(true);
            _windowOpened = true;
        }
    }

    public void OpenInventory()
    {
        if (!_windowOpened)
        {
            _playerMenu.gameObject.SetActive(true);
            _game.Pause(true);
            _playerMenu.ShowInventory();
            _windowOpened = true;
        }
    }

    public void OpenQuestWindow()
    {
        if (!_windowOpened)
        {
            _playerMenu.gameObject.SetActive(true);
            _game.Pause(true);
            _playerMenu.ShowQuestWindow();
            _windowOpened = true;
        }
    }

    public void OpenUpgradeMask()
    {
        if (!_windowOpened)
        {
            _playerMenu.gameObject.SetActive(true);
            _game.Pause(true);
            _playerMenu.ShowUpgradeMask();
            _windowOpened = true;
        }
    }

    public void OpenDialogue()
    {
        if (!_windowOpened)
        {
            _game.Pause(true);
            _dialodueWindow.gameObject.SetActive(true);
            _windowOpened = true;
        }
    }

    public void CloseWindow()
    {
        _pauseMenu.gameObject.SetActive(false);

        _playerMenu.CloseCurrentWindow();
        _playerMenu.gameObject.SetActive(false);

        _dialodueWindow.gameObject.SetActive(false);

        _game.Pause(false);
        _windowOpened = false;
    }

    public void EnableHUD(bool enabled)
    {
        _HUD.gameObject.SetActive(enabled);
    }

    private void OnDestroy()
    {
        _dialodueWindow.DialogueStarted -= OpenDialogue;
        _dialodueWindow.DialogueEnded -= CloseWindow;

        _inputReader.PausePressed -= OnPausePressed;
        _inputReader.InventoryPressed -= OpenInventory;

        _pauseMenu.ResumePressed -= CloseWindow;
        _inputReader.UnPausePressed -= CloseWindow;
    }
}