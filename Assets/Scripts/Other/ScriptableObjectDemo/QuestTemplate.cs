using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum SOLanguage
{
    English, Japanese
}

public enum SOQuestType
{
    Global, Daily, Title, General, Repeatable
}

public partial class QuestTemplate : ScriptableObject
{
    public QuestLocalization[] questLocalizations = new QuestLocalization[2];
}

[System.Serializable]
public class QuestLocalization
{
    public SOLanguage language;
    public string questName;
    public string questDescription;
    public string questCondition;
    public string questReward;
}
