using System;
using System.Collections.Generic;

public interface IGameMessage { }
public interface IGameQuery<TResult> { }

public class GameEvents
{
    private readonly Dictionary<Type, object> _messageHandlers = new();
    private readonly Dictionary<Type, object> _queryHandlers = new();

    public void Subscribe<T>(Action<T> handler) where T : IGameMessage
    {
        var type = typeof(T);
        if (!_messageHandlers.ContainsKey(type))
        {
            _messageHandlers[type] = new List<Action<T>>();
        }
        ((List<Action<T>>)_messageHandlers[type]).Add(handler);
    }

    public void Unsubscribe<T>(Action<T> handler) where T : IGameMessage
    {
        var type = typeof(T);
        if (_messageHandlers.TryGetValue(type, out var list))
        {
            ((List<Action<T>>)list).Remove(handler);
        }
    }

    public void Publish<T>(T gameEvent) where T : IGameMessage
    {
        var type = typeof(T);
        if (_messageHandlers.TryGetValue(type, out var list))
        {
            var handlersCopy = new List<Action<T>>((List<Action<T>>)list);
            foreach (var handler in handlersCopy)
            {
                handler?.Invoke(gameEvent);
            }
        }
    }

    public void RegisterQueryHandler<TQuery, TResult>(Func<TQuery, TResult> handler) where TQuery : IGameQuery<TResult>
    {
        var type = typeof(TQuery);
        if (_queryHandlers.ContainsKey(type))
        {
            UnityEngine.Debug.LogError($"Double query source");
            return;
        }
        _queryHandlers[type] = handler;
    }

    public void UnregisterQueryHandler<TQuery, TResult>() where TQuery : IGameQuery<TResult>
    {
        _queryHandlers.Remove(typeof(TQuery));
    }

    public TResult Query<TQuery, TResult>(TQuery query) where TQuery : IGameQuery<TResult>
    {
        var type = typeof(TQuery);
        if (_queryHandlers.TryGetValue(type, out var handlerObj))
        {
            var handler = (Func<TQuery, TResult>)handlerObj;
            return handler.Invoke(query);
        }

        UnityEngine.Debug.LogWarning($"Query source absent");
        return default;
    }
}

public struct EnemyKilledMessage : IGameMessage
{
    public string EnemyId;
    public int Count;
}

public struct ItemGainedMessage : IGameMessage
{
    public string ItemId;
    public int Amount;
}

public struct EnemyDeathMessage : IGameMessage
{
    public string EnemyId;
    public LootTable LootTable;
}

public struct HideHUDMessage : IGameMessage
{
    public int milliseconds;
}

public struct QuestCompleted : IGameMessage
{
    public List<QuestRewardData> rewards;
}

public struct GetItemCountQuery : IGameQuery<int>
{
    public string ItemId;
}