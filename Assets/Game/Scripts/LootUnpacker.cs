using System;
using System.Collections.Generic;

public class LootUnpacker : IDisposable
{
    private Player _player;
    private GameEvents _gameEvents;

    public LootUnpacker(Player player, GameEvents eventBus)
    {
        _player = player;
        _gameEvents = eventBus;

        _gameEvents.Subscribe<EnemyDeathMessage>(UnpackEnemyLoot);
        _gameEvents.Subscribe<QuestCompleted>(UnpackQuestReward);
    }

    private void UnpackEnemyLoot(EnemyDeathMessage message)
    {
        var lootTable = message.LootTable;

        foreach (var loot in lootTable.entries)
        {
            if (UnityEngine.Random.value < loot.DropChance)
            {
                _player.Inventory.AddItem(loot.itemID, loot.Amount);
            }
        }
    }

    private void UnpackQuestReward(QuestCompleted quest)
    {
        foreach (var reward in quest.rewards)
        {
            _player.Inventory.AddItem(reward.ItemId, reward.ItemAmount);
        }
    }

    public void Dispose()
    {
        _gameEvents.Unsubscribe<EnemyDeathMessage>(UnpackEnemyLoot);
    }

}