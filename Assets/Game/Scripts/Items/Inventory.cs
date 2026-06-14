using System;
using System.Collections.Generic;

public class Inventory : IDisposable, ISavable
{
    private GameEvents _eventBus;
    private Dictionary<string, int> _mappingItemToAmount;

    public Inventory(ISaveRegistry saveRegistry, GameEvents eventBus)
    {
        _mappingItemToAmount = new Dictionary<string, int>();
        saveRegistry.Register(this);

        _eventBus = eventBus;
        _eventBus.RegisterQueryHandler<GetItemCountQuery, int>(OnGetItemCountRequested);
    }

    private int OnGetItemCountRequested(GetItemCountQuery query)
    {
        if (_mappingItemToAmount.TryGetValue(query.ItemId, out int amount))
        {
            return amount;
        }

        return 0;
    }

    public void Dispose()
    {
        _eventBus.UnregisterQueryHandler<GetItemCountQuery, int>();
    }

    public void AddItem(string itemId, int amount)
    {
        if (!HasItem(itemId))
        {
            _mappingItemToAmount.TryAdd(itemId, amount);
        }
        else
        {
            _mappingItemToAmount[itemId] += amount;
        }

        var message = new ItemGainedMessage { ItemId = itemId, Amount = amount };
        _eventBus.Publish(message);
    }

    public bool HasItem(string itemId, int amount = 1)
    {
        if (_mappingItemToAmount.TryGetValue(itemId, out int value))
        {
            if (value >= amount)
            {
                return true;
            }
        }

        return false;
    }

    public List<(string Id, int Amount)> GetItems()
    {
        var res = new List<(string, int)>();

        foreach (var kvp in _mappingItemToAmount)
        {
            res.Add((kvp.Key, kvp.Value));
        }

        return res;
    }

    public void Save(GameSaveData saveData)
    {
        saveData.InventoryData.Items.Clear();

        foreach (var kvp in _mappingItemToAmount)
        {
            saveData.InventoryData.Items.Add(kvp.Key, kvp.Value);
        }
    }

    public void Load(GameSaveData saveData)
    {
        _mappingItemToAmount.Clear();

        foreach (var kvp in saveData.InventoryData.Items)
        {
            _mappingItemToAmount.Add(kvp.Key, kvp.Value);
        }
    }
}