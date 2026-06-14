using System;

public class LootUnpacker : IDisposable
{
    private Player _player;
    private GameEvents _gameEvents;

    public LootUnpacker(Player player, GameEvents eventBus)
    {
        _player = player;
        _gameEvents = eventBus;

        _gameEvents.Subscribe<EnemyDeathMessage>(UnpackEnemyLoot);
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

    public void Dispose()
    {
        _gameEvents.Unsubscribe<EnemyDeathMessage>(UnpackEnemyLoot);
    }

}