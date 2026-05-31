using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Loading,
    Gameplay,
    Paused
}

public class Game
{
    [Inject] private InputReader _inputReader;
    [Inject] private SaveSystem _saveSystem;

    public GameState CurrentState { get; private set; }

    private SceneLoader _sceneLoader;
    private UIRoot _uiRoot;
    private Container _rootContainer;
    private Container _sessionContainer;

    public Game(Container rootContainer)
    {
        _sceneLoader = new SceneLoader();
        _rootContainer = rootContainer;
    }

    public void ToMainMenu()
    {
        ChangeState(GameState.MainMenu);
        _sceneLoader?.LoadScene("MainMenu");
    }

    public async UniTask InitializeGame()
    {
        var sessionInstaller = new SessionInstaller();
        _sessionContainer = await sessionInstaller.InstallGameSession(_rootContainer);

        var gameSettings = _sessionContainer.Resolve<GameSettings>();
        var uiRootObject = Object.Instantiate(gameSettings.UIRootPrefab);
        GameObjectInjector.InjectRecursive(uiRootObject, _sessionContainer);
        _uiRoot = uiRootObject.GetComponent<UIRoot>();
    }

    public async void StartNewGame()
    {
        await InitializeGame();

        await _sceneLoader.LoadScene("TestSceneTD", _sessionContainer);
        ChangeState(GameState.Gameplay);
    }

    public async void ContinueGame()
    {
        await InitializeGame();

        _saveSystem.LoadGame();

        await _sceneLoader.LoadScene("TestSceneTD", _sessionContainer);
        ChangeState(GameState.Gameplay);
    }

    public void ExitToMainMenu()
    {
        Object.Destroy(_uiRoot.gameObject);
        _saveSystem.SaveGame();
        _sessionContainer.Dispose();
        _sceneLoader?.LoadScene("MainMenu");
        ChangeState(GameState.MainMenu);
    }

    public void Pause(bool pause)
    {
        if (pause)
        {
            ChangeState(GameState.Paused);
        }
        else
        {
            ChangeState(GameState.Gameplay);
        }
    }

    public void SceneTransition(string destination)
    {
        _sceneLoader?.LoadScene(destination, _sessionContainer);
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        EnterState(CurrentState);
        Debug.Log($"Game state changed to: {CurrentState}");
    }

    private void EnterState(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;

            case GameState.Gameplay:
                _inputReader.EnablePlayerActionMap(true);
                _inputReader.EnableUIActionMap(false);
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;

            case GameState.Paused:
                _inputReader.EnablePlayerActionMap(false);
                _inputReader.EnableUIActionMap(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
        }
    }
}