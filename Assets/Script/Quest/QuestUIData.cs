using System.Collections.Generic;

[System.Serializable]
public class QuestObjective
{
    public string text;
    public int current;
    public int required;
}

[System.Serializable]
public class QuestModel
{
    public string id;
    public string title;
    public string description;
    public bool completed;
    public List<QuestObjective> objectives = new();
}
