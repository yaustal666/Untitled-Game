using System.Collections.Generic;

public class ItemLibrary
{
    private Dictionary<string, ItemData> _items;

    public ItemLibrary(List<ItemData> items)
    {
        _items = new Dictionary<string, ItemData>();
        foreach (ItemData item in items)
        {
            _items.TryAdd(item.Id, item);
        }
    }

    public ItemData GetItemById(string itemId)
    {
        return _items[itemId];
    }
}