using Reflex.Attributes;
using System.Collections.Generic;
using UnityEngine;

public class InventoryView : UIWindow
{
    [Inject] private ItemLibrary _itemLib;
    [Inject] private Player player;

    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _itemViewPrefab;
    [SerializeField] private ItemDescriptionView _descriptionPanel;
    [SerializeField] private List<InventoryItemView> _items = new();

    private InventoryItemView _currentItem;

    public void Refresh()
    {
        var itemList = player.Inventory.GetItems();

        for (int i = 0; i < itemList.Count; i++)
        {
            var (itemId, amount) = itemList[i];
            var existingItemView = _items.Find(item => item.item.Id == itemId);
            if (existingItemView)
            {
                existingItemView.SetAmount(amount);
                continue;
            }

            var itemView = Instantiate(_itemViewPrefab, _content.transform).GetComponent<InventoryItemView>();

            itemView.gameObject.SetActive(true);
            ItemData itemData = _itemLib.GetItemById(itemId);
            itemView.SetItem(itemData, amount);
            itemView.ItemChosen += OnItemChosen;
            _items.Add(itemView);
            itemView.index = _items.Count - 1;
        }
    }

    public void OnItemChosen(ItemData item, int index)
    {
        _currentItem?.Unchoose();
        _currentItem = _items[index];
        _descriptionPanel.ShowItem(item);
    }

    public void Clear()
    {
        _currentItem?.Unchoose();
        _currentItem = null;
        _descriptionPanel.Clear();
    }

    public override void Close()
    {
        Clear();
        gameObject.SetActive(false);
    }
}
