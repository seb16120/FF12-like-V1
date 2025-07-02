using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("Battle Settings")]
    public List<CharacterStats> allies;
    public CharacterStats currentCharacter; // The character currently selected in the battle menu
    public List<CharacterStats> enemies = new List<CharacterStats>();
    public CharacterStats currentEnemy; // The enemy currently selected in the battle menu
    public bool isBattleActive = false; // Is the battle active or not
    public float chargeMultiplier = 1.0f; // How fast the ChargeBar fills overall
    public float maxChargeTime = 100f; // Value where the bar is considered full
    public float chargeTime = 0f; // Current charge time of the character
    public float chargeBarSpeed = 1f; // Speed of the charge bar filling up
    public float chargeBarMax = 100f; // Maximum value of the charge bar
    public float chargeBarMin = 0f; // Minimum value of the charge bar
    public float chargeBarCurrent = 0f; // Current value of the charge bar


    [Header("Prefabs")]
    public GameObject floatingTextPrefab; // Floating Text prefab for "Blocked!", "Dodged!" etc. // #todo?



    private void Start()
    {
        SetAllies();
        SetCurrentCharacter(allies[0]); // Set the first ally as the current character
        // Set the list of enemies 
        enemies = GameObject.Find("Characters/Enemies").GetComponentsInChildren<CharacterStats>()
    .Where(c => c.Faction == Faction.Enemy)
    .ToList();
        // check if ennemies exist
        if (enemies.Count > 0)
        {
            Debug.Log("Enemies found: " + enemies.Count);
        }
        else
        {
            Debug.LogWarning("No enemies found in the scene.");
        }

    }
    private void Update()
    {
        HandleChargeBars();
    }

    public void SetAllies()
    {
        // Find all allies in the hierarchy under "Characters/Allies"
        GameObject alliesParent = GameObject.Find("Characters/Allies");
        if (alliesParent != null)
        {
            allies = alliesParent.GetComponentsInChildren<CharacterStats>()
                                 .Where(c => c.Faction == Faction.Ally)
                                 .ToList();
        }
        else
        {
            Debug.LogWarning("No 'Characters/Allies' GameObject found in the hierarchy.");
        }
    }
    public void SetCurrentCharacter(CharacterStats character)
    {
        currentCharacter = character;
    }
    public CharacterStats GetCurrentCharacter()
    {
        return currentCharacter;
    }
    public List<CharacterStats> GetAllies()
    {
        return allies;
    }

    public void SetNewEnemies(List<CharacterStats> newEnemies)
    {
        enemies = newEnemies;
    }
    public List<CharacterStats> GetEnemies()
    {
        return enemies;
    }

    public void AddEnemy(CharacterStats enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void RemoveEnemy(CharacterStats enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }
    public void ClearEnemies()
    {
        enemies.Clear();
    }


    public void AddAlly(CharacterStats ally)
    {
        if (!allies.Contains(ally))
        {
            allies.Add(ally);
        }
    }
    public void RemoveAlly(CharacterStats ally)
    {
        if (allies.Contains(ally))
        {
            allies.Remove(ally);
        }
    }
    public void ClearAllies()
    {
        allies.Clear();
    }

    public void Remove()
    {
        if (currentCharacter != null)
        {
            if (currentCharacter.Faction == Faction.Enemy)
            {
                RemoveEnemy(currentCharacter);

            }
            else if (currentCharacter.Faction == Faction.Ally)
            {
                RemoveAlly(currentCharacter);
            }
        }
        // Autres actions à effectuer lors de la défaite (par exemple, jouer une animation)
        //Destroy(gameObject);
    }

    private void HandleChargeBars()
    {
        foreach (CharacterStats ally in allies)
        {
            if (!ally.IsKO)
                ChargeCharacter(ally);
        }

        foreach (CharacterStats enemy in enemies)
        {
            if (!enemy.IsKO)
                ChargeCharacter(enemy);
        }
    }

    private void ChargeCharacter(CharacterStats character)
    {
        float weaponSpeedModifier = GetWeaponSpeedModifier(character.Weapon);
        float finalSpeed = (character.Speed + weaponSpeedModifier) * chargeMultiplier;
        character.CurrentChargeTime += finalSpeed * Time.deltaTime;

        if (character.CurrentChargeTime >= maxChargeTime)
        {
            character.CurrentChargeTime = maxChargeTime; // Clamp to max

            // Character ready to act
            Debug.Log(character.CharacterName + " is ready to act!");

            // Later: trigger BattleMenu or AI move depending on Faction
        }
    }

    private float GetWeaponSpeedModifier(WeaponType weapon)
    {
        switch (weapon)
        {
            // Hammer, Axe, GreatSword, Gun slow you down :
            case WeaponType.Gun:
            case WeaponType.Hammer:
            case WeaponType.Axe:
            case WeaponType.GreatSword:
                return -5f; // Heavy weapons slow you.
            // dagger, ninja sword, Bow speed you up :
            case WeaponType.Dagger:
            case WeaponType.Bow:
            case WeaponType.NinjaSword:
                return 5f; // Light weapons speed you up.
            // Staff, Pole, Spear, Sword, Katana, Instrument, Crossbow, BombLance are neutral :
            default:
                return 0f; // Neutral weapons.
        }
    }
    public void OnDefeated()
    {
        if (currentCharacter != null)
        {
            if (currentCharacter.IsKO)
            {
                // Afficher un message de défaite
                FloatingTextManager.Instance.ShowText("Defeated!", currentCharacter.transform.position, Color.red);
            }
            else
            {
                // Afficher un message de victoire
                FloatingTextManager.Instance.ShowText("Victory!", currentCharacter.transform.position, Color.green);
            }
        }

        // Autres actions à effectuer lors de la défaite (par exemple, jouer une animation)
        Destroy(gameObject);
    }
}
