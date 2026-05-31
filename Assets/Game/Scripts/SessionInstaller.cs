using Cysharp.Threading.Tasks;
using Reflex.Core;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using Lifetime = Reflex.Enums.Lifetime;
using Resolution = Reflex.Enums.Resolution;

public class SessionInstaller
{
    public async UniTask<Container> InstallGameSession(Container rootContainer)
    {
        var quests = await LoadQuests();
        var items = await LoadItems();

        var sessionContainer = rootContainer.Scope(builder =>
        {
            var gameSettings = rootContainer.Resolve<GameSettings>();

            builder.RegisterValue(quests);
            builder.RegisterValue(items);

            builder.RegisterType(typeof(PlayerSpawner), Lifetime.Singleton, Resolution.Eager);
            builder.RegisterType(typeof(ItemLibrary), Lifetime.Singleton, Resolution.Eager);

            builder.RegisterType(typeof(DialogueChannel), Lifetime.Singleton, Resolution.Eager);
            builder.RegisterType(typeof(GameEvents), Lifetime.Singleton, Resolution.Eager);

            builder.RegisterType(typeof(QuestSystem), Lifetime.Singleton, Resolution.Eager);
            builder.RegisterType(typeof(DialogueSystem), Lifetime.Singleton, Resolution.Eager);

            builder.RegisterType(typeof(Player), Lifetime.Singleton, Resolution.Eager);
            builder.RegisterType(typeof(LootUnpacker), Lifetime.Singleton, Resolution.Eager);
        });


        return sessionContainer;
    }

    public async UniTask<List<QuestData>> LoadQuests()
    {
        var quests = new List<QuestData>();

        var locations = await Addressables.LoadResourceLocationsAsync("Quest").ToUniTask();
        var loadedAssets = await Addressables.LoadAssetsAsync<QuestData>(locations, null).ToUniTask();

        foreach (var quest in loadedAssets)
        {
            quests.Add(quest);
        }

        return quests;
    }

    public async UniTask<List<ItemData>> LoadItems()
    {
        var items = new List<ItemData>();

        var locations = await Addressables.LoadResourceLocationsAsync("Item").ToUniTask();
        var loadedAssets = await Addressables.LoadAssetsAsync<ItemData>(locations, null).ToUniTask();

        foreach (var item in loadedAssets)
        {
            items.Add(item);
        }

        return items;
    }
}