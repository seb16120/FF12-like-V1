using System.Collections.Generic;

public static class GambitLists
{
    public static readonly List<Gambit.Target> Targets = new List<Gambit.Target>
    {
        Gambit.Target.Self,
        Gambit.Target.ClosestEnemy,
        Gambit.Target.FarthestEnemy,
        Gambit.Target.EnemyTargetingAlly,
        Gambit.Target.AllyLowestHP,
        Gambit.Target.AnyAlly,
        Gambit.Target.None
    };

    public static readonly List<Gambit.Condition> Conditions = new List<Gambit.Condition>
    {
        Gambit.Condition.FullLife,
        Gambit.Condition.HP_90,
        Gambit.Condition.HP_50,
        Gambit.Condition.HP_10,
        Gambit.Condition.KO,
        Gambit.Condition.FullMana,
        Gambit.Condition.NoMana,
        Gambit.Condition.None
    };

    public static readonly List<Gambit.Action> Actions = new List<Gambit.Action>
    {
        Gambit.Action.Attack,
        Gambit.Action.Heal,
        Gambit.Action.FireSpell,
        Gambit.Action.Potion,
        Gambit.Action.Revive,
        Gambit.Action.None
    };
    public static bool IsValidTarget(Gambit.Target target) => Targets.Contains(target);
    public static bool IsValidCondition(Gambit.Condition condition) => Conditions.Contains(condition);
    public static bool IsValidAction(Gambit.Action action) => Actions.Contains(action);
}

