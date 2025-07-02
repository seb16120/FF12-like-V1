using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Spell;

public enum Faction
{
    Ally,
    Enemy,
    Neutral
}

public enum WeaponRange // Weapon Range
{
    shortRange, // dagger, sword, axe, hammer, ninja sword, instrument : 75units
    mediumRange, // polearm, staff, GreatSword, katana : 100units
    longRange, // spear : 150units, can attack flying enemies
    veryLongRange // bow, crossbow, gun, bombLance : 256units, can attack flying enemies
}



public enum WeaponType
{
    Sword, GreatSword, Katana, Staff, Pole, Spear, Dagger, NinjaSword, Instrument,
    Bow, Crossbow, Gun, BombLance, Hammer, Axe
}



public class CharacterStats : MonoBehaviour
{
    [Header("Identity")]
    public string CharacterName;
    public Faction Faction;
    public WeaponType Weapon; // Weapon Type
    public WeaponRange Range; // Weapon Range

    [Header("Base Stats")]
    public int MaxHP;
    public int AvHP; // Avaible HP (some abilities can lower max HP, we keep track of the original max HP and show what is left : [*****+++--] *: current hp, +: avaible hp, -- maxhp, the player has 10 maxHp, 8 AvHP 5 curHP, default avHP = maxHP)
    public int MaxMP;
    public int AvMP; // Avaible MP (some abilities can lower max MP, we keep track of the original max MP and show what is left : [*****+++--] *: current mp, +: avaible mp, -- maxmp, the player has 10 maxMp, 8 AvMP 5 curMP, default avMP = maxMP)
    public int Strength; // Affects physical damage (1 to 256).
    public int Magic; // Affects magical damage (1 to 256).
    public int Defense; // Affects physical damage taken (0 to 256).
    public int MagicDefense; // Affects magical damage taken (0 to 256).
    public int Attack;      // Physical Weapon Attack (1 to 256).
    public int MagicAttack; // Magical Weapon Attack (1 to 256).
    public int Speed;       // Affects Charge Bar, combo rate, and dmg of some weapons.
    public int Evasion;     // Affects chance to dodge an attack, 1% by default.
    public int Vigor;      // Affects chance to resist a status effect (0 to 100%)
    public int luck;       // Affects chance to steal items, chance to crit (if using a weapon that has a crit rate), and chance to find items in chests. Also affects the chance to get a rare item from a monster. (0 to 100%), It affect the chance of avoidng, if avoide stat is too low, and the chance of avoiding a status effect (0 to 100%) if vigor stat is too low to successfully resist a status effect. (0 to 100%)
    public bool AreGambitsEnabled = true; // Par défaut, les Gambits sont activés.

    [Header("Accuracy and Crit")]
    public float Accuracy;   // (0 to 100%), default is 100% (no miss)
    public float CritRate;   // (0 to 100%), default is 0% (no crit)
    public float ComboRate;  // (0 to 100% depending weapon), default is the same as the weapon type (0 to 100%).

    // if a block is successful, by default the damage is reduced by 66% (0.33 * dmg) for shield block, by 50% (0.5 * dmg) for weapon parade, and by 33% (0.67 * dmg) for unarmed parade.
    [Header("Defense Skills")]
    public float ShieldBlockRate;     // % chance to block physical attack with shield, shield has a default of 5% block rate.
    public float MagicBlockRate;      // % chance to block magic attack with special shield, 0% if no magic shield equipped.
    public float WeaponParadeRate;    // % chance to block using weapon (no shield), 3% by default.
    public float UnarmedParadeRate;   // % chance to block, unarmed, an attack: 1% by default vs armed enemy, 2% by default vs unarmed enemy.


    [Header("Current Stats")]
    public int CurrentHP;
    public int CurrentMP;
    public float CurrentChargeTime; // Charge Bar Fill

    [Header("Buffs")]
    public bool HasBravery;    // +30% physical
    public bool HasFaith;      // +30% magical
    public bool HasProtect;    // -30% physical taken
    public bool HasShell;    // -30% magical taken
    public bool HasHaste;      // +50% speed
    public bool HasSlow;       // -50% speed
    public bool HasRegen;      // +x% HP regen
    public bool HasMPRegen;    // +x% MP regen

    [Header("Status Effects")]
    public bool IsKO;          // KO = Can't act
    public bool IsBlind;       // Blind = Lower accuracy

    [Header("Resistances, Weaknesses, and Absorptions")]
    public bool ResistFire;    // -50% fire damage (*0.5)
    public bool WeakToFire;    // +50% fire damage (*1.5)
    public bool AbsorbFire;    // Absorb fire damage

    public bool ResistWater;   // -50% water damage
    public bool WeakToWater;   // +50% water damage
    public bool AbsorbWater;   // Absorb water damage

    public bool HasResistThunder;
    public bool WeakToThunder;
    public bool AbsorbThunder;

    public bool HasResistIce;
    public bool WeakToIce;
    public bool AbsorbIce;

    public bool HasResistEarth;
    public bool WeakToEarth;
    public bool AbsorbEarth;

    public bool HasResistAir;
    public bool WeakToAir;
    public bool AbsorbAir;

    public bool HasResistLight;
    public bool WeakToLight;
    public bool AbsorbLight;

    public bool HasResistDark;
    public bool WeakToDark;
    public bool AbsorbDark;

    // - Déclaration d'un champ privé pour stocker les affinités par défaut.

    private static Dictionary<string, List<Element.ElementType>> _DefaultAffinities = new()
    {
        { "element", new List<Element.ElementType>() },
        { "neutralised", new List<Element.ElementType>() },
        { "weakness", new List<Element.ElementType>() },
        { "absorbtion", new List<Element.ElementType>() },
        { "resistance", new List<Element.ElementType>() },
        { "none", new List<Element.ElementType> {
            Element.ElementType.Fire, Element.ElementType.Water, Element.ElementType.Thunder,
            Element.ElementType.Ice, Element.ElementType.Air, Element.ElementType.Earth,
            Element.ElementType.Light, Element.ElementType.Dark
        } }
    };
    // - Déclaration d'un champ public qui copy les affinités par défaut dans son initialisation (dans Start() ou dans le constructeur).
    public Dictionary<string, List<Element.ElementType>> affinities;


    // function to clone the affinities dictionary
    private static Dictionary<string, List<Element.ElementType>> CloneAffinities(Dictionary<string, List<Element.ElementType>> source)
    {
        var copy = new Dictionary<string, List<Element.ElementType>>();
        foreach (var entry in source)
        {
            copy[entry.Key] = new List<Element.ElementType>(entry.Value);
        }
        return copy;
    }


    // - Déclaration d'un champ privé pour stocker les affinités élémentaires.
    [System.Serializable]
    public class ElementsAffinities
    {
        public string Type; // "weakness", "resistance", etc.
        public List<Element.ElementType> Elements = new();
    }

    // - Liste publique pour stocker les affinités élémentaires de base.
    [Header("Affinités élémentaires de base")]
    public List<ElementsAffinities> BaseAffinities = new();

    // - Liste publique pour stocker les affinités élémentaires temporaires.
    public List<Element.ElementType> GetElementsByType(string type)
    {
        var affinity = BaseAffinities.Find(a => a.Type == type);
        return affinity != null ? affinity.Elements : new List<Element.ElementType>();
    }




    private void ValidateElementalAttributes() // need to be changed.
    {
        // Fire
        if (ResistFire && (WeakToFire || AbsorbFire))
        {
            Debug.LogWarning($"{CharacterName} ne peut pas être à la fois Résistant, Faible et Absorber le Feu. Réinitialisation des attributs Feu.");
            ResistFire = WeakToFire = AbsorbFire = false;
        }

        // Water
        if (ResistWater && (WeakToWater || AbsorbWater))
        {
            Debug.LogWarning($"{CharacterName} ne peut pas être à la fois Résistant, Faible et Absorber l'Eau. Réinitialisation des attributs Eau.");
            ResistWater = WeakToWater = AbsorbWater = false;
        }

        // Thunder
        if (HasResistThunder && (WeakToThunder || AbsorbThunder))
        {
            Debug.LogWarning($"{CharacterName} ne peut pas être à la fois Résistant, Faible et Absorber la Foudre. Réinitialisation des attributs Foudre.");
            HasResistThunder = WeakToThunder = AbsorbThunder = false;
        }

        // Ice
        if (HasResistIce && (WeakToIce || AbsorbIce))
        {
            Debug.LogWarning($"{CharacterName} ne peut pas être à la fois Résistant, Faible et Absorber la Glace. Réinitialisation des attributs Glace.");
            HasResistIce = WeakToIce = AbsorbIce = false;
        }

        // Earth
        if (HasResistEarth && (WeakToEarth || AbsorbEarth))
        {
            Debug.LogWarning($"{CharacterName} ne peut pas être à la fois Résistant, Faible et Absorber la Terre. Réinitialisation des attributs Terre.");
            HasResistEarth = WeakToEarth = AbsorbEarth = false;
        }

        // Air
        if (HasResistAir && (WeakToAir || AbsorbAir))
        {
            Debug.LogWarning($"{CharacterName} ne peut pas être à la fois Résistant, Faible et Absorber l'Air. Réinitialisation des attributs Air.");
            HasResistAir = WeakToAir = AbsorbAir = false;
        }

        // Light
        if (HasResistLight && (WeakToLight || AbsorbLight))
        {
            Debug.LogWarning($"{CharacterName} ne peut pas être à la fois Résistant, Faible et Absorber la Lumière. Réinitialisation des attributs Lumière.");
            HasResistLight = WeakToLight = AbsorbLight = false;
        }

        // Dark
        if (HasResistDark && (WeakToDark || AbsorbDark))
        {
            Debug.LogWarning($"{CharacterName} ne peut pas être à la fois Résistant, Faible et Absorber les Ténèbres. Réinitialisation des attributs Ténèbres.");
            HasResistDark = WeakToDark = AbsorbDark = false;
        }
    }

    private void Awake()
    {
        affinities = CloneAffinities(_DefaultAffinities);
    }

    private void Start()
    {
        AvHP = MaxHP;
        CurrentHP = AvHP;
        AvMP = MaxMP;
        CurrentMP = AvMP;
        CurrentChargeTime = 0f;

        // Validate elemental attributes at the start
        ValidateElementalAttributes();
    }

    void Update() // to be remove when BattleSystem starts real updates.
    {
        CurrentChargeTime += Time.deltaTime * 10f;
    }

    public void PerformAttack(CharacterStats target)
    {
        if (target == null) return;
        float damage = DamageCalculator.CalculatePhysicalDamage(this, target);
        Debug.Log($"{CharacterName} attacks {target.CharacterName} for {damage} damage.");
        target.TakeDamage(damage);
        FloatingTextManager.Instance.CreateFloatingText(((int)damage).ToString(), target.transform);
    }


    public void TakeDamage(float amount)
    {
        CurrentHP -= (int)amount;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);

        Debug.Log(CharacterName + " took " + amount + " damage!");
        //AudioManager.Instance.Play("hit"); // #TODO: add sound effect and create the AudioManager


        if (CurrentHP <= 0)
        {
            IsKO = true;
            Debug.Log(CharacterName + " is KO!");
            //AudioManager.Instance.Play("KO"); // #TODO: add sound effect and create the AudioManager
        }
    }
    public void ExecuteGambits() // #TODO: link with the Gambit system
    {
        if (!AreGambitsEnabled)
        {
            Debug.Log($"{CharacterName} n'exécute pas de Gambits car ils sont désactivés.");
            return;
        }

        // Exemple de logique pour exécuter les Gambits
        Debug.Log($"{CharacterName} exécute ses Gambits.");
        // Ajoutez ici la logique pour exécuter les actions automatiques des Gambits.
    }
    // C#
    public void AddTemporaryAffinity(Dictionary<string, List<Element.ElementType>> baseAffinities, Dictionary<string, List<Element.ElementType>> CopyAffinities, string type, Element.ElementType element)
    {
        CopyAffinities = affinities;
        // Vérifie si le type existe dans le dictionnaire
        if (!affinities.ContainsKey(type))
        {
            Debug.LogWarning($"Type d'affinité {type} non reconnu pour {CharacterName}.");
            return;
        }

        // Ajoute l'élément à la liste si ce n'est pas déjà présent
        if (!affinities[type].Contains(element))
        {
            affinities[type].Add(element);
            Debug.Log($"{CharacterName} a ajouté {element} à ses affinités {type}.");
        }
        else
        {
            Debug.Log($"{CharacterName} possède déjà {element} dans ses affinités {type}.");
        }
    }

    public void RemoveTemporaryAffinity(string type, Element.ElementType element)
    {
        // Retire l’élément de la catégorie pendant la durée de l’effet
    }





}

