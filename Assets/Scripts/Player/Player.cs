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
            Debug.LogError("Le syst�me de sauvegarde n'est pas initialis� !");
            return;
        }
        // V�rifie si le slot de sauvegarde est valide
        if (saveSlot < 0 || lastSaveSlot > saveSystemBin.GetMaxSaveSlots())
        {
            Debug.LogError("Slot de sauvegarde invalide !");
            return;
        }
        // Cr�e une instance de PlayerData � partir de l'objet Player actuel
        PlayerData playerData = new PlayerData(this);
        // Cr�e une instance de Characters � partir de l'objet Characters actuel
        Characters characters = null; // la liste des personnages existant dans characters.cs ou GameData.cs, je sais pas encore.
        // Cr�e une instance de Inventory � partir de l'objet Inventory actuel
        Inventory inventory = null; // liste des inventaires des personnages existants // #Todo!
        // Cr�e une instance de WorldState � partir de l'objet WorldState actuel
        WorldState worldState = null; // r�cup�rer les donn�e de WorldState.cs ou GameData.cs, je sais pas encore.
        // Cr�e une instance de Stash � partir de l'objet Stash actuel
        Stash stash = null; // r�cup�rer les donn�es deStash.cs ou GameData.cs, je sais pas encore.

        // Cr�e une instance de GameData et y associe les donn�es du joueur
        GameData gameData = new GameData(playerData, characters, inventory, worldState, stash);

        // Sauvegarde les donn�es dans le slot sp�cifi�
        saveSystemBin.SaveGame(gameData, slot);
    }

    public void LoadPlayer(int slot)
    {
        // V�rifie si le slot de sauvegarde est valide
        if (slot < 0 || slot > saveSystemBin.GetMaxSaveSlots())
        {
            Debug.LogError("Slot de sauvegarde invalide !");
            return;
        }
        // #Todo? : Cr�er une instance de PlayerData � partir de l'objet Player actuel
        // Charge les donn�es de jeu depuis le slot sp�cifi�
        GameData gameData = saveSystemBin.LoadGame(slot);

        // Charge les donn�es du joueur depuis GameData
        if (gameData != null && gameData.player != null)
        {
            playerName = gameData.player.playerName;
            gold = gameData.player.gold;
            area = gameData.player.area;
        }
        else
        {
            Debug.LogWarning("Aucune donn�e de joueur trouv�e dans le slot sp�cifi�.");
        }
    }

    #region UI Methods



    public void changeGold(int amount)
    {
        gold += amount;
    }

    #endregion
}
