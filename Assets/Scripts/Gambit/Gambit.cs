using System.Collections.Generic;
using UnityEngine;

public class Gambit : MonoBehaviour
{
    public enum Target
    {
        Self,
        ClosestEnemy,
        FarthestEnemy,
        EnemyTargetingAlly,
        AllyLowestHP,
        AnyAlly,
        None
    }

    public enum Condition
    {
        FullLife,
        HP_90,
        HP_50,
        HP_10,
        KO,
        FullMana,
        NoMana,
        None
    }

    public enum Action
    {
        Attack,
        Heal,
        FireSpell,
        Potion,
        Revive,
        None
    }

    public Target target;
    public Condition condition;
    public Action action;

    public Gambit(Target t, Condition c, Action a)
    {
        target = t;
        condition = c;
        action = a;
    }
}
