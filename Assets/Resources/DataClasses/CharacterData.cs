using System.Collections.Generic;
[System.Serializable]
public class CharacterData
{
    public List<UnitData> units;

    /**
     * Destructure CharacterData class into its values.
     */
    public void Deconstruct(out List<UnitData> _units)
    {
        _units = units;
    }
}