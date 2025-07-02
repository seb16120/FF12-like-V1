using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using Unity.Mathematics;
using static SaveSystemBin;
using Unity.VisualScripting;
using static GameData;
using System.Linq;
using static Stash;

// Path: Assets/Scripts/Characters/Characters.cs

[System.Serializable]
public class Character : MonoBehaviour
{
    public string characterName;
    public int level = 1;
    public int maxHP = 100;
    public int avHP = 100; // avaivable HP
    public int curHP = 100; // current HP
    public int xp = 0;
    public float[] position = new float[3] { 0, 0, 0 };
    public float[] velocity = new float[3] { 0, 0, 0 };
    public List<Gambit> gambits;
    public Gambit NewGambit = new Gambit(Gambit.Target.ClosestEnemy, Gambit.Condition.None, Gambit.Action.Attack);

    public Inventory Inventory { get; set; }
    public string equippedWeapon;
    public string equippedArmor;
    public Element.ElementType element;
    public bool isActive = false;
    public bool IsKO => curHP <= 0;


    public List<Gambit> AddGambit()
    {
        if (gambits == null)
        {
            gambits = new List<Gambit>();
        }
        gambits.Add(NewGambit);
        return gambits;
    }

    public void RemoveGambit(Gambit gambit)
    {
        if (gambits != null && gambits.Contains(gambit))
        {
            gambits.Remove(gambit);
        }
    }

    public void RemoveGambit(int index)
    {
        if (gambits != null && index >= 0 && index < gambits.Count)
        {
            gambits.RemoveAt(index);
        }
    }

    public void EquipWeapon(string weapon)
    {
        equippedWeapon = weapon;
    }
    public void EquipArmor(string armor)
    {
        equippedArmor = armor;
    }
    public void UnequipWeapon()
    {
        equippedWeapon = null;
    }
    public void UnequipArmor()
    {
        equippedArmor = null;
    }
    public void Move(float x, float y, float z)
    {
        position[0] += x;
        position[1] += y;
        position[2] += z;
    }
    public void SetPosition(float x, float y, float z)
    {
        position[0] = x;
        position[1] = y;
        position[2] = z;
    }
    public void SetVelocity(float x, float y, float z)
    {
        velocity[0] = x;
        velocity[1] = y;
        velocity[2] = z;
    }
    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }
    public void SetInactive()
    {
        isActive = false;
    }


    #region UI Methods

    public void changeLevel(int characterIndex, int amount, List<Character> playerCharacters)
    {
        if (characterIndex >= 0 && characterIndex < playerCharacters.Count)
        {
            playerCharacters[characterIndex].level += amount;
            if (playerCharacters[characterIndex].level < 1)
            {
                playerCharacters[characterIndex].level = 1;
            }
            else if (playerCharacters[characterIndex].level > 99)
            {
                playerCharacters[characterIndex].level = 99;
            }
        }
    }

    public void changeHP(int characterIndex, int amount, List<Character> playerCharacters)
    {
        if (characterIndex >= 0 && characterIndex < playerCharacters.Count)
        {
            playerCharacters[characterIndex].curHP += amount;
            if (playerCharacters[characterIndex].curHP > playerCharacters[characterIndex].avHP)
            {
                playerCharacters[characterIndex].curHP = playerCharacters[characterIndex].avHP;
            }
            else if (playerCharacters[characterIndex].curHP < 0)
            {
                playerCharacters[characterIndex].curHP = 0;
                // playerCharacters[characterIndex] is K.O; // TODO: Implement K.O. logic
            }
        }
    }

    public void changeAvHP(int characterIndex, int amount, List<Character> playerCharacters)
    {
        if (characterIndex >= 0 && characterIndex < playerCharacters.Count)
        {
            playerCharacters[characterIndex].avHP += amount;
            if (playerCharacters[characterIndex].avHP > playerCharacters[characterIndex].maxHP)
            {
                playerCharacters[characterIndex].avHP = playerCharacters[characterIndex].maxHP;
            }
        }
    }

    public void changeMaxHP(int characterIndex, int amount, List<Character> playerCharacters)
    {
        if (characterIndex >= 0 && characterIndex < playerCharacters.Count)
        {
            playerCharacters[characterIndex].maxHP += amount;
            if (playerCharacters[characterIndex].maxHP < 1)
            {
                playerCharacters[characterIndex].maxHP = 1;
            }
            else if (playerCharacters[characterIndex].maxHP > 65536)
            {
                playerCharacters[characterIndex].maxHP = 65536;
            }
        }
    }

    public void changeXP(int characterIndex, int amount, List<Character> playerCharacters)
    {
        if (characterIndex >= 0 && characterIndex < playerCharacters.Count)
        {
            playerCharacters[characterIndex].xp += amount;
        }
    }
    public void HandleKO(Character character)
    {
        if (character.IsKO)
        {
            // Déclenchez l'animation de chute // #TodoLater
            Debug.Log($"{character.characterName} est K.O. !");
        }
    }

/*    public static int CountCharacters(this IEnumerable<Character> characters)
    {
        return characters?.Count() ?? 0; // Utilise LINQ pour compter les éléments
    }*/

    #endregion
}



// crée une liste de personnages. // #TODO? : quid pour les persos actifs et inactifs ? ? ?
[System.Serializable]
public class Characters : MonoBehaviour, IEnumerable<Character>
{
    //public Player player = new Player();
    public Inventory inventory = new Inventory();
    public Stash stash = new Stash();
    public WorldState worldState = new WorldState();
    public List<Character> playerCharacters = new List<Character>();

    public List<Character> activeCharacters = new List<Character>();
    public List<Character> inactiveCharacters = new List<Character>();
    /// <summary>
    /// Active un personnage en le déplaçant des inactifs vers les actifs.
    /// </summary>
    public void ActivateCharacter(Character character)
    {
        if (inactiveCharacters.Contains(character))
        {
            inactiveCharacters.Remove(character);
            activeCharacters.Add(character);
        }
        else
        {
            Debug.LogWarning("Le personnage n'est pas dans la liste des inactifs.");
        }
    }
    /// <summary>
    /// Désactive un personnage en le déplaçant des actifs vers les inactifs.
    /// </summary>
    public void DeactivateCharacter(Character character)
    {
        if (activeCharacters.Contains(character))
        {
            activeCharacters.Remove(character);
            inactiveCharacters.Add(character);
        }
        else
        {
            Debug.LogWarning("Le personnage n'est pas dans la liste des actifs.");
        }
    }
    /// <summary>
    /// Vérifie si un personnage est actif.
    /// </summary>
    public bool IsCharacterActive(Character character)
    {
        return activeCharacters.Contains(character);
    }

    public void SyncActiveInactiveCharacters()
    {
        activeCharacters = playerCharacters.Where(c => !inactiveCharacters.Contains(c)).ToList();
        inactiveCharacters = playerCharacters.Where(c => !activeCharacters.Contains(c)).ToList();
    }

    // Implémentation de GetEnumerator pour IEnumerable<Character>
    public IEnumerator<Character> GetEnumerator()
    {
        return playerCharacters.GetEnumerator();
    }
    // Implémentation explicite pour IEnumerable
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public static Character instanceOfCharacter = new Character();
    public int saveSlot = SaveSystemBin.lastSaveLoaded;
    public SaveSystemBin saveSystemBin;

    // Ajoutez une méthode statique dans la classe Characters pour convertir une List<Character> en un objet Characters.
    /// <summary>
    /// Convertit une liste de personnages en un objet Characters.
    /// </summary>
    /// <param name="characterList"></param>
    /// <returns></returns>
    public static Characters FromCharacterList(List<Character> characterList)
    {
        Characters characters = new Characters();
        characters.playerCharacters = characterList;
        return characters;
    }

    // Exemple d'utilisation pour convertir une List<Character> en Characters avant de l'utiliser avec GameData.
    public static Characters CastCharacterListIntoCharacters(List<Character> playerCharacters)
    {
        //Characters characters = new Characters();
        Characters characters = Characters.FromCharacterList(playerCharacters);
        characters.playerCharacters = playerCharacters;
        return characters;
    }

    

    public void SaveCharacters(Characters characters, PlayerData player)
    {
        if (characters != null && inventory != null && worldState != null)
        {
            GameData data = new GameData(player, characters, inventory, worldState, stash);
            saveSystemBin.SaveGame(data, saveSlot);
        }
        else
        {
            Debug.LogError("Une ou plusieurs données nécessaires sont nulles !");
        }
    }

    public void LoadCharacters(int slot)
    {
        GameData gameData = saveSystemBin.LoadGame(slot);
        gameData.LoadGameData(slot);
        if (gameData != null)
        {
            playerCharacters.Clear();
            List<Character> charactersList = gameData.characters;
            // Assurez-vous que charactersList n'est pas null avant de l'utiliser
            if (charactersList == null)
            {
                Debug.LogError("charactersList est null !");
                return;
            }
            // Vérifiez si la liste est vide
            if (charactersList.Count == 0)
            {
                Debug.LogError("charactersList est vide !");
                return;
            }
            // Vérifiez si la liste contient des éléments
            if (charactersList.Count > 0)
            {
                Debug.Log($"charactersList contient {charactersList.Count} éléments.");
            }
            else
            {
                Debug.LogError("charactersList est vide !");
                return;
            }

            // Ajoutez les personnages chargés à la liste playerCharacters
            foreach (var data in charactersList)
            {
                Character character = new Character();
                character.characterName = data.characterName;
                character.level = data.level;
                character.maxHP = data.maxHP;
                character.avHP = data.avHP;
                character.curHP = data.curHP;
                character.xp = data.xp;
                character.position[0] = data.position[0];
                character.position[1] = data.position[1];
                character.position[2] = data.position[2];
                playerCharacters.Add(character);
            }
        }
    }
}

public static class CharacterExtensions
{
    /// <summary>
    /// Méthode d'extension pour obtenir le nombre de personnages dans une liste de Character.
    /// </summary>
    /// <param name="characters">La liste des personnages.</param>
    /// <returns>Le nombre de personnages dans la liste.</returns>
    public static int CountCharacterList(this List<Character> characters)
    {
        return characters?.Count ?? 0;
    }

    public static int CountCharacters(this IEnumerable<Character> characters)
    {
        return characters?.Count() ?? 0; // Utilise LINQ pour compter les éléments
    }


}

