using UnityEngine;

public class PlayerMenu : MonoBehaviour
{
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private MaskView _maskUpgradeWindow;
    [SerializeField] private QuestWindow _questWindow;

    private GameObject _currentWindow;

    public void ShowInventory()
    {
        _currentWindow = _inventoryView.gameObject;
        _currentWindow.SetActive(true);
        _inventoryView.Refresh();
    }

    public void ShowUpgradeMask()
    {
        _currentWindow = _maskUpgradeWindow.gameObject;
        _currentWindow.SetActive(true);
        //_maskUpgradeWindow.Refresh();
    }

    public void ShowQuestWindow()
    {
        _currentWindow = _questWindow.gameObject;
        _currentWindow.SetActive(true);
        //_questWindow.Refresh();
    }

    public void CloseCurrentWindow()
    {
        _currentWindow?.SetActive(false);
    }
}