//using Ink;
using JetBrains.Annotations;
//using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Gambit;
using static GambitLists;
using static Inventory;
using static Player;
using static WorldState;
using static SaveSystemBin;
using static Character;
using System.Linq;


[System.Serializable]
public class GameData
{
    public string leaderCharacter; // Le personnage actuellement contrôlé

    public List<Character> characters = new List<Character>(); // Liste des personnages
    public List<CharacterDataGD> activeParty; // Liste des persos actifs
    public List<CharacterDataGD> reserveParty; // Les autres persos hors combat

    public InventoryData inventory; // inventaire des personnages
    public Stash stash; // Inventaire du joueur
    public WorldStateData worldState;
    public PlayerData player;
    public static SaveSystemBin saveSystemBin = new SaveSystemBin(); // Instance de la classe SaveSystemBin

    /// <summary>
    /// Constructeur de GameData
    /// </summary>
    /// <param name="player"></param>
    /// <param name="characters"></param>
    /// <param name="inventory"></param>
    /// <param name="worldState"></param>
    /// <param name="stash"></param>
    public GameData(PlayerData player, Characters characters, Inventory inventory, WorldState worldState, Stash stash)
    {
        if (characters == null || characters.playerCharacters.CountCharacterList() == 0)
        {
            Debug.LogError("Aucun personnage trouvé dans la liste.");
            this.characters = new List<Character>();
        }
        else
        {
            this.characters = characters.playerCharacters;
        }
        foreach (var character in characters.activeCharacters) // #ToDoExt: Ajout des persos actifs à la liste des perso actifs. Done?
        {
            this.activeParty.Add(new CharacterDataGD(character));
        }
        this.inventory = inventory != null ? new InventoryData(inventory) : new InventoryData(new Inventory());
        this.worldState = worldState != null ? new WorldStateData(worldState) : new WorldStateData(new WorldState());
        this.player = player != null ? player : new PlayerData(new Player());
        this.stash = stash != null ? stash : new Stash();
        this.leaderCharacter = characters.activeCharacters[0].characterName; // Le premier personnage actif est le leader
        this.activeParty = characters.activeCharacters.Select(c => new CharacterDataGD(c)).ToList();
        this.reserveParty = characters.inactiveCharacters.Select(c => new CharacterDataGD(c)).ToList();

    }

    public void SaveGameData(int slot)
    {
        SaveSystemBin saveSystem = new SaveSystemBin(); // Créez une instance si nécessaire
        saveSystem.SaveGame(this, slot); // Utilisez l'instance pour appeler la méthode
    }


    public void LoadGameData(int slot)
    {
        // #ToDo? : Mise à jour des propriétés de l'instance actuelle avec les données chargées.
        // Vérifiez si le slot est valide
        if (slot < 0 || slot > saveSystemBin.GetMaxSaveSlots())
        {
            Debug.LogError($"Slot de sauvegarde invalide : {slot}");
            return;
        }
        Debug.Log($"Tentative de chargement des données du slot {slot}...");
        SaveSystemBin saveSystem = new SaveSystemBin();
        GameData loadedData = saveSystem.LoadGame(slot);

        if (loadedData == null)
        {
            Debug.LogError($"Échec du chargement des données pour le slot {slot}. Aucune donnée trouvée ou erreur lors du chargement.");
        }
        else
        {
            Debug.Log($"Données chargées avec succès pour le slot {slot}.");
        }
    }
}

public class PlayerData
{
    public string playerName;
    public int gold;
    public List<string> items;
    public string area;
    public PlayerData(Player player)
    {
        playerName = player.playerName;
        gold = player.gold;
        items = new List<string>();
        area = player.area; // ex: "NameOfTheArea" or "NameOfTheArea/NameOfTheSubArea" or "NameOfTheScene"
    }
}

[System.Serializable]
public class CharacterDataGD
{
    public string characterName;
    public int level;
    public int xp;
    public int curHP;
    public int avHP;
    public int maxHP;
    public List<Gambit> gambits;
    //public GambitLists gambitLst;
    public string target;
    public string condition;
    public string action;
    public string equippedWeapon;
    public string equippedArmor;
    public float[] position;
    public bool isActive; // True si dans le groupe actif

    public CharacterDataGD(Character character)
    {

        characterName = character.characterName;
        level = character.level;
        xp = character.xp;
        curHP = character.curHP;
        avHP = character.avHP;
        maxHP = character.maxHP;
        gambits = new List<Gambit>();
        //target = gambitLst.Targets,
        foreach (var gambit in character.gambits)
        {
            gambits.Add(new Gambit(gambit.target, gambit.condition, gambit.action));

        }
        equippedWeapon = character.equippedWeapon;
        equippedArmor = character.equippedArmor;
        position = new float[3];
        position[0] = character.transform.position.x;
        position[1] = character.transform.position.y;
        position[2] = character.transform.position.z;
        isActive = character.isActive;


    }
}

[System.Serializable]
public class InventoryData
{
    public Dictionary<string, int> items = new Dictionary<string, int>(); // Item name & quantity
    public InventoryData(Inventory inventory)
    {
        items = inventory.items;
    }
}

[System.Serializable]
public class WorldStateData
{
    public string currentLocation;
    public string currentWeather;
    public float gameTime;

    public WorldStateData(WorldState worldState)
    {
        currentLocation = worldState.currentLocation;
        currentWeather = worldState.currentWeather;
        gameTime = worldState.gameTime;
    }
}

[System.Serializable]
public class GambitData
{
    public string target;
    public string condition;
    public string action;

    public GambitData(Gambit gambit)
    {
        target = gambit.target.ToString();
        condition = gambit.condition.ToString();
        action = gambit.action.ToString();
    }

    public Gambit ToGambit()
    {
        return new Gambit
        (
            (Gambit.Target)System.Enum.Parse(typeof(Gambit.Target), target),
            (Gambit.Condition)System.Enum.Parse(typeof(Gambit.Condition), condition),
            (Gambit.Action)System.Enum.Parse(typeof(Gambit.Action), action)
        );
    }

    public string where = "action"; // "action" ou "condition" ou "target"
    public string Where
    {
        get => where;
        set => where = value;
    }

    //[Button("log")]
    static void DebugLog(string where)
    {
        if (where == "action")
        {
            foreach (var action in GambitLists.Actions)
            {
                Debug.Log("Action possible: " + action);
            }
        }
        else if (where == "condition")
        {
            foreach (var condition in GambitLists.Conditions)
            {
                Debug.Log("Condition possible: " + condition);
            }
        }
        else if (where == "target")
        {
            foreach (var target in GambitLists.Targets)
            {
                Debug.Log("Cible possible: " + target);
            }
        }
    }
}

