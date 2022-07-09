using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static Parameter;
using static GameController;
using static UsefulMethod;

public partial class SaveR
{
    public double[] materials;
}
public class MaterialController
{
    public MaterialController()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = new Material((MaterialKind)i);
        }
        doubleMaterialChance = new Multiplier();
    }
    public Multiplier doubleMaterialChance;
    public Material[] materials = new Material[Enum.GetNames(typeof(MaterialKind)).Length];
    public Material Material(MaterialKind kind)
    {
        return materials[(int)kind];
    }
}

public class Material : NUMBER
{
    public override string Name()
    {
        return Localized.localized.Material(kind);
    }
    public Material(MaterialKind kind)
    {
        this.kind = kind;
    }
    public MaterialKind kind;
    public override double value { get => main.SR.materials[(int)kind]; set => main.SR.materials[(int)kind] = value; }
    public void Increase(double increment, HeroKind heroKind)
    {
        //if (increment < 1) return;
        if (WithinRandom(game.materialCtrl.doubleMaterialChance.Value())) increment *= 2;
        base.Increase(increment);
        if (logUIAction != null && game.IsUI(heroKind)) logUIAction(kind, increment);
        if (game.IsUI(heroKind)) game.battleCtrl.areaBattle.materials[(int)kind] += increment;
        //if (resultUIAction != null) resultUIAction(increment);
    }
    public Action<MaterialKind, double> logUIAction;
    //public Action<double> resultUIAction;
}

public class MaterialGenerator : DROP_GENERATOR
{
    public MaterialGenerator(HeroKind heroKind) : base(heroKind)
    {
    }
    public override void Drop(MaterialKind kind, double num, Vector2 position)
    {
        this.num = num;
        this.kind = kind;
        if (isAutoGet)
        {
            Get();
            return;
        }
        Vector2 tempPosition = position + randomVec[UnityEngine.Random.Range(0, randomVec.Length)] * 50f * UnityEngine.Random.Range(1, 4);
        if (tempPosition.x > moveRangeX / 2f || tempPosition.x < -moveRangeX / 2f || tempPosition.y > moveRangeY / 2f || tempPosition.y < -moveRangeY / 2f)
            tempPosition = Vector2.zero;
        this.position = tempPosition;
        if (game.IsUI(heroKind) && dropUIAction != null) dropUIAction();
    }
    public override void Get()
    {
        if (num < 1) return;
        game.materialCtrl.Material(kind).Increase(num, heroKind);
        Initialize();
    }
    public MaterialKind kind;
    public override bool isAutoGet => game.monsterCtrl.IsPetActiveEffectKind(PetActiveEffectKind.GetMaterial);

}
public enum MaterialKind
{
    MonsterFluid,//Normal
    FlameShard,//Red
    FrostShard,//Blue
    LightningShard,//Yellow
    NatureShard,//Green
    PoisonShard,//Purple
    BlackPearl,//Boss
    OilOfSlime,//HP,DEF
    EnchantedCloth,//MP,MDEF
    SpiderSilk,//DEF,MDEF
    BatWing,//ATK
    FairyDust,//MATK
    FoxTail,//Prop,EXP
    FishScales,//Absorp,Move,Gold
    CarvedBranch,//Multi
    ThickFur,//SkillLevel,EXP,Gold
    UnicornHorn,//SPD,Prof
    //Lab
    SlimeBall,
    ManaSeed,
    UnmeltingIce,
    EternalFlame,
    AncientBattery,
    Ectoplasm,
    Stardust,
    VoidEgg,
    //Enchant
    EnchantedShard,
}