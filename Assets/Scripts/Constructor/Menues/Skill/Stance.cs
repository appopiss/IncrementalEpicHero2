using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using static GameController;

public class Stance
{
    public Stance(HeroKind heroKind, int id)
    {
        this.heroKind = heroKind;
        this.id = id;
        SetEffect();
    }
    public int id;
    public HeroKind heroKind;
    public virtual bool isActive { get => game.skillCtrl.classSkills[(int)heroKind].currentStanceId == id; }
    public virtual void SetEffect() { }
    public virtual double effectValueBuff { get => 0; }
    public virtual double effectValueDebuff { get => 0; }
}

public class NullStance : Stance
{
    public NullStance(HeroKind heroKind, int id) : base(heroKind, id)
    {
    }
}
