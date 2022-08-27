
[System.Serializable]
public class ActionData
{
    public string name;
    public string animationClip;
    public string audioFile;
    public int successRate;
    public int healthModifier;
    public int margin;

    /**
    * Destructure UnitData class into its values.
    */
    public void Deconstruct(out string _actionName, out string _animationClip, out string _audioFile, out int _successRate, out int _healthModifier, out int _margin)
    {
        _actionName = name;
        _animationClip = animationClip;
        _audioFile = audioFile;
        _successRate = successRate;
        _healthModifier = healthModifier;
        _margin = margin;
    }
}