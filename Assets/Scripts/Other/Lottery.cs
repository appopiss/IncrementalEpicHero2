using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lottery
{
    public double[] chances;
    public Lottery(double[] chances)
    {
        this.chances = chances;
        CheckChances();
    }

    void CheckChances()
    {
        double tempChance = 0;
        for (int i = 0; i < chances.Length; i++)
        {
            tempChance += chances[i];
        }
        if (tempChance.Equals(0)) return;
        if (tempChance.Equals(1)) return;
        for (int i = 0; i < chances.Length; i++)
        {
            chances[i] *= 1d / tempChance;
        }
    }

    public int SelectedId()
    {
        int temp = Random.Range(0, Parameter.randomAccuracy);
        double tempChance = 0;
        for (int i = 0; i < chances.Length; i++)
        {
            tempChance += chances[i];
            if (temp < tempChance * Parameter.randomAccuracy) return i;
        }
        return 0;
    }
}
