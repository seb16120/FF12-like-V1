using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using static SaveSystemBin;
using static GameData;
using static PlayerData;
using static Inventory;
using static Stash;
using static Gambit;
using static GambitLists;
using static Characters;
using static WorldState;






public class Player : MonoBehaviour
{

    public int lastSaveSlot = SaveSystemBin.lastSaveLoaded;
    public SaveSystemBin saveSystemBin;
    public string playerName = "Player 1";
    public int gold = 0;
    public string area = "None";

    public void SavePlayer(int saveSlot, int slot)
    {
        if (saveSystemBin == null)
        {
            Debug.LogError("Le système de sauvegarde n'est pas initialisé !");
            return;
        }
        // Vérifie si le slot de sauvegarde est valide
        if (saveSlot < 0 || lastSaveSlot > saveSystemBin.GetMaxSaveSlots())
        {
            Debug.LogError("Slot de sauvegarde invalide !");
            return;
        }
        // Crée une instance de PlayerData à partir de l'objet Player actuel
        PlayerData playerData = new PlayerData(this);
        // Crée une instance de Characters à partir de l'objet Characters actuel
        Characters characters = null; // la liste des personnages existant dans characters.cs ou GameData.cs, je sais pas encore.
        // Crée une instance de Inventory à partir de l'objet Inventory actuel
        Inventory inventory = null; // liste des inventaires des personnages existants // #Todo!
        // Crée une instance de WorldState à partir de l'objet WorldState actuel
        WorldState worldState = null; // récupérer les donnée de WorldState.cs ou GameData.cs, je sais pas encore.
        // Crée une instance de Stash à partir de l'objet Stash actuel
        Stash stash = null; // récupérer les données deStash.cs ou GameData.cs, je sais pas encore.

        // Crée une instance de GameData et y associe les données du joueur
        GameData gameData = new GameData(playerData, characters, inventory, worldState, stash);

        // Sauvegarde les données dans le slot spécifié
        saveSystemBin.SaveGame(gameData, slot);
    }

    public void LoadPlayer(int slot)
    {
        // Vérifie si le slot de sauvegarde est valide
        if (slot < 0 || slot > saveSystemBin.GetMaxSaveSlots())
        {
            Debug.LogError("Slot de sauvegarde invalide !");
            return;
        }
        // #Todo? : Créer une instance de PlayerData à partir de l'objet Player actuel
        // Charge les données de jeu depuis le slot spécifié
        GameData gameData = saveSystemBin.LoadGame(slot);

        // Charge les données du joueur depuis GameData
        if (gameData != null && gameData.player != null)
        {
            playerName = gameData.player.playerName;
            gold = gameData.player.gold;
            area = gameData.player.area;
        }
        else
        {
            Debug.LogWarning("Aucune donnée de joueur trouvée dans le slot spécifié.");
        }
    }

    #region UI Methods



    public void changeGold(int amount)
    {
        gold += amount;
    }

    #endregion
}
