using System.Collections.Generic;

public class GameSaveData
{
    public InventoryData InventoryData = new();
    public StatsData StatsData = new();
    public DialogueData DialogueData = new();
    public MaskData MaskData = new();
    public List<QuestSaveData> QuestSystemData = new();
}

public class InventoryData
{
    public Dictionary<string, int> Items = new();
}

public class StatsData
{

}

public class DialogueData
{
    public string StoryState;
}

public class MaskData
{
    public int[,] Grid;
}

public class QuestSaveData
{
    public string QuestId;
    public QuestStatus Status;
    public List<int> ObjectivesProgress = new();
}
