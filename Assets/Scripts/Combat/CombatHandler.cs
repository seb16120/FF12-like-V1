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
            Debug.LogWarning("Une autre instance de CombatHandler existe déjà. Destruction de l'objet actuel.");
            Object.Destroy(this.gameObject); // Détruit l'objet actuel
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        // Récupérer une instance de BattleManager
        battleManager = Object.FindFirstObjectByType<BattleManager>();
        if (battleManager == null)
        {
            Debug.LogError("BattleManager introuvable. Assurez-vous qu'il est présent dans la scène.");
        }

        // Vérifier FloatingTextManager
        if (FloatingTextManager.Instance == null)
        {
            Debug.LogError("FloatingTextManager.Instance est null. Assurez-vous qu'il est initialisé.");
        }

        // Récupérer tous les personnages
        if (allCharacters == null || allCharacters.Count == 0)
        {
            allCharacters = battleManager?.allies.Concat(battleManager?.enemies).ToList();
            if (allCharacters == null || allCharacters.Count == 0)
            {
                Debug.LogWarning("Aucun personnage trouvé dans BattleManager.");
            }
        }
        else
        {
            Debug.Log("Tous les personnages sont déjà initialisés.");
        }

        foreach (var character in allCharacters)
        {
            character.AreGambitsEnabled = true; // Activer les gambits par défaut
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
                Debug.LogWarning($"Gambits désactivés pour {character.CharacterName}.");
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
        // Filtrer les personnages qui appartiennent à la même faction que le joueur
        return allCharacters.Where(character => character.Faction == Faction.Ally).ToList();
    }
    public List<CharacterStats> GetEnemies()
    {
        // Récupérer les ennemis via BattleManager
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
        // Vérification des paramètres
        if (attacker == null || target == null)
        {
            Debug.LogError("Attacker or target is null. Cannot perform attack.");
            return;
        }

        // Calcul des dégâts via DamageCalculator
        int damage = (int)DamageCalculator.CalculatePhysicalDamage(attacker, target);

        // Appliquer les dégâts à la cible
        target.TakeDamage(damage);

        // vérifier si la cible est KO
        if (target.CurrentHP <= 0)
        {
            target.IsKO = true;
            Debug.Log($"{target.CharacterName} est KO.");
        }

        // Afficher un texte flottant pour les dégâts
        FloatingTextManager.Instance.ShowText(
            damage.ToString(),
            target.transform.position + Vector3.up * 2,
            Color.red
        );
    }

}
