[System.Serializable]
public class Achievement
{
    public string name;
    public bool isUnlocked;
    public string conditions;

    public Achievement(string name, string conditions)
    {
        this.name = name;
        this.isUnlocked = false;
        this.conditions = conditions;
    }
}
