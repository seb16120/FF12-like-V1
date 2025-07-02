using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CombatHandler : MonoBehaviour, ICombatHandler
{
    public static CombatHandler Instance { get; private set; }

    private BattleManager battleManager;
    private List<CharacterStats> allCharacters;
    private Faction characterFaction; // Faction du personnage

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Une autre instance de CombatHandler existe d�j�. Destruction de l'objet actuel.");
            Object.Destroy(this.gameObject); // D�truit l'objet actuel
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        // R�cup�rer une instance de BattleManager
        battleManager = Object.FindFirstObjectByType<BattleManager>();
        if (battleManager == null)
        {
            Debug.LogError("BattleManager introuvable. Assurez-vous qu'il est pr�sent dans la sc�ne.");
        }

        // V�rifier FloatingTextManager
        if (FloatingTextManager.Instance == null)
        {
            Debug.LogError("FloatingTextManager.Instance est null. Assurez-vous qu'il est initialis�.");
        }

        // R�cup�rer tous les personnages
        if (allCharacters == null || allCharacters.Count == 0)
        {
            allCharacters = battleManager?.allies.Concat(battleManager?.enemies).ToList();
            if (allCharacters == null || allCharacters.Count == 0)
            {
                Debug.LogWarning("Aucun personnage trouv� dans BattleManager.");
            }
        }
        else
        {
            Debug.Log("Tous les personnages sont d�j� initialis�s.");
        }

        foreach (var character in allCharacters)
        {
            character.AreGambitsEnabled = true; // Activer les gambits par d�faut
        }

    }
    public void Update()
    {
        foreach (var character in allCharacters)
        {
            if (character.AreGambitsEnabled)
            {
                character.ExecuteGambits();
            }
            else
            {
                Debug.LogWarning($"Gambits d�sactiv�s pour {character.CharacterName}.");
            }
        }
    }
    

    /// <summary>
    /// Effectue une attaque d'un personnage sur une cible.
    /// </summary>
    /// <param name="attacker">Le personnage qui attaque.</param>
    /// <param name="target">La cible de l'attaque.</param>

    public CombatHandler(List<CharacterStats> characters, Faction faction)
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of CombatHandler detected.");
            return;
        }
        Instance = this;
        allCharacters = characters;
        characterFaction = faction;
    }
    public List<CharacterStats> GetAllies()
    {
        // Filtrer les personnages qui appartiennent � la m�me faction que le joueur
        return allCharacters.Where(character => character.Faction == Faction.Ally).ToList();
    }
    public List<CharacterStats> GetEnemies()
    {
        // R�cup�rer les ennemis via BattleManager
        battleManager = Object.FindFirstObjectByType<BattleManager>();
        if (battleManager == null || battleManager.enemies == null || battleManager.enemies.Count == 0)
        {
            Debug.LogWarning("No enemies available in BattleManager.");
            return allCharacters.Where(character => character.Faction == Faction.Enemy).ToList();
        }
        return battleManager.enemies;
    }
    public void PerformAttack(CharacterStats attacker, CharacterStats target)
    {
        // V�rification des param�tres
        if (attacker == null || target == null)
        {
            Debug.LogError("Attacker or target is null. Cannot perform attack.");
            return;
        }

        // Calcul des d�g�ts via DamageCalculator
        int damage = (int)DamageCalculator.CalculatePhysicalDamage(attacker, target);

        // Appliquer les d�g�ts � la cible
        target.TakeDamage(damage);

        // v�rifier si la cible est KO
        if (target.CurrentHP <= 0)
        {
            target.IsKO = true;
            Debug.Log($"{target.CharacterName} est KO.");
        }

        // Afficher un texte flottant pour les d�g�ts
        FloatingTextManager.Instance.ShowText(
            damage.ToString(),
            target.transform.position + Vector3.up * 2,
            Color.red
        );
    }

}
