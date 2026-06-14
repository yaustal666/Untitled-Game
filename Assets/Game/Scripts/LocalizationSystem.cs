using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public struct ItemInfo
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("description")] public string Description { get; set; }
}

public struct QuestInfo
{
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("description")] public string Description { get; set; }
    [JsonProperty("objectives")] public List<string> Objectives { get; set; }
}

public class LocalizationSystem
{
    private Dictionary<string, ItemInfo> _items = new Dictionary<string, ItemInfo>();
    private Dictionary<string, QuestInfo> _quests = new Dictionary<string, QuestInfo>();

    public string CurrentLanguage { get; private set; }

    public LocalizationSystem()
    {
        CurrentLanguage = "en";
    }

    public async UniTask ChangeLanguage(string language)
    {
        if (CurrentLanguage == language && _items.Count > 0) return;

        var address = $"Assets/Game/Localization/Strings/{language.ToLower()}.json";
        AsyncOperationHandle<TextAsset> handle = Addressables.LoadAssetAsync<TextAsset>(address);

        await handle.Task.AsUniTask();

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            JObject root = JObject.Parse(handle.Result.text);

            if (root["items"] != null)
            {
                _items = root["items"].ToObject<Dictionary<string, ItemInfo>>();
            }

            if (root["quests"] != null)
            {
                _quests = root["quests"].ToObject<Dictionary<string, QuestInfo>>();
            }

            CurrentLanguage = language.ToLower();
        }
        else
        {
            Addressables.Release(handle);
            throw new Exception($"Failed to load addressable asset at: {address}");
        }

        Addressables.Release(handle);
    }

    public ItemInfo GetItemInfo(string itemId)
    {
        if (_items.TryGetValue(itemId, out var itemInfo))
        {
            return itemInfo;
        }

        return default;
    }

    public QuestInfo GetQuestInfo(string questId)
    {
        if (_quests.TryGetValue(questId, out var questInfo))
        {
            return questInfo;
        }

        return default;
    }

    public async UniTask<TextAsset> GetStoryAssetAsync()
    {
        string address = $"Assets/Game/Localization/Story/StoryMain.json";

        AsyncOperationHandle<TextAsset> handle = Addressables.LoadAssetAsync<TextAsset>(address);
        await handle.Task.AsUniTask();

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }

        Addressables.Release(handle);
        return null;
    }
}
