using System.Collections.Generic;

public class GameSaveData
{
    public InventoryData InventoryData = new();
    public StatsData StatsData = new();
    public DialogueData DialogueData = new();
    public MaskData MaskData = new MaskData();
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