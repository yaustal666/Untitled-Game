using UnityEngine;

public class PlayerMenu : UIWindow
{
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private MaskView _maskUpgradeWindow;
    [SerializeField] private QuestWindow _questWindow;

    private UIWindow _currentWindow;
    private void Awake()
    {
        _inventoryView.Close();
        _maskUpgradeWindow.Close();
        _questWindow.Close();
    }

    public void ShowInventory()
    {
        CloseCurrentWindow();
        _currentWindow = _inventoryView;
        _currentWindow.Open();
        _inventoryView.Refresh();
    }

    public void ShowUpgradeMask()
    {
        CloseCurrentWindow();
        _currentWindow = _maskUpgradeWindow;
        _currentWindow.Open();
        //_maskUpgradeWindow.Refresh();
    }

    public void ShowQuestWindow()
    {
        CloseCurrentWindow();
        _currentWindow = _questWindow;
        _currentWindow.Open();
        //_questWindow.Refresh();
    }

    public void CloseCurrentWindow()
    {
        _currentWindow?.Close();
    }
}