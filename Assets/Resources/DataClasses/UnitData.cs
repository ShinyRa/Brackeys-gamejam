using System.Collections.Generic;
[System.Serializable]
public class UnitData {
    public string name;
    public float health;
    public string prefabName;
    public List<ActionData> actions;

    /**
     * Destructure UnitData class into its values.
     */
    public void Deconstruct(out string _name, out float _health, out string _prefabName, out List<ActionData> _actions) {
        _name = name;
        _health = health;
        _prefabName = prefabName;
        _actions = actions;
    }
}