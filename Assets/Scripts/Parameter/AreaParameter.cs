using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaParameter
{
    //public readonly static int maxAreaId = 8;//こちらはリリース後も可変（ただし100まで）
    //Areaはリリース後のアプデでも各エリア最大10個まで。Levelは0~19まで。これはリリース後変更不可。
    public readonly static int firstDungeonIdForSave = 200;//こちらはリリース後は変更不可
    public readonly static int firstLevelIdForSave = 10;//リリース後変更不可
    public readonly static int firstAreaIdForSave = 10;//変更不可
    public readonly static int firstMissionIdForSave = 10;//変更不可
    //AreaPrestige
    public readonly static int maxPrestigeLevel = 9;
    public static MonsterRarity DefaultRarity(int id)
    {
        switch (id)
        {
            case 0:
                return MonsterRarity.Normal;
            case 1:
                return MonsterRarity.Common;
            case 2:
                return MonsterRarity.Common;
            case 3:
                return MonsterRarity.Uncommon;
            case 4:
                return MonsterRarity.Uncommon;
            case 5:
                return MonsterRarity.Uncommon;
            case 6:
                return MonsterRarity.Rare;
            case 7:
                return MonsterRarity.Rare;
        }
        return MonsterRarity.Normal;
    }
    //Spawn Rate : Normal, Blue, Yellow, Red, Green, Purple, Boss, Metal
    public static readonly double[][] monsterColorRate = new double[][]//[Rarity][Color]
    {
        //Normal
        new double[] { 1, 0, 0, 0, 0, 0, 0, 0},
        //Common
        new double[] { 0.5, 0.3, 0.2, 0, 0, 0, 0, 0},
        //Uncommon
        new double[] { 0, 0.4, 0.3, 0.2, 0.1, 0, 0, 0},
        //Rare
        new double[] { 0, 0, 0, 0.2, 0.35, 0.35, 0, 0},
        //Boss
        new double[] { 0, 0, 0, 0, 0, 0, 1, 0},
    };
}
