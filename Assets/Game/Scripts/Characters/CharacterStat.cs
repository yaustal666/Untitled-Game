public enum StatType
{
    MaxHealth,
    Strength,
    CritChance
}

public class CharacterStat
{
    public float Value => _base + _diff;

    private float _base;
    private float _diff;

    public CharacterStat(float baseVal)
    {
        _base = baseVal;
    }

    public void AddDifference(float val)
    {
        _diff += val;
    }

}