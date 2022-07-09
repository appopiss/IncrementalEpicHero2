using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameController;
using static UsefulMethod;
using static MonsterParameter;
using System;
using Cysharp.Threading.Tasks;

public partial class Save
{
    public bool[] isStartedExpedition;//[id]
    public double[] expeditionProgress;
    public long[] expeditionTimeId;
    public double[] expeditionMovedDistance;

    public MonsterSpecies[] expeditionPetSpecies;//[slotId + 5 * id]
    public MonsterColor[] expeditionPetColors;//[slotId + 5 * id]
    public bool[] expeditionPetIsSet;//[slotId + 5 * id]
}

public class ExpeditionController 
{
    public Expedition[] expeditions = new Expedition[10];
    public ExpeditionController()
    {
        expGainMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        petExpGainMultiplier = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));
        unlockedExpeditionSlotNum = new Multiplier(new MultiplierInfo(MultiplierKind.Base, MultiplierType.Add, () => 1));

        globalInfoList.Add(new Expedition_Brick(this));
        globalInfoList.Add(new Expedition_Log(this));
        globalInfoList.Add(new Expedition_Shard(this));
        globalInfoList.Add(new Expedition_PetRank(this));
        globalInfoList.Add(new Expedition_Equipment(this));
        globalInfoList.Add(new Expedition_PetExp(this));

        for (int i = 0; i < expeditions.Length; i++)
        {
            int count = i;
            expeditions[i] = new Expedition(this, i);
            expeditions[i].unlock.RegisterCondition(() => count < unlockedExpeditionSlotNum.Value());
        }

        //Milestone
        //milestoneList.Add(new ExpeditionMilestone(TotalLevel, 5, (x) => { }, "Unlocks the 2nd Pet Slot"));
        milestoneList.Add(new ExpeditionMilestone(TotalLevel, 3, (x) => RegisterUnlockPet(1, x, "Expedition Milestone Lv 10"), "Unlocks the 2nd Pet Slot"));
        milestoneList.Add(new ExpeditionMilestone(TotalLevel, 20, (x) => { }, "Placeholder"));
        milestoneList.Add(new ExpeditionMilestone(TotalLevel, 30, (x) => { }, "Placeholder"));
        milestoneList.Add(new ExpeditionMilestone(TotalLevel, 40, (x) => { }, "Placeholder"));
        milestoneList.Add(new ExpeditionMilestone(TotalLevel, 50, (x) => { }, "Placeholder"));
        milestoneList.Add(new ExpeditionMilestone(TotalLevel, 60, (x) => { }, "Placeholder"));
        milestoneList.Add(new ExpeditionMilestone(TotalLevel, 70, (x) => { }, "Placeholder"));
        milestoneList.Add(new ExpeditionMilestone(TotalLevel, 80, (x) => { }, "Placeholder"));
        milestoneList.Add(new ExpeditionMilestone(TotalLevel, 90, (x) => { }, "Placeholder"));
        milestoneList.Add(new ExpeditionMilestone(TotalLevel, 100, (x) => RegisterUnlockPet(2, x, "Expedition Milestone Lv 100"), "Unlocks the 3rd Pet Slot"));

    }
    double deltaTimesec;
    public void Start()
    {
        for (int i = 0; i < expeditions.Length; i++)
        {
            expeditions[i].Start();
        }
        for (int i = 0; i < globalInfoList.Count; i++)
        {
            globalInfoList[i].Start();
        }
    }
    public void Update()
    {
        deltaTimesec += Time.deltaTime;
        if (deltaTimesec >= 1)
        {
            Progress(deltaTimesec);
            deltaTimesec = 0;
        }
    }
    public void Progress(double deltaTimesec)
    {
        for (int i = 0; i < expeditions.Length; i++)
        {
            expeditions[i].Update(deltaTimesec);
        }
    }

    public List<ExpeditionGlobalInformation> globalInfoList = new List<ExpeditionGlobalInformation>();
    public ExpeditionGlobalInformation GlobalInfo(ExpeditionKind kind)
    {
        for (int i = 0; i < globalInfoList.Count; i++)
        {
            if (globalInfoList[i].kind == kind) return globalInfoList[i];
        }
        return globalInfoList[0];
    }

    public Multiplier expGainMultiplier;
    public Multiplier petExpGainMultiplier;
    public Multiplier unlockedExpeditionSlotNum;
    public List<ExpeditionMilestone> milestoneList = new List<ExpeditionMilestone>();

    public double TotalCompletedNum()
    {
        double tempNum = 0;
        for (int i = 0; i < globalInfoList.Count; i++)
        {
            tempNum += globalInfoList[i].completedNum;
        }
        return tempNum;
    }

    public long TotalLevel()//ExpeditionのLevel
    {
        long tempLevel = 0;
        for (int i = 0; i < globalInfoList.Count; i++)
        {
            tempLevel += globalInfoList[i].level.value;
        }
        return tempLevel;
    }

    public string StatsString()
    {
        string tempStr = optStr + "<size=20>Total Expedition Levels : <color=green>Lv " + tDigit(TotalLevel()) + "</color><size=18>";
        //for (int i = 0; i < globalInfoList.Count; i++)
        //{
        //    tempStr += optStr + "\n- " + globalInfoList[i].name + " < <color=green>Lv " + tDigit(globalInfoList[i].level.value) + "</color> >  EXP : " + globalInfoList[i].exp.Description(true);
        //}
        return tempStr;
    }
    public string ExpeditionMilestoneString()
    {
        string tempStr = optStr + "<size=20>Expedition Milestones  ( <color=green>Total Lv " + tDigit(TotalLevel()) + "</color> )<size=18>";
        for (int i = 0; i < milestoneList.Count; i++)
        {
            tempStr += "\n- " + milestoneList[i].DescriptionString();
        }
        return tempStr;
    }

    void RegisterUnlockPet(int slotId, Func<bool> condition, string conditionString)
    {
        for (int i = 0; i < expeditions.Length; i++)
        {
            expeditions[i].pets[slotId].unlock.RegisterCondition(condition, conditionString);
        }
    }

    public bool CanClaim()
    {
        for (int i = 0; i < expeditions.Length; i++)
        {
            if (expeditions[i].CanClaim()) return true;
        }
        return false;
    }
}

public class ExpeditionTimeId : ArrayId
{
    public ExpeditionTimeId(int id, int initId, Func<long> arrayLength) : base(initId, arrayLength)
    {
        this.id = id;
    }
    public int id;
    public override long value { get => main.S.expeditionTimeId[id]; set => main.S.expeditionTimeId[id] = value; }
}
public class Expedition
{
    public ExpeditionController expeditionCtrl;
    public virtual ExpeditionType type => ExpeditionType.Distant;
    public ExpeditionKind kind { get => main.S.expeditionKinds[id]; set => main.S.expeditionKinds[id] = value; }
    public ExpeditionGlobalInformation globalInfo => expeditionCtrl.GlobalInfo(kind);
    public double[] timeHours = new double[] { 0.5d, 1.0d, 2.0d, 4.0d, 8.0d, 16.0d, 24.0d, 48.0d };
    public ExpeditionTimeId timeId;
    public int id;
    public ExpeditionProgress progress;
    public Unlock unlock;

    public double RequiredGold()//Startするときに必要なGold
    {
        return 5000 * Math.Max(1, Math.Pow(PetNum(), 2)) * (1 + Math.Pow(timeId.value, 2)) * Math.Pow(2, Math.Max(0, timeId.value - 3));//5K,10K,20K,50K,200K,680K,2.080M,5.920M
    }
    public Expedition(ExpeditionController expeditionCtrl, int id)
    {
        this.expeditionCtrl = expeditionCtrl;
        this.id = id;
        timeId = new ExpeditionTimeId(id, -1, () => timeHours.Length);
        progress = new ExpeditionProgress(this);
        unlock = new Unlock();

        for (int i = 0; i < pets.Length; i++)
        {
            pets[i] = new ExpeditionPet(this, i);
        }
    }
    public void Start()
    {
        //SetReward();
    }
    public void Update(double deltaTime)
    {
        Progress(deltaTime);
    }

    public bool isStarted { get => main.S.isStartedExpedition[id]; set => main.S.isStartedExpedition[id] = value; }
    public bool isFinished => progress.IsMaxed();

    public void ChangeKind(ExpeditionKind kind)
    {
        if (isStarted) return;
        this.kind = kind;
        //SetReward();
    }
    public ExpeditionPet[] pets = new ExpeditionPet[3];
    public int PetNum()
    {
        int tempNum = 0;
        for (int i = 0; i < pets.Length; i++)
        {
            if (pets[i].isSet) tempNum++;
        }
        return tempNum;
    }

    public long TotalRank()
    {
        long tempValue = 0;
        for (int i = 0; i < pets.Length; i++)
        {
            if (pets[i].isSet)
                tempValue += pets[i].pet.rank.value;
        }
        return tempValue;
    }
    public long TotalLevel()
    {
        long tempValue = 0;
        for (int i = 0; i < pets.Length; i++)
        {
            if (pets[i].isSet)
                tempValue += pets[i].pet.level.value;
        }
        return tempValue;
    }
    public double TotalMutantCaptureNum()
    {
        double tempValue = 0;
        for (int i = 0; i < pets.Length; i++)
        {
            if (pets[i].isSet)
                tempValue += pets[i].pet.globalInfo.CapturedNum(true);
        }
        return tempValue;
    }

    //MutantによるRareチャンス
    public double RareChance()
    {
        return Math.Pow(TotalMutantCaptureNum(), 2 / 3d) * 0.0001d * timeHour;
    }
    //Speed
    public double TimeSpeed()
    {
        return 1 + TotalLevel() * 0.001;
    }
    //EXP １秒あたり
    public double ExpGainPerSec()
    {
        return globalInfo.PetExpGainPerSec();
    }
    public void GetExp(double deltaTime)
    {
        for (int i = 0; i < pets.Length; i++)
        {
            if (pets[i].isSet)
                pets[i].pet.exp.Increase(ExpGainPerSec() * deltaTime);
        }
    }
    public void GetMove(double deltaTime)
    {
        for (int i = 0; i < pets.Length; i++)
        {
            if (pets[i].isSet)
            {
                double tempDistance = deltaTime * pets[i].pet.globalInfo.MoveSpeed(0, 0, false, HeroKind.Warrior);//MoveSpeedの初期値分
                main.SR.movedDistancePet += tempDistance;
                main.S.movedDistancePet += tempDistance;
                main.S.totalMovedDistancePet += tempDistance;
                movedDistance += tempDistance;
            }
        }
    }
    public double movedDistance { get => main.S.expeditionMovedDistance[id]; set => main.S.expeditionMovedDistance[id] = value; }

    //StartButton
    public void StartButtonAction()
    {
        if (!isStarted)
        {
            StartAction();
            return;
        }
        if (isFinished)
        {
            ClaimAction();
            Initialize();
            return;
        }
        //ConfirmWindowを出す
        Initialize();
    }
    public void StartAction()
    {
        //SetReward();
        game.resourceCtrl.gold.Decrease(RequiredGold());
        isStarted = true;
    }
    public bool CanStart()
    {
        if (isStarted) return false;
        if (game.resourceCtrl.gold.value < RequiredGold()) return false;
        return TotalRank() > 0;
    }
    public bool StartButtonInteractable()
    {
        if (!isStarted)
        {
            return CanStart();
        }
        if (isFinished)
        {
            return CanClaim();
        }
        return true;
    }
    public void Initialize()
    {
        isStarted = false;
        progress.ChangeValue(0);
        main.S.expeditionMovedDistance[id] = 0;
        //SetReward();
    }
    public void ClaimAction()
    {
        if (!CanClaim()) return;
        globalInfo.completedNum++;
        GetReward();
    }
    public void GetReward()
    {
        for (int i = 0; i < pets.Length; i++)
        {
            if (pets[i].isSet)
                globalInfo.GetRewardAction(this, pets[i].pet, timeHour);
        }
    }
    public bool CanClaim()
    {
        if (!isStarted) return false;
        if (!isFinished) return false;
        if (!globalInfo.ClaimCondition(this)) return false;
        return true;
    }

    public double timeHour => timeHours[timeId.value];
    public static double minTime = 5 * 60;//5分
    public static double minTimeReduction = 0.5d;//一旦、50%までとする
    public double RequiredTimesec()
    {
        double tempTime = timeHours[timeId.value] * 3600d;
        tempTime *= Math.Max(minTimeReduction, 1 / TimeSpeed());
        return Math.Max(minTime, tempTime);
    }
    public void Progress(double deltaTime)
    {
        if (!isStarted) return;
        if (isFinished) return;
        progress.Increase(deltaTime);
        globalInfo.totalTime += deltaTime;
        GetExp(deltaTime);
        GetMove(deltaTime);
        globalInfo.GetExp(deltaTime, PetNum());
    }
    public double ProgressPercent()
    {
        return Math.Min(1, Math.Max(0, progress.value / RequiredTimesec()));
    }
    public double TimesecLeft()
    {
        return Math.Max(0, RequiredTimesec() - progress.value);
    }

    //String
    public string NameString()
    {
        return optStr + "Team " + (id + 1).ToString() + " < <color=green>Lv " + tDigit(TotalLevel()) + "</color> > <color=orange>Rank " + tDigit(TotalRank()) + "</color>";
    }
    public string KindString()
    {
        return globalInfo.NameString();
    }
    public string RewardString(bool isShort = false)
    {
        string tempString = isShort ? "" : "<u>Complete Reward</u>";
        for (int i = 0; i < pets.Length; i++)
        {
            if (pets[i].isSet)
            {
                if (!isShort) tempString += "\n- ";
                tempString += globalInfo.RewardString(this, pets[i].pet, timeHour);
                if (isShort) tempString += "\n";
            }
        }
        return tempString;
    }
    public string StartButtonString()
    {
        if (!isStarted) return "Start";
        if (!isFinished) return "Cancel";
        return "Claim";
    }
}
public class ExpeditionProgress : NUMBER
{
    public ExpeditionProgress(Expedition expedition)
    {
        this.expedition = expedition;
        maxValue = () => expedition.RequiredTimesec();
    }
    Expedition expedition;
    int id => expedition.id;
    public override double value { get => main.S.expeditionProgress[id]; set => main.S.expeditionProgress[id] = value; }
}

public class ExpeditionPet
{
    public ExpeditionPet(Expedition expedition, int slotId)
    {
        this.expedition = expedition;
        this.slotId = slotId;
        unlock = new Unlock();
    }
    Expedition expedition;
    public Unlock unlock;
    int id => expedition.id;
    int slotId;
    public MonsterSpecies species { get => main.S.expeditionPetSpecies[slotId + 5 * id]; set => main.S.expeditionPetSpecies[slotId + 5 * id] = value; }
    public MonsterColor color { get => main.S.expeditionPetColors[slotId + 5 * id]; set => main.S.expeditionPetColors[slotId + 5 * id] = value; }
    public bool isSet { get => main.S.expeditionPetIsSet[slotId + 5 * id]; set => main.S.expeditionPetIsSet[slotId + 5 * id] = value; }
    public MonsterPet pet => game.monsterCtrl.GlobalInformation(species, color).pet;

    public void Set(MonsterSpecies species, MonsterColor color)
    {
        isSet = true;
        this.species = species;
        this.color = color;
    }
    public void Remove()
    {
        isSet = false;
    }
}



public enum ExpeditionType
{
    Distant,
    Passive,
}
