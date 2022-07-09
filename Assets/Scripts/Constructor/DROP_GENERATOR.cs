using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Main;
using static Parameter;
using static GameController;
using static UsefulMethod;

public class DROP_GENERATOR
{
    public DROP_GENERATOR(HeroKind heroKind)
    {
        this.heroKind = heroKind;
        Initialize();
    }
    public virtual void Drop(MaterialKind kind, double num, Vector2 position) { }
    public virtual void Drop(ResourceKind kind, double num, Vector2 position) { }
    public virtual void Drop() { }
    public virtual void Get() { }

    public virtual void Initialize()
    {
        num = 0;
        position = hidePosition;
        if (game.IsUI(heroKind) && initializeUIAction != null) initializeUIAction();
    }
    public double num;
    public Vector2 position;
    public Action dropUIAction;
    public Action initializeUIAction;
    public HeroKind heroKind;
    public virtual bool isAutoGet { get; }
}
