using UnityEngine;

public class PlayerMenu : MonoBehaviour
{
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private GameObject _inventoryWindow;

    //private MaskView _maskUpgradeWindow;
    private GameObject _currentWindow;

    public void ShowInventory()
    {
        _currentWindow = _inventoryWindow;
        _currentWindow.SetActive(true);
        _inventoryView.Refresh();
    }

    public void ShowUpgradeMask()
    {
        //_currentWindow = _maskUpgradeWindow.gameObject;
    }

    public void CloseCurrentWindow()
    {
        _currentWindow?.SetActive(false);
    }
}