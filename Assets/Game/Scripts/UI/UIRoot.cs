using System;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot : MonoBehaviour
{
    public event Action WindowClosed;
    public event Action MainMenuPressed;

    public bool IsWindowOpened => _windowStack.Count > 0;
    public UIWindow ActiveWindow => _windowStack.Peek();

    [SerializeField] private List<UIWindow> _windows = new();
    private Dictionary<Type, UIWindow> _mappingTypeToWindows = new();
    private Stack<UIWindow> _windowStack = new();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        foreach (var window in _windows)
        {
            var windowType = window.GetType();
            _mappingTypeToWindows.Add(windowType, window);
            window.Close();
        }
    }

    public void Initialize()
    {
        foreach(var window in _windows)
        {
            window.Initialize();
        }
    }

    public void OpenWindow<T>(Action<T> setup = null) where T : UIWindow
    {
        UIWindow window;
        if (!_mappingTypeToWindows.TryGetValue(typeof(T), out window))
        {
            Debug.LogError("Requested non existent window Type");
            return;
        }

        if (setup != null)
        {
            setup.Invoke((T)window);
        }

        window.Open();
        _windowStack.Push(window);
    }

    public void CloseWindow()
    {
        var window = _windowStack.Pop();
        window.Close();
        WindowClosed.Invoke();
    }

    public void ExitToMainMenu()
    {
        MainMenuPressed?.Invoke();
    }
}