using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static SaveSystemBin;
using static GameData;
using static Inventory;
using static Player;
using static Gambit;
using static GambitLists;
using static Characters;
using static WorldState;

[System.Serializable]
public class Stash
{
    public Dictionary<string, int> items = new Dictionary<string, int>();

    public Stash()
    {
        // Initialisation avec quelques objets par défaut
        AddNItems("Gold", 1000);
        AddNItems("Potion", 5);
    }

    public void AddNItems(string itemName, int quantity)
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogError("Le nom de l'objet ne peut pas être vide.");
            return;
        }
        if (quantity <= 0)
        {
            Debug.LogError("La quantité doit être supérieure à zéro.");
            return;
        }
        // Vérifie si l'objet existe dans ItemLists:
        var item = ItemLists.GetItemByName(itemName);
        if (item == null)
        {
            Debug.LogError($"L'objet '{itemName}' n'existe pas dans ItemLists.");
            return;
        }

        if (items.ContainsKey(itemName))
        {
            items[itemName] += quantity;
            if (items[itemName] > item.MaxQuantity)
                items[itemName] = item.MaxQuantity; // Respecter la limite
        }
        else
        {
            items[itemName] = Mathf.Min(quantity, item.MaxQuantity);
        }
    }

    public bool RemoveNItems(string itemName, int quantity)
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogError("Le nom de l'objet ne peut pas être vide.");
            return false;
        }
        if (quantity <= 0)
        {
            Debug.LogError("La quantité doit être supérieure à zéro.");
            return false;
        }
        // Vérifie si l'objet existe dans ItemLists:
        if (items.ContainsKey(itemName) && items[itemName] >= quantity)
        {
            items[itemName] -= quantity;
            if (items[itemName] <= 0) items.Remove(itemName);
            return true;
        }
        return false;
    }

    public bool GetItemQuantity(string itemName, out int quantity) // Récupère la quantité d'un objet
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogError("Le nom de l'objet ne peut pas être vide.");
            quantity = 0;
            return false;
        }
        if (items.ContainsKey(itemName))
        {
            quantity = items[itemName];
            return true;
        }
        quantity = 0;
        return false;
    }

    public bool HasThisQuantityOfItem(string itemName, int quantity) // regarde si l'inventaire a un certain nombre d'objets
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogError("Le nom de l'objet ne peut pas être vide.");
            return false;
        }
        if (items.ContainsKey(itemName))
        {
            return items[itemName] >= quantity;
        }
        else
        {
            Debug.LogError($"L'objet '{itemName}' n'existe pas dans le stash.");
            return false;
        }
    }

    public bool HasItem(string itemName) // regarde si l'inventaire a un certain objet
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogError("Le nom de l'objet ne peut pas être vide.");
            return false;
        }
        return items.ContainsKey(itemName);
    }

    public List<string> GetAllItems() // Récupère tous les objets du stash
    {
        return items.Keys.ToList();
    }
    public void PrintStash() // Affiche le contenu du stash
    {
        foreach (var item in items)
        {
            Debug.Log($"Objet: {item.Key}, Quantité: {item.Value}");
        }
    }

    public void GetStashSize() // Récupère la taille du stash
    {
        Debug.Log($"Taille du stash: {items.Count}");
    }

    /*    public void ClearStash() // Vider le stash
        {
            items.Clear();
        }*/


    public void SaveStash(int slot)
    {
        // Sauvegarde le stash dans le slot spécifié // #Todo!
    }
    public void LoadStash(int slot)
    {
        GameData gameData = saveSystemBin.LoadGame(slot);
        gameData.LoadGameData(slot);
        if (gameData != null)
        {
            items = gameData.stash.items;
        }
        else
        {
            Debug.LogError("Erreur lors du chargement du stash.");
        }
    }
}
