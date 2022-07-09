using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum SOGlobalQuestType
{
    AreaCleared, MonstersKilled, MaterialsLooted, MaterialsHeld, LevelAcquired, RebirthTimes, PotionsCrafted, RegionCleared
}

public enum SOPretendMaterialList
{
    MonsterJuice, WomansPortrait, Spyglass, BasicResource
}

[CreateAssetMenu(fileName = "GlobalQuestTemplate", menuName = "Quests/New Global Quest")]
public partial class GlobalQuestTemplate : QuestTemplate
{
    [Space(10)]
    public SOQuestType soQuestType;
    [Space(5)]
    public SOGlobalQuestType questType;
    [Space(5)]
    public QuestTemplate questTriggeredBy;
    [Space(5)]
    public QuestTemplate[] questsTriggeredOnComplete;
    [Space(5)]
    public QuestRewards[] questRewards;

}

[System.Serializable]
public class QuestRewards
{
    public SOPretendMaterialList questLoot;
    public int quantityLooted;
}