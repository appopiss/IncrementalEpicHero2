using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using System;

public partial class SaveR
{
    public double[] essences;
}

public class EssenceController
{
    public EssenceController()
    {
        for (int i = 0; i < essences.Length; i++)
        {
            essences[i] = new Essence((EssenceKind)i);
        }
    }
    public Essence[] essences = new Essence[Enum.GetNames(typeof(EssenceKind)).Length];
    public Essence Essence(EssenceKind kind)
    {
        return essences[(int)kind];
    }
}

public class Essence : NUMBER
{
    public override string Name()
    {
        return Localized.localized.EssenceName(kind);
    }
    public Essence(EssenceKind kind)
    {
        this.kind = kind;
    }
    public EssenceKind kind;
    public override double value { get => main.SR.essences[(int)kind]; set => main.SR.essences[(int)kind] = value; }
}

public enum  EssenceKind
{
    EssenceOfSlime,
    EssenceOfLife,
    EssenceOfMagic,
    EssenceOfCreation,
    EssenceOfIce,
    EssenceOfWinter,
    EssenceOfFire,
    EssenceOfSummer,
    EssenceOfThunder,
    EssenceOfWind,
    EssenceOfSpirit,
    EssenceOfDeath,
    EssenceOfLight,
    EssenceOfRebirth,
    EssenceOfTime,
    EssenceOfEternity,
}
