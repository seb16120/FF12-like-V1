using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Element;
using System;
using static Spell;


public class Spell : MonoBehaviour
{
    public enum SpellType
    {
        Offensive,
        Buff,
        Debuff,
        Healing,
        Resurrection,
        Other
    }

    public enum SpellTarget
    {
        SingleTarget,
        MultiTarget,
        AreaOfEffect,
        Self,
        Other
    }

    public enum SpellId
    {
        // Offensif
        Fireball,
        AverageFireball,
        GreatFireball,
        Firestorm,
        GreatFirestorm,
        WaterBall,
        AverageWaterBall,
        GreatWaterBall,
        Tsunami,
        GreatTsunami,
        LightningStrike,
        AverageLightningStrike,
        GreatLightningStrike,
        Thunderstorm,
        GreatThunderstorm,
        IceShard,
        AverageIceShard,
        GreatIceShard,
        Blizzard,
        GreatBlizzard,
        EarthSpike,
        AverageEarthSpike,
        GreatEarthSpike,
        Meteor,
        GreatMeteor,
        WindSlash,
        AverageWindSlash,
        GreatWindSlash,
        Tornado,
        GreatTornado,
        HolyLight,
        AverageHolyLight,
        GreatHolyLight,
        HeavenlyLight,
        DarkFlare,
        AverageDarkFlare,
        GreatDarkFlare,
        Apocalypse,
        // Soin
        FirstAid,
        BeginnerHeal,
        AverageHeal,
        GreatHeal,
        AreaHeal,
        MultiHeal,
        OmniHeal,
        // Résurrection
        LastStand,
        Zombify,
        SecondChance,
        Revive,
        Paradise,
        HeavenOnEarth,
        // Buff
        Faith,
        Hast,
        Bravery,
        Protect,
        Shell,
        HolyProtection, // protect against Light element : 
        HolyNeutralization, // neutralize the Light element
        HolyAbsorption, // absorb the Light element
        DarkProtection, // protect against Dark element
        DarkNeutralization, // neutralize the Dark element
        DarkAbsorption, // absorb the Dark element
        FireProtection, // protect against Fire element
        FireNeutralization, // neutralize the Fire element
        FireAbsorption, // absorb the Fire element
        WaterProtection, // protect against Water element
        WaterNeutralization, // neutralize the Water element
        WaterAbsorption, // absorb the Water element
        ThunderProtection, // protect against Thunder element
        ThunderNeutralization, // neutralize the Thunder element
        ThunderAbsorption, // absorb the Thunder element
        IceProtection, // protect against Ice element
        IceNeutralization, // neutralize the Ice element
        IceAbsorption, // absorb the Ice element
        EarthProtection, // protect against Earth element
        EarthNeutralization, // neutralize the Earth element
        EarthAbsorption, // absorb the Earth element
        AirProtection, // protect against Air element
        AirNeutralization, // neutralize the Air element
        AirAbsorption, // absorb the Air element
        GroupFaith,
        GroupHast,
        GroupBravery,
        GroupProtect,
        GroupShell,
        GroupHolyProtection, // Buff spell that protects against all elements
        GroupDarkProtection, // Buff spell that protects against all elements
        GroupFireProtection, // Buff spell that protects against all elements
        GroupWaterProtection, // Buff spell that protects against all elements
        GroupThunderProtection, // Buff spell that protects against all elements
        GroupIceProtection, // Buff spell that protects against all elements
        GroupEarthProtection, // Buff spell that protects against all elements
        GroupAirProtection, // Buff spell that protects against all elements
        GroupDivineProtection, // Buff spell that protects against all elements
        // Debuff
        Slow,
        HolyWeakness, // lower the resistance against Light element
        DarkWeakness, // lower the resistance against Dark element
        FireWeakness, // lower the resistance against Fire element
        WaterWeakness,
        ThunderWeakness, // lower the resistance against Thunder element
        IceWeakness, // lower the resistance against Ice element
        EarthWeakness, // lower the resistance against Earth element
        AirWeakness, // lower the resistance against Air element
        GroupSlow,
        GroupHolyWeakness, // Buff spell that protects against all elements
        GroupDarkWeakness, // Buff spell that protects against all elements
        GroupFireWeakness, // Buff spell that protects against all elements
        GroupWaterWeakness, // Buff spell that protects against all elements
        GroupThunderWeakness, // Buff spell that protects against all elements
        GroupIceWeakness, // Buff spell that protects against all elements
        GroupEarthWeakness, // Buff spell that protects against all elements
        GroupAirWeakness, // Buff spell that protects against all elements
        // Autres


    }

    // Définir les noms des sorts dans une classe statique
    public static class SpellName
    {
        public enum HealSpell
        {
            //unique target
            FirstAid,
            BeginnerHeal,
            AverageHeal,
            GreatHeal,
            //multi target
            AreaHeal,
            MultiHeal,
            OmniHeal
        }

        public enum OffensiveSpell
        {
            //unique target // se dirige vers un seul ennemi de maniere directe
            Fireball,
            AverageFireball,
            GreatFireball,
            WaterBall,
            AverageWaterBall,
            GreatWaterBall,
            LightningStrike,
            AverageLightningStrike,
            GreatLightningStrike,
            IceShard,
            AverageIceShard,
            GreatIceShard,
            EarthSpike,
            WindSlash,
            AverageWindSlash,
            GreatWindSlash,
            AverageEarthSpike,
            GreatEarthSpike,
            HolyLight,
            AverageHolyLight,
            GreatHolyLight,
            DarkFlare,
            AverageDarkFlare,
            GreatDarkFlare,
            //Multi Target AoE
            Firestorm, //une zone de feu au sol se declanche (Le W de Brand)
            Tsunami, // une vague d'eau qui deferle en ligne droite (le R de Nami)
            GreatTsunami, // R de Nami en plus large
            Thunderstorm, // une tempete de foudre qui se deplace en ligne droite (le R de Victor)
            Blizzard, // une tempête de neige qui se déplace en ligne droite (le R de nami mais en plus lent et d'élement glace)
            Tornado, // une tornade qui se deplace en ligne droite (Le Q de Janna)
            GreatTornado, // R de Janna s'il ne repoussez pas et faissait des dmg (comme l'ancien R de Maokai)
            Meteor, // Une méteorite qui tombe sur une zone (le W de Veigar)
            HeavenlyLight, // une lumière divine qui se déplace en ligne droite (le R de Lux)
            //Multi target Targeted
            GreatFirestorm, // R de Brand
            GreatThunderstorm, // le R de Victor en plus large
            GreatMeteor, // R de Veigar en plus large
            GreatBlizzard, // une tempête de neige qui se déplace en ligne droite (le R de nami mais en plus lent et d'élement glace), plus large et plus rapide
            Apocalypse, // une sorte de R de Karthus

        }

        // Vous pouvez ajouter d'autres catégories ici
        public enum ReviveSpell
        {
            //unique target
            LastStand, // Buff spell that resurrects the target with 10% curHP, 10% avHP, have the debuff "bloody", the target lose 1% of his curHP every 5 sec
            Zombify, // Buff spell that resurrects the target with 25% curHP, 25% avHP, have the debuff "zombie", the target can't be healed, the target need to see a Priest to be dezombified.
            SecondChance, // Buff spell that resurrects the target with 50% curHP, 50% avHP
            Revive, // Buff spell that resurrects the target with 100% curHP, 100% avHP
            //multi target
            Paradise, // Buff spell that resurrects all allies 50% curHP, 50% avHP in the Group
            HeavenOnEarth, // Buff spell that resurrects all allies 100% curHP, 100% avHP in the Group


        }
        public enum BuffSpell
        {
            //unique target
            Faith, // Buff spell that increases the magic power of the target
            Hast, // Buff spell that increases the speed of the target
            Bravery, // Buff spell that increases the attack dmg of the target
            Protect, // Buff spell that increases the defense of the target
            Shell, // Buff spell that increases the magic defense of the target
            HolyProtection, // protect against Light element : 
            HolyNeutralization, // neutralize the Light element
            HolyAbsorption, // absorb the Light element
            DarkProtection,
            DarkNeutralization,
            DarkAbsorption,
            FireProtection,
            FireNeutralization,
            FireAbsorption,
            WaterProtection,
            WaterNeutralization,
            WaterAbsorption,
            ThunderProtection,
            ThunderNeutralization,
            ThunderAbsorption,
            IceProtection,
            IceNeutralization,
            IceAbsorption,
            EarthProtection,
            EarthNeutralization,
            EarthAbsorption,
            AirProtection,
            AirNeutralization,
            AirAbsorption,

            //multi target
            GroupFaith,
            GroupHast,
            GroupBravery,
            GroupProtect,
            GroupShell,
            GroupHolyProtection, // Buff spell that protects the group against light element
            GroupDarkProtection, 
            GroupFireProtection,
            GroupWaterProtection,
            GroupThunderProtection,
            GroupIceProtection,
            GroupEarthProtection,
            GroupAirProtection,
            GroupDivineProtection, // Buff spell that protects the group against all elements
        }

        public enum DebuffSpell
        {
            //unique target
            Slow,
            HolyWeakness, // protect against Light element : 
            DarkWeakness, // protect against Dark element
            FireWeakness, // protect against Fire element
            WaterWeakness, // protect against Water element
            ThunderWeakness, // protect against Thunder element
            IceWeakness, // protect against Ice element
            EarthWeakness, // protect against Earth element
            AirWeakness, // protect against Air element
            //multi target
            GroupSlow,
            GroupHolyWeakness, // Buff spell that protects against all elements
            GroupDarkWeakness, // Buff spell that protects against all elements
            GroupFireWeakness, // Buff spell that protects against all elements
            GroupWaterWeakness, // Buff spell that protects against all elements
            GroupThunderWeakness, // Buff spell that protects against all elements
            GroupIceWeakness, // Buff spell that protects against all elements
            GroupEarthWeakness, // Buff spell that protects against all elements
            GroupAirWeakness, // Buff spell that protects against all elements
        }
    }

    public class ElementRegistry
    {
        public readonly Dictionary<ElementType, List<Spell.SpellId>> SpellsByElement = new()
        {
            { ElementType.Fire, new List<Spell.SpellId> { Spell.SpellId.Fireball, SpellId.AverageFireball, SpellId.GreatFireball, SpellId.Firestorm, SpellId.GreatFirestorm } },
            { ElementType.Water, new List<Spell.SpellId> { Spell.SpellId.WaterBall, SpellId.AverageWaterBall, SpellId.GreatWaterBall, SpellId.Tsunami, SpellId.GreatTsunami } },
            { ElementType.Thunder, new List<Spell.SpellId> { Spell.SpellId.LightningStrike, SpellId.AverageLightningStrike, SpellId.GreatLightningStrike, SpellId.Thunderstorm, SpellId.GreatThunderstorm } },
            { ElementType.Ice, new List<Spell.SpellId> { Spell.SpellId.IceShard, SpellId.AverageIceShard, SpellId.GreatIceShard, SpellId.Blizzard, SpellId.GreatBlizzard } },
            { ElementType.Earth, new List<Spell.SpellId> { Spell.SpellId.EarthSpike, Spell.SpellId.AverageEarthSpike, Spell.SpellId.GreatEarthSpike, Spell.SpellId.Meteor, Spell.SpellId.GreatMeteor } },
            { ElementType.Air, new List<Spell.SpellId> { Spell.SpellId.WindSlash, Spell.SpellId.AverageWindSlash, Spell.SpellId.GreatWindSlash, Spell.SpellId.Tornado, Spell.SpellId.GreatTornado } },
            { ElementType.Light, new List<Spell.SpellId> { Spell.SpellId.BeginnerHeal, Spell.SpellId.AverageHeal, Spell.SpellId.GreatHeal, Spell.SpellId.AreaHeal, Spell.SpellId.MultiHeal, Spell.SpellId.OmniHeal, Spell.SpellId.HolyLight, Spell.SpellId.AverageHolyLight, Spell.SpellId.GreatHolyLight, Spell.SpellId.HeavenlyLight, Spell.SpellId.SecondChance, Spell.SpellId.Revive, Spell.SpellId.Paradise, Spell.SpellId.HeavenOnEarth, SpellId.GroupDivineProtection } },
            { ElementType.Dark, new List<Spell.SpellId> { Spell.SpellId.DarkFlare, Spell.SpellId.AverageDarkFlare, Spell.SpellId.GreatDarkFlare, Spell.SpellId.Apocalypse, Spell.SpellId.Zombify } },
            { ElementType.None, new List<Spell.SpellId> { Spell.SpellId.FirstAid, Spell.SpellId.LastStand, Spell.SpellId.Slow, Spell.SpellId.GroupSlow, Spell.SpellId.Faith, Spell.SpellId.GroupFaith, Spell.SpellId.Hast, Spell.SpellId.GroupHast, Spell.SpellId.Bravery, Spell.SpellId.GroupBravery, Spell.SpellId.Protect, Spell.SpellId.GroupProtect, Spell.SpellId.Shell, Spell.SpellId.GroupShell, SpellId.HolyProtection, SpellId.GroupHolyProtection,SpellId.HolyWeakness, SpellId.GroupHolyWeakness, SpellId.HolyNeutralization, SpellId.HolyAbsorption, SpellId.DarkProtection, SpellId.GroupDarkProtection, Spell.SpellId.DarkWeakness, SpellId.GroupDarkWeakness, SpellId.DarkNeutralization, SpellId.DarkAbsorption, SpellId.FireProtection, SpellId.GroupFireProtection, SpellId.FireWeakness, SpellId.GroupFireWeakness, SpellId.FireNeutralization, SpellId.FireAbsorption, SpellId.WaterProtection, SpellId.GroupWaterProtection, SpellId.WaterWeakness, SpellId.GroupWaterWeakness, SpellId.WaterNeutralization, SpellId.WaterAbsorption, SpellId.ThunderProtection, SpellId.GroupThunderProtection, SpellId.ThunderWeakness,SpellId.GroupThunderWeakness, SpellId.ThunderNeutralization, SpellId.ThunderAbsorption, SpellId.IceProtection, SpellId.GroupIceProtection, SpellId.IceWeakness, SpellId.GroupIceWeakness, SpellId.IceNeutralization, SpellId.IceAbsorption, SpellId.EarthProtection, SpellId.GroupEarthProtection, SpellId.EarthWeakness, SpellId.GroupEarthWeakness, SpellId.EarthNeutralization, SpellId.EarthAbsorption, SpellId.AirProtection, SpellId.GroupAirProtection, SpellId.AirWeakness,  SpellId.GroupAirWeakness, SpellId.AirNeutralization, SpellId.AirAbsorption } },

        };
    }
    public class ProprietiesOfTheSpell
    {
        public readonly Dictionary<Spell.SpellId, List<object> > ProprietiesBySpell = new()
        {
            { Spell.SpellId.Fireball, new List<object> { ElementType.Fire, SpellType.Offensive, SpellTarget.SingleTarget, 10 } },
            { Spell.SpellId.AverageFireball, new List<object> { ElementType.Fire, SpellType.Offensive, SpellTarget.SingleTarget, 40 } },
            { Spell.SpellId.GreatFireball, new List<object> { ElementType.Fire, SpellType.Offensive, SpellTarget.SingleTarget, 100 } },
            { Spell.SpellId.Firestorm, new List<object> { ElementType.Fire, SpellType.Offensive, SpellTarget.AreaOfEffect, 33 } },
            { Spell.SpellId.GreatFirestorm, new List<object> { ElementType.Fire, SpellType.Offensive, SpellTarget.AreaOfEffect, 75 } },
            { Spell.SpellId.WaterBall, new List<object> { ElementType.Water, SpellType.Offensive, SpellTarget.SingleTarget, 10 } },
            { Spell.SpellId.AverageWaterBall, new List<object> { ElementType.Water, SpellType.Offensive, SpellTarget.SingleTarget, 40 } },
            { Spell.SpellId.GreatWaterBall, new List<object> { ElementType.Water, SpellType.Offensive, SpellTarget.SingleTarget, 100 } },
            { Spell.SpellId.Tsunami, new List<object> { ElementType.Water, SpellType.Offensive, SpellTarget.AreaOfEffect, 33 } },
            { Spell.SpellId.GreatTsunami, new List<object> { ElementType.Water, SpellType.Offensive, SpellTarget.AreaOfEffect, 75 } },
            { Spell.SpellId.LightningStrike, new List<object> { ElementType.Thunder, SpellType.Offensive, SpellTarget.SingleTarget, 10 } },
            { Spell.SpellId.AverageLightningStrike, new List<object> { ElementType.Thunder, SpellType.Offensive, SpellTarget.SingleTarget, 40 } },
            { Spell.SpellId.GreatLightningStrike, new List<object> { ElementType.Thunder, SpellType.Offensive, SpellTarget.SingleTarget, 100 } },
            { Spell.SpellId.Thunderstorm, new List<object> { ElementType.Thunder,SpellType.Offensive ,SpellTarget.AreaOfEffect, 33} },
            { Spell.SpellId.GreatThunderstorm, new List<object> { ElementType.Thunder, SpellType.Offensive, SpellTarget.AreaOfEffect, 75 } },
            { Spell.SpellId.IceShard, new List<object> { ElementType.Ice, SpellType.Offensive, SpellTarget.SingleTarget, 10 } },
            { Spell.SpellId.AverageIceShard, new List<object> { ElementType.Ice, SpellType.Offensive, SpellTarget.SingleTarget, 40 } },
            { Spell.SpellId.GreatIceShard, new List<object> { ElementType.Ice, SpellType.Offensive, SpellTarget.SingleTarget, 100 } },
            { Spell.SpellId.Blizzard, new List<object> { ElementType.Ice, SpellType.Offensive, SpellTarget.AreaOfEffect, 33 } },
            { Spell.SpellId.GreatBlizzard, new List<object> { ElementType.Ice, SpellType.Offensive, SpellTarget.AreaOfEffect, 75 } },
            { Spell.SpellId.EarthSpike, new List<object> { ElementType.Earth,SpellType.Offensive ,SpellTarget.SingleTarget, 10} },
            { Spell.SpellId.AverageEarthSpike, new List<object> { ElementType.Earth,SpellType.Offensive ,SpellTarget.SingleTarget, 40} },
            { Spell.SpellId.GreatEarthSpike, new List<object> { ElementType.Earth,SpellType.Offensive ,SpellTarget.SingleTarget, 100} },
            { Spell.SpellId.Meteor, new List<object> { ElementType.Earth ,SpellType.Offensive ,SpellTarget.AreaOfEffect, 33} },
            { Spell.SpellId.GreatMeteor ,new List<object> { ElementType.Earth ,SpellType.Offensive ,SpellTarget.AreaOfEffect, 75} },
            { Spell.SpellId.WindSlash ,new List<object> { ElementType.Air ,SpellType.Offensive ,SpellTarget.SingleTarget, 10} },
            { Spell.SpellId.AverageWindSlash ,new List<object> { ElementType.Air ,SpellType.Offensive ,SpellTarget.SingleTarget, 40} },
            { Spell.SpellId.GreatWindSlash ,new List<object> { ElementType.Air ,SpellType.Offensive ,SpellTarget.SingleTarget, 100} },
            { Spell.SpellId.Tornado ,new List<object> { ElementType.Air ,SpellType.Offensive ,SpellTarget.AreaOfEffect, 33} },
            { Spell.SpellId.GreatTornado ,new List<object> { ElementType.Air ,SpellType.Offensive ,SpellTarget.AreaOfEffect, 75} },
            { Spell.SpellId.HolyLight ,new List<object> { ElementType.Light ,SpellType.Offensive ,SpellTarget.SingleTarget, 15} },
            { Spell.SpellId.AverageHolyLight ,new List<object> { ElementType.Light ,SpellType.Offensive ,SpellTarget.SingleTarget, 50} },
            { Spell.SpellId.GreatHolyLight ,new List<object> { ElementType.Light ,SpellType.Offensive ,SpellTarget.SingleTarget, 128} },
            { Spell.SpellId.HeavenlyLight ,new List<object> { ElementType.Light ,SpellType.Offensive ,SpellTarget.MultiTarget, 100} },
            { Spell.SpellId.DarkFlare ,new List<object> { ElementType.Dark, SpellType.Offensive, SpellTarget.SingleTarget, 15 } },
            { Spell.SpellId.AverageDarkFlare, new List<object> { ElementType.Dark, SpellType.Offensive, SpellTarget.SingleTarget, 50 } },
            { Spell.SpellId.GreatDarkFlare, new List<object> { ElementType.Dark, SpellType.Offensive, SpellTarget.SingleTarget, 128 } },
            { Spell.SpellId.Apocalypse, new List<object> { ElementType.Dark, SpellType.Offensive, SpellTarget.MultiTarget, 100 } },
            { Spell.SpellId.FirstAid, new List<object> { ElementType.Light, SpellType.Healing, SpellTarget.Self, 5 } },
            { Spell.SpellId.BeginnerHeal, new List<object> { ElementType.Light, SpellType.Healing, SpellTarget.SingleTarget, 10 } },
            { Spell.SpellId.AverageHeal, new List<object> { ElementType.Light, SpellType.Healing, SpellTarget.SingleTarget, 40 } },
            { Spell.SpellId.GreatHeal, new List<object> { ElementType.Light, SpellType.Healing, SpellTarget.SingleTarget, 100 } },
            { Spell.SpellId.AreaHeal, new List<object> { ElementType.Light, SpellType.Healing, SpellTarget.MultiTarget, 33 } },
            { Spell.SpellId.MultiHeal, new List<object> { ElementType.Light, SpellType.Healing, SpellTarget.MultiTarget, 75 } },
            { Spell.SpellId.OmniHeal, new List<object> { ElementType.Light, SpellType.Healing, SpellTarget.Self, 255 } }, // no power/dmg, but it's for telling it will full heal the target (or put the Zomby type of enemy to 1HP, expect if he is immune to thos effect)
            { Spell.SpellId.LastStand ,new List<object> { ElementType.None ,SpellType.Resurrection ,SpellTarget.SingleTarget, 10} },
            { Spell.SpellId.Zombify ,new List<object> { ElementType.None ,SpellType.Resurrection ,SpellTarget.SingleTarget, 25} },
            { Spell.SpellId.SecondChance ,new List<object> { ElementType.None ,SpellType.Resurrection ,SpellTarget.SingleTarget, 50} },
            { Spell.SpellId.Revive ,new List<object> { ElementType.None ,SpellType.Resurrection ,SpellTarget.SingleTarget, 100} },
            { Spell.SpellId.Paradise ,new List<object> { ElementType.None ,SpellType.Resurrection ,SpellTarget.MultiTarget, 50} },
            { Spell.SpellId.HeavenOnEarth ,new List<object> { ElementType.None ,SpellType.Resurrection ,SpellTarget.MultiTarget, 100} },
            { Spell.SpellId.Faith ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, 30} }, // no dmg but it's for telling the target will do +30% magic dmg
            { Spell.SpellId.Hast ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, 33} }, // // no dmg but it's for telling the target will have +33% loading speed of the ATB Bar.
            { Spell.SpellId.Bravery ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, 30} },
            { Spell.SpellId.Protect ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, 30} },
            { Spell.SpellId.Shell ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, 30} },
            { Spell.SpellId.GroupFaith, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.MultiTarget, 30 } },
            { Spell.SpellId.GroupHast, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.MultiTarget, 33 } },
            { Spell.SpellId.GroupBravery, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.MultiTarget, 30 } },
            { Spell.SpellId.GroupProtect, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.MultiTarget, 30 } },
            { Spell.SpellId.GroupShell, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.MultiTarget, 30 } },
            { Spell.SpellId.Slow ,new List<object> { ElementType.None ,SpellType.Debuff ,SpellTarget.SingleTarget, 25} },
            { Spell.SpellId.HolyWeakness ,new List<object> { ElementType.None ,SpellType.Debuff ,SpellTarget.SingleTarget, 50} },
            { Spell.SpellId.DarkWeakness ,new List<object> { ElementType.None ,SpellType.Debuff ,SpellTarget.SingleTarget, 50} },
            { Spell.SpellId.FireWeakness ,new List<object> { ElementType.None ,SpellType.Debuff ,SpellTarget.SingleTarget, 33} },
            { Spell.SpellId.WaterWeakness ,new List<object> { ElementType.None, SpellType.Debuff, SpellTarget.SingleTarget, 33 } },
            { Spell.SpellId.ThunderWeakness ,new List<object> { ElementType.None, SpellType.Debuff, SpellTarget.SingleTarget, 33 } },
            { Spell.SpellId.IceWeakness ,new List<object> { ElementType.None, SpellType.Debuff, SpellTarget.SingleTarget, 33 } },
            { Spell.SpellId.EarthWeakness ,new List<object> { ElementType.None, SpellType.Debuff, SpellTarget.SingleTarget, 33 } },
            { Spell.SpellId.AirWeakness ,new List<object> { ElementType.None, SpellType.Debuff, SpellTarget.SingleTarget, 33 } },
            { Spell.SpellId.GroupSlow ,new List<object> { ElementType.None ,SpellType.Debuff ,SpellTarget.MultiTarget, 25} },
            { Spell.SpellId.GroupHolyWeakness ,new List<object> { ElementType.None ,SpellType.Debuff ,SpellTarget.MultiTarget, 50} },
            { Spell.SpellId.GroupDarkWeakness ,new List<object> { ElementType.None ,SpellType.Debuff ,SpellTarget.MultiTarget, 50} },
            { Spell.SpellId.GroupFireWeakness ,new List<object> { ElementType.None ,SpellType.Debuff ,SpellTarget.MultiTarget, 50} },
            { Spell.SpellId.GroupWaterWeakness ,new List<object> { ElementType.None ,SpellType.Debuff ,SpellTarget.MultiTarget, 50} },
            { Spell.SpellId.GroupThunderWeakness ,new List<object> { ElementType.None ,SpellType.Debuff ,SpellTarget.MultiTarget, 50} },
            { Spell.SpellId.GroupIceWeakness ,new List<object> { ElementType.None ,SpellType.Debuff ,SpellTarget.MultiTarget, 50} },
            { Spell.SpellId.GroupEarthWeakness, new List<object> { ElementType.None, SpellType.Debuff, SpellTarget.MultiTarget, 50 } },
            { Spell.SpellId.GroupAirWeakness, new List<object> { ElementType.None, SpellType.Debuff, SpellTarget.MultiTarget, 50 } },
            { Spell.SpellId.FireProtection, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.SingleTarget, 50 } }, // no dmg but it's for telling the target will reduce the fire dmg by half after he receive the effect of the spell
            { Spell.SpellId.FireNeutralization, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.SingleTarget, 0 } }, // no dmg but it's for telling the target will nullify the fire dmg after he receive the effect of the spell.
            { Spell.SpellId.FireAbsorption, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.SingleTarget, -100 } }, // no dmg but it's for telling the target will absorb the fire dmg.
            { Spell.SpellId.WaterProtection, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.SingleTarget, 50 } }, 
            { Spell.SpellId.WaterNeutralization, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.SingleTarget, 0 } },
            { Spell.SpellId.WaterAbsorption, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.SingleTarget, -100 } },
            { Spell.SpellId.ThunderProtection, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.SingleTarget, 50 } },
            { Spell.SpellId.ThunderNeutralization, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.SingleTarget, 0 } },
            { Spell.SpellId.ThunderAbsorption, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.SingleTarget, -100 } },
            { Spell.SpellId.IceProtection ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, 50} },
            { Spell.SpellId.IceNeutralization ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, 0} },
            { Spell.SpellId.IceAbsorption ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, -100} },
            { Spell.SpellId.EarthProtection ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, 50} },
            { Spell.SpellId.EarthNeutralization ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, 0} },
            { Spell.SpellId.EarthAbsorption ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, -100} },
            { Spell.SpellId.AirProtection ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, 50} },
            { Spell.SpellId.AirNeutralization ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, 0} },
            { Spell.SpellId.AirAbsorption ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, -100} },
            { Spell.SpellId.HolyProtection ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, 0} },
            { Spell.SpellId.HolyNeutralization ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, 0} },
            { Spell.SpellId.HolyAbsorption ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, -100} },
            { Spell.SpellId.DarkProtection ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, 50} },
            { Spell.SpellId.DarkNeutralization ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, 0} },
            { Spell.SpellId.DarkAbsorption ,new List<object> { ElementType.None ,SpellType.Buff ,SpellTarget.SingleTarget, -100} },
            { Spell.SpellId.GroupHolyProtection, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.MultiTarget, 50 } },
            { Spell.SpellId.GroupDarkProtection, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.MultiTarget, 50 } },
            { Spell.SpellId.GroupFireProtection, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.MultiTarget, 33 } },
            { Spell.SpellId.GroupWaterProtection, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.MultiTarget, 33 } },
            { Spell.SpellId.GroupThunderProtection, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.MultiTarget, 33 } },
            { Spell.SpellId.GroupIceProtection, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.MultiTarget, 33 } },
            { Spell.SpellId.GroupEarthProtection, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.MultiTarget, 33 } },
            { Spell.SpellId.GroupAirProtection, new List<object> { ElementType.None, SpellType.Buff, SpellTarget.MultiTarget, 33 } },
            { Spell.SpellId.GroupDivineProtection ,new List<object> { ElementType.None, SpellType.Buff, SpellTarget.MultiTarget, 42 } },
            { Spell.SpellId.GroupDivineProtection ,new List<object> { ElementType.Light ,SpellType.Buff ,SpellTarget.MultiTarget, 50} },

        };
    }

    public class SpellTypeRegistry
    {
        public readonly Dictionary<Spell.SpellType, List<Spell.SpellId>> TypeOfTheSpell = new()
        {
            { Spell.SpellType.Offensive, new List<Spell.SpellId> { Spell.SpellId.Fireball, SpellId.AverageFireball, SpellId.GreatFireball, SpellId.Firestorm, SpellId.GreatFirestorm, SpellId.WaterBall, SpellId.AverageWaterBall, SpellId.GreatWaterBall, SpellId.Tsunami, SpellId.GreatTsunami, SpellId.LightningStrike, SpellId.AverageLightningStrike, SpellId.GreatLightningStrike, SpellId.Thunderstorm, SpellId.GreatThunderstorm, SpellId.IceShard, SpellId.AverageIceShard, SpellId.GreatIceShard, SpellId.Blizzard, SpellId.GreatBlizzard, SpellId.EarthSpike, SpellId.AverageEarthSpike, SpellId.GreatEarthSpike, SpellId.Meteor, SpellId.GreatMeteor, SpellId.WindSlash, SpellId.AverageWindSlash, SpellId.GreatWindSlash, SpellId.Tornado, SpellId.GreatTornado, SpellId.HolyLight, SpellId.AverageHolyLight, SpellId.GreatHolyLight, SpellId.HeavenlyLight, SpellId.DarkFlare, SpellId.AverageDarkFlare, SpellId.GreatDarkFlare, SpellId.Apocalypse } },
            { Spell.SpellType.Healing, new List<Spell.SpellId> { Spell.SpellId.BeginnerHeal, Spell.SpellId.AverageHeal, Spell.SpellId.GreatHeal, Spell.SpellId.AreaHeal, Spell.SpellId.MultiHeal, Spell.SpellId.OmniHeal} },
            { Spell.SpellType.Buff, new List<Spell.SpellId> { Spell.SpellId.Faith, Spell.SpellId.Hast, Spell.SpellId.Bravery, Spell.SpellId.Protect, Spell.SpellId.Shell, SpellId.GroupFaith, SpellId.GroupHast, SpellId.GroupBravery, SpellId.GroupProtect, SpellId.GroupShell, SpellId.GroupDarkProtection, SpellId.GroupFireProtection, SpellId.GroupWaterProtection, SpellId.GroupThunderProtection, SpellId.GroupIceProtection, SpellId.GroupEarthProtection, SpellId.GroupAirProtection, SpellId.GroupDivineProtection, SpellId.GroupHolyProtection, SpellId.HolyProtection, SpellId.HolyNeutralization, SpellId.HolyAbsorption, SpellId.DarkProtection, SpellId.DarkNeutralization, SpellId.DarkAbsorption, SpellId.FireProtection, SpellId.FireNeutralization, SpellId.FireAbsorption, SpellId.WaterProtection, SpellId.WaterNeutralization, SpellId.WaterAbsorption, SpellId.ThunderProtection, SpellId.ThunderNeutralization, SpellId.ThunderAbsorption, SpellId.IceProtection, SpellId.IceNeutralization, SpellId.IceAbsorption, SpellId.EarthProtection, SpellId.EarthNeutralization, SpellId.EarthAbsorption, SpellId.AirProtection, SpellId.AirNeutralization, SpellId.AirAbsorption } },
            { Spell.SpellType.Resurrection, new List<Spell.SpellId> { Spell.SpellId.LastStand, Spell.SpellId.Zombify, Spell.SpellId.SecondChance, Spell.SpellId.Revive, Spell.SpellId.Paradise, Spell.SpellId.HeavenOnEarth } },
            { Spell.SpellType.Debuff, new List<Spell.SpellId> { Spell.SpellId.Slow, Spell.SpellId.HolyWeakness, Spell.SpellId.DarkWeakness, Spell.SpellId.FireWeakness, Spell.SpellId.WaterWeakness, Spell.SpellId.ThunderWeakness, Spell.SpellId.IceWeakness, Spell.SpellId.EarthWeakness, Spell.SpellId.AirWeakness, SpellId.GroupSlow, SpellId.GroupHolyWeakness, SpellId.GroupDarkWeakness, SpellId.GroupFireWeakness, SpellId.GroupWaterWeakness, SpellId.GroupThunderWeakness, SpellId.GroupIceWeakness, SpellId.GroupEarthWeakness, SpellId.GroupAirWeakness } },
            { Spell.SpellType.Other, new List<Spell.SpellId> {  } } // Add other spell types here if needed

        };
    }



    public class SpellTargetRegistry
    {
        public readonly Dictionary<SpellTarget, List<Spell.SpellId>> TargetOfTheSpell = new() // Just a copy past, i need to change it !!! :
    {
        { SpellTarget.SingleTarget, new List<Spell.SpellId> { Spell.SpellId.Fireball, SpellId.AverageFireball, SpellId.GreatFireball, SpellId.Firestorm, SpellId.GreatFirestorm, SpellId.WaterBall, SpellId.AverageWaterBall, SpellId.GreatWaterBall, SpellId.LightningStrike, SpellId.AverageLightningStrike, SpellId.GreatLightningStrike, SpellId.IceShard, SpellId.AverageIceShard, SpellId.GreatIceShard, SpellId.EarthSpike, SpellId.AverageEarthSpike, SpellId.GreatEarthSpike } },
        { SpellTarget.MultiTarget, new List<Spell.SpellId> { SpellId.Tsunami, SpellId.GreatTsunami, SpellId.Thunderstorm, SpellId.GreatThunderstorm, SpellId.Blizzard, SpellId.GreatBlizzard, SpellId.HolyLight, SpellId.AverageHolyLight, SpellId.GreatHolyLight, SpellId.HeavenlyLight, SpellId.DarkFlare, SpellId.AverageDarkFlare, SpellId.GreatDarkFlare, SpellId.Apocalypse } },
        { SpellTarget.AreaOfEffect, new List<Spell.SpellId> { SpellId.Meteor, SpellId.GreatMeteor, SpellId.Tornado, SpellId.GreatTornado } },
        { SpellTarget.Self, new List<Spell.SpellId> {  } },
        { SpellTarget.Other, new List<Spell.SpellId> {  } }
    };
    }

    void CheckSpellIdAndElementBySpell()
    {
        var allEnumIds = Enum.GetValues(typeof(Spell.SpellId)).Cast<Spell.SpellId>().ToHashSet();
        var dictIds = new ProprietiesOfTheSpell().ProprietiesBySpell.Keys.ToHashSet();

        var inEnumNotInDict = allEnumIds.Except(dictIds).ToList();
        var inDictNotInEnum = dictIds.Except(allEnumIds).ToList();

        Console.WriteLine("Présents dans SpellId mais absents de ProprietiesBySpell :");
        foreach (var id in inEnumNotInDict)
            Console.WriteLine(id);

        Console.WriteLine("\nPrésents dans ProprietiesBySpell mais absents de SpellId :");
        foreach (var id in inDictNotInEnum)
            Console.WriteLine(id);
    }

    void CheckSpellIdDuplicates()
    {
        var registry = new Spell.ElementRegistry();
        var allSpellIds = registry.SpellsByElement.Values.SelectMany(list => list);
        var duplicates = allSpellIds
            .GroupBy(id => id)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        Console.WriteLine("Doublons trouvés dans SpellsByElement :");
        foreach (var id in duplicates)
            Console.WriteLine(id);
    }

    public static int GetSpellDamage(Spell.SpellId spellId)
    {
        var spellProperties = new ProprietiesOfTheSpell().ProprietiesBySpell[spellId];
        var damage = (int)spellProperties[3]; // Assuming the damage is the 4th element in the list
        Console.WriteLine($"Dégâts de {spellId}: {damage}");
        return damage;
    }

    void Start()
    {
        CheckSpellIdAndElementBySpell();
        CheckSpellIdDuplicates();
    }
}
