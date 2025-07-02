using UnityEditor.PackageManager;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Spell;

public static class DamageCalculator
{
    //public static Spell spell;
    public static Spell.SpellId spellId;
    public static Element ElementInstance;
    // La ligne suivante ne fonctionne pas car elle ne respecte pas la syntaxe C# pour déclarer un champ ou une propriété :
    // public static ElDmgCalc = ElementInstance.ElementalDamage();

    // Problèmes :
    // 1. Il manque un type pour la variable (ex : float, int, etc.).
    // 2. L'appel à ElementInstance.ElementalDamage() nécessite des arguments obligatoires selon la signature de la méthode dans la classe Element.
    // 3. On ne peut pas initialiser un champ statique avec une méthode nécessitant des paramètres sans fournir ces paramètres.
    // 4. Les méthodes ne peuvent pas être assignées directement à un champ de cette façon en C#.

    // Exemple de correction possible (selon le type de retour attendu et l'utilisation) :
    /*
    public static float ElDmgCalc; // Déclaration du champ avec un type explicite

    // Initialisation dans une méthode statique ou dans un constructeur statique si besoin :
    static DamageCalculator()
    {
        // Il faut fournir les bons arguments à la méthode ElementalDamage
        // ElDmgCalc = ElementInstance.ElementalDamage(...);
    }
    */
    public static float CalculatePhysicalDamage(CharacterStats attacker, CharacterStats target, bool isCritical = false, bool weapon = true)
    {
        // 0. weapon random variance
        float WeaponRandomVariance = GetRandomVariance(attacker.Weapon);

        // 1. Basic Base Damage
        float baseDamage = ((1 + attacker.Attack * attacker.Strength) / 2); // max :( 256 * 256 ) / 2 = 32768 // a rare weapon will give 255 Attack but it will be a slow hammer and with have a huge variance of 0.01 - 2.0

        // 2. Apply Target Defense
        float damageAfterDefense = (baseDamage * (256 - target.Defense)) / 256f;

        // 3. Apply Buffs/Debuffs
        if (attacker.HasBravery)
            damageAfterDefense *= 1.3f; // Bravery +30%
        if (target.HasProtect)
            damageAfterDefense *= 0.7f; // Protect -30%

        // 4. Critical Hit
        if (isCritical)
            damageAfterDefense *= 1.5f; // Critical multiplier

        // 5. Apply Accuracy
        if (attacker.Range == WeaponRange.veryLongRange && (Random.value > (attacker.Accuracy / 100f))) // attacker.WeaponRange make an error, why ?
        {
            Debug.Log(attacker.name + " missed the attack!");
            return 0f; // Range Attack missed
        }

        // 6. Apply Evasion
        if (Random.value > (target.Evasion / 100f))
        {
            Debug.Log(target.name + " dodged the attack!");
            return 0f; // Attack missed
        }

        // 7. and 8 : Apply block chance, Final Damage with Random Variance
        float damageAfterVariance = damageAfterDefense * WeaponRandomVariance;
        if (Random.value < (target.ShieldBlockRate / 100f))
        {
            Debug.Log(target.name + " blocked the attack with shield!");
            return Mathf.Floor(damageAfterVariance * 0.33f); // Shield Block
        }
        if (weapon && (Random.value < (target.WeaponParadeRate / 100f)))
        {
            Debug.Log(target.name + " parried the attack with weapon!");
            return Mathf.Floor(damageAfterVariance * 0.5f); // Weapon Parade
        }
        else if (Random.value < (target.UnarmedParadeRate / 100f))
            return Mathf.Floor(damageAfterVariance * 0.67f); // Unarmed Parade

        return Mathf.Floor(damageAfterVariance); // No block, return damage
    }

    public static float CalculateMagicDamage(CharacterStats caster, CharacterStats target, Spell.SpellId spellId, Element.ElementType? overrideElement = null)
    {
        float spellDmg = Spell.GetSpellDamage(spellId); // #TODO: need to implement Spell.GetSpellDamage() or something like that.
        // 0. random variance of the spell
        float randomVariance = Random.Range(0.95f, 1.05f);

        // 1. Base Magic Damage
        float baseMagicDamage = ((caster.MagicAttack) + (caster.Magic)) / 2 * (spellDmg); //  max : 

        // 2. Apply Target Magic Defense
        float damageAfterMDEF = (baseMagicDamage * (256 - target.MagicDefense)) / 256f;

        // 3. Récupération des listes d'affinités du target
        var resistances = target.GetElementsByType("resistance");
        var immunities = target.GetElementsByType("immunity");
        var absorbtions = target.GetElementsByType("absorbtion");
        var faiblesses = target.GetElementsByType("weakness");
        var elements = target.GetElementsByType("element");

        // 4. Détermination de l'élément du sort (optionnel override)
        Element.ElementType elementType;
        if (overrideElement.HasValue)
            elementType = overrideElement.Value;
        else
            elementType = elements.Count > 0 ? elements[0] : Element.ElementType.None;

        // 5. Calcul des dégâts élémentaires via ElementalDamage
        float finalDamage = ElementInstance != null
            ? ElementInstance.ElementalDamage(
                spellId,
                damageAfterMDEF,
                elementType,
                resistances,
                immunities,
                absorbtions,
                faiblesses)
            : damageAfterMDEF;

        // 6. Application des buffs/débuffs magiques
        if (caster.HasFaith)
            finalDamage *= 1.3f;
        if (target.HasShell)
            finalDamage *= 0.7f;

        // 7. Application de la variance aléatoire
        return Mathf.Floor(finalDamage * randomVariance);
    }

    public static float CalculateHealAmount(CharacterStats caster, CharacterStats target, int healPower)
    {
        // 0. random variance of the spell
        float randomVariance = Random.Range(0.95f, 1.05f);
        // 1. Basic Base Heal Amount
        float baseHealAmount = ((caster.MagicAttack) + (caster.Magic))/2 * (healPower); // max heal : (256 * 256 / 2) * 256 = 65536, but only the omniHeal has 256 Heal Power and only one staff will have 256 Attack Power and he will be a slow staff with a variance of 0.01 - 1.5
        // 2. Apply Buffs
        if (caster.HasFaith)
            baseHealAmount *= 1.3f; // Faith +30%
        // 3. Final Heal Amount with Random Variance
        return Mathf.Floor(baseHealAmount * randomVariance);
    }

    private static float GetRandomVariance(WeaponType weaponType)
    {
        switch (weaponType)
        {
            // Hammer, Axe, BombLance weapons have a higher variance
            case WeaponType.Hammer:
            case WeaponType.Axe:
            case WeaponType.BombLance:
                return Random.Range(0.5f, 2.0f);
            default:
                return Random.Range(0.95f, 1.05f);
        }
    }

}

