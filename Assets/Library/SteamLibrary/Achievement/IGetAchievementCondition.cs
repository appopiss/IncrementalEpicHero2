using static SteamAchievement;
using static GameController;
using static AchievementKind;

public interface IGetAchievementCondition
{
    bool GetConditionsById(int id);
}

public class IGetAchievementConditions : Main, IGetAchievementCondition
{

    public bool GetConditionsById(int id)
    {
        var conditions = (Achievement)id switch
        {
            Achievement.Playtest => main.S.isPlaytestBeforeJune10,
            Achievement.Tutorial => game.achievementCtrl.Achievement(ClearTutorial).isCleared,
            Achievement.Hero3 => game.achievementCtrl.Achievement(UnlockAngel).isCleared,
            Achievement.Hero6 => game.achievementCtrl.Achievement(UnlockTamer).isCleared,
            Achievement.Magicslime => game.achievementCtrl.Achievement(UnlockMagicslime).isCleared,
            Achievement.Sider => game.achievementCtrl.Achievement(UnlockSpider).isCleared,
            Achievement.Bat => game.achievementCtrl.Achievement(UnlockBat).isCleared,
            Achievement.Fairy => game.achievementCtrl.Achievement(UnlockFairy).isCleared,
            Achievement.Fox => game.achievementCtrl.Achievement(UnlockFox).isCleared,
            Achievement.Devilfish => game.achievementCtrl.Achievement(UnlockDevilfish).isCleared,
            Achievement.Treant => game.achievementCtrl.Achievement(UnlockTreant).isCleared,
            Achievement.Flametiger => game.achievementCtrl.Achievement(UnlockFlametiger).isCleared,
            Achievement.Unicorn => game.achievementCtrl.Achievement(UnlockUnicorn).isCleared,
            Achievement.Swarm1 => game.achievementCtrl.Achievement(Swarm1).isCleared,
            Achievement.Swarm10000 => game.achievementCtrl.Achievement(Swarm10000).isCleared,
            //Achievement.Florzporb => game.achievementCtrl.Achievement(Florzporb).isCleared,
            //Achievement.Arachnetta => game.achievementCtrl.Achievement(Arachnetta).isCleared,
            //Achievement.GurdianKor => game.achievementCtrl.Achievement(GurdianKor).isCleared,
            //Achievement.Nostro => game.achievementCtrl.Achievement(Nostro).isCleared,
            //Achievement.LadyEmelda => game.achievementCtrl.Achievement(LadyEmelda).isCleared,
            //Achievement.NariSune => game.achievementCtrl.Achievement(NariSune).isCleared,
            //Achievement.Octobaddie => game.achievementCtrl.Achievement(Octobaddie).isCleared,
            //Achievement.Bananoon => game.achievementCtrl.Achievement(Bananoon).isCleared,
            //Achievement.Glorbliorbus => game.achievementCtrl.Achievement(Glorbliorbus).isCleared,
            //Achievement.Gankyu => game.achievementCtrl.Achievement(Gankyu).isCleared,
            Achievement.AlchemyMaster => game.achievementCtrl.Achievement(PotionLv1250).isCleared,
            Achievement.AlchemyGrandmaster => game.achievementCtrl.Achievement(PotionLv3000).isCleared,
            Achievement.ClassSkill8 => game.achievementCtrl.Achievement(ClassSkill8Warrior).isCleared || game.achievementCtrl.Achievement(ClassSkill8Wizard).isCleared || game.achievementCtrl.Achievement(ClassSkill8Angel).isCleared || game.achievementCtrl.Achievement(ClassSkill8Thief).isCleared || game.achievementCtrl.Achievement(ClassSkill8Archer).isCleared || game.achievementCtrl.Achievement(ClassSkill8Tamer).isCleared,
            Achievement.GlobalSkill8 => game.achievementCtrl.Achievement(GlobalSkill8).isCleared,
            Achievement.Rebirth1 => game.achievementCtrl.IsAchievedAnyHeroRebirth(0),
            Achievement.Rebirth2 => game.achievementCtrl.IsAchievedAnyHeroRebirth(1),
            Achievement.Rebirth3 => game.achievementCtrl.IsAchievedAnyHeroRebirth(2),
            Achievement.Rebirth4 => game.achievementCtrl.IsAchievedAnyHeroRebirth(3),
            Achievement.Rebirth5 => game.achievementCtrl.IsAchievedAnyHeroRebirth(4),
            Achievement.Rebirth6 => game.achievementCtrl.IsAchievedAnyHeroRebirth(5),
            Achievement.Ascension1 => game.achievementCtrl.Achievement(Ascension1).isCleared,
            Achievement.Ascension2 => game.achievementCtrl.Achievement(Ascension2).isCleared,
            Achievement.Ascension3 => game.achievementCtrl.Achievement(Ascension3).isCleared,
            Achievement.Walk => game.achievementCtrl.Achievement(Walk40075km).isCleared,
            Achievement.Walk2 => game.achievementCtrl.Achievement(Walk384400km).isCleared,
            Achievement.Chest10000 => game.achievementCtrl.Achievement(Chest10000).isCleared,
            Achievement.EpicCoin10000 => game.achievementCtrl.Achievement(EpicCoin10000).isCleared,
            _ => false
        };
        return conditions;
    }
}

