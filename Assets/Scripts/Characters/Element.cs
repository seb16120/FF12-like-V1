using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using static Spell;

public class Element : MonoBehaviour
{
    public static CharacterStats CharacterStats;
    // stat
    CharacterStats stats = CharacterStats.GetComponent<CharacterStats>();
    // dictionary for affinities :
    public Dictionary<string, List<Element.ElementType>> aff;
    // List of weaknesses :
    public List<Element.ElementType> faiblesses;
    // List of resistances :
    public List<Element.ElementType> resistances;
    // List of immunities :
    public List<Element.ElementType> immunities;
    // List of absorptions :
    public List<Element.ElementType> absorptions;
    // List of elements :
    public List<Element.ElementType> elements;

    public Dictionary<ElementType, ElementType> OppositeElement => _oppositeElement;

    public ElementType element;
    public ElementType spellElement;
    public static float DefaultDamageMultiplier = 1.0f; // default damage value, will be replace if needed by resistance or weakness
    public static float damageMultiplier = DefaultDamageMultiplier; // default Multiplier for damage calculation, will be replace if needed by resistance or weakness
    public float resistanceMultiplier = 0.5f; // Multiplier for resistance calculation
    public float weaknessMultiplier = 1.333f; // Multiplier for weakness calculation
    public float absorption = -1f; // Multiplier for absorption calculation
    public float immunity = 0f; // Multiplier for immunity calculation
    public float oppositeElementMultiplier = 1.5f; // Multiplier for opposite element calculation


    public enum ElementType
    {
        Fire,
        Water,
        Thunder,
        Ice,
        Earth,
        Air,
        Light,
        Dark,
        None
    }

    private readonly Dictionary<ElementType, ElementType> _oppositeElement = new Dictionary<ElementType, ElementType>
    {
        { ElementType.Fire, ElementType.Water },
        { ElementType.Water, ElementType.Fire },
        { ElementType.Thunder, ElementType.Earth },
        { ElementType.Earth, ElementType.Thunder },
        { ElementType.Air, ElementType.Ice },
        { ElementType.Ice, ElementType.Air },
        { ElementType.Light, ElementType.Dark },
        { ElementType.Dark, ElementType.Light }
    };




    private void Awake()
    {
        // Assure-toi que CharacterStats est bien assigné avant !
        if (CharacterStats != null)
        {
            // Initialize the affinities dictionary
            aff = CharacterStats.affinities;

            if (aff != null && aff.ContainsKey("element"))
            {
                elements = aff["element"];
            }
            else
            {
                Debug.LogWarning("element not found in affinities.");
            }

            if (aff != null && aff.ContainsKey("weakness"))
            {
                faiblesses = aff["weakness"];
            }
            else
            {
                Debug.LogWarning("Weaknesses not found in affinities.");
            }

            if (aff != null && aff.ContainsKey("resistance"))
            {
                resistances = aff["resistance"];
            }
            else
            {
                Debug.LogWarning("Resistances not found in affinities.");
            }

            if (aff != null && aff.ContainsKey("immunity"))
            {
                immunities = aff["immunity"];
            }
            else
            {
                Debug.LogWarning("Immunities not found in affinities.");
            }

            if (aff != null && aff.ContainsKey("absorption"))
            {
                absorptions = aff["absorption"];
            }
            else
            {
                Debug.LogWarning("Absorptions not found in affinities.");
            }
        }
    }


    public float ElementalDamage(SpellId spellId, float baseDmg, ElementType element, List<Element.ElementType> Resistances, List<Element.ElementType> Immunities, List<Element.ElementType> Absorbtions, List<Element.ElementType> Faiblesses) // List<ElementType> ElementResistance has been changed. ElementsAfinities need to be create in CharacterStats.
    {
        var prop = new Spell.ProprietiesOfTheSpell();
        if (prop.ProprietiesBySpell.TryGetValue(spellId, out var properties) && properties.Count > 0)
        {
            spellElement = (ElementType)properties[0];
        }
        else
        {
            spellElement = ElementType.None; // Valeur par défaut si non trouvé
        }

        // Calculate the elemental damage based on the element type and multipliers
        float finalDamage = baseDmg * damageMultiplier; // * 1 by default
        // Apply resistance or weakness if applicable
        if (IsWeakness(spellElement, Faiblesses))
        {
            finalDamage *= weaknessMultiplier; // * 1.333 : increase damage by 33%
        }
        // Apply absorption if applicable
        else if (IsAbsorption(spellElement, Absorbtions))
        {
            finalDamage *= absorption; // * -1 : heal the character
        }
        // Apply immunity if applicable
        else if (IsImmunity(spellElement, Immunities))
        {
            finalDamage *= immunity; // * 0 : no damage
        }
        // Apply resistance if applicable
        else if (IsResistance(spellElement, Resistances))
        {
            finalDamage *= resistanceMultiplier; // * 0.5 : reduce damage by half
        }
        // Apply opposite element multiplier if applicable
        else if (IsOppositeElement(spellElement, elements))
        {
            finalDamage *= oppositeElementMultiplier; // * 1.5 : increase damage by 50%
        }
        // if none of the above, apply the default damage multiplier
        else
        {
            finalDamage *= damageMultiplier; // * 1 : no change
        }

        return finalDamage; // return the final damage value : can be up to BaseDmg *1.5 * 1.333 if opposite element and weakness
    }

    public bool IsWeakness(ElementType SpellElement, List<Element.ElementType> Faiblesses)
    {
        // Vérifie si l'élément est une faiblesse du personnage
        return Faiblesses != null && Faiblesses.Contains(SpellElement);
    }
    public bool IsResistance(ElementType SpellElement, List<Element.ElementType> Resistances)
    {
        // Vérifie si l'élément est une résistance du personnage
        return Resistances != null && Resistances.Contains(SpellElement);
    }

    public bool IsAbsorption(ElementType SpellElement, List<Element.ElementType> Absorbtions)
    {
        // Check if the element is an absorption
        return Absorbtions != null && Absorbtions.Contains(SpellElement);
    }

    public bool IsImmunity(ElementType SpellElement, List<Element.ElementType> Immunities)
    {
        // Check if the element is an immunity
        return Immunities != null && Immunities.Contains(SpellElement);

    }

    public bool IsOppositeElement(ElementType SpellElement, List<Element.ElementType> Elements) // for elementary enemies like a spirit of air or a spirit of fire enemies
    {
        // Get the opposite element of the spell
        ElementType OppositeElementOfTheSpell = _oppositeElement[SpellElement];
        // Check if the element is the opposite element
        return Elements != null && Elements.Contains(OppositeElementOfTheSpell);
    }


}
