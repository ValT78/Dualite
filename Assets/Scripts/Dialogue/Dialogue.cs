[System.Serializable]
public class Dialogue
{
    public string name;
    public Story story;

    public Dialogue(string _name, Story _story)
    {
        name = _name;
        story = _story;
    }
}

