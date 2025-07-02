using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using System.IO;
//using Newtonsoft.Json;
using static SaveSystemBin;
using static GameData;
using static Player;
using static Stash;
using static Gambit;
using static GambitLists;
using static Characters;
using static WorldState;



public class Inventory
{
    public Dictionary<string, int> items = new Dictionary<string, int>();

    public Inventory()
    {
        // Initialisation de l'inventaire avec quelques objets
        items.Add("Potion", 10);
        items.Add("Sword", 1);
        items.Add("Shield", 1);
    }

    public int GetItemQuantity(string itemName) // Récupère la quantité d'un objet
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogError("Item name cannot be empty.");
            return 0;
        }
        if (items.ContainsKey(itemName))
        {
            return items[itemName];
        }
        else
        {
            Debug.LogError($"Item {itemName} not found in inventory.");
            return 0;
        }
    }

    public bool HasItem(string itemName, int quantity) // Vérifie si l'inventaire a un objet
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogError("Item name cannot be empty.");
            return false;
        }
        if (items.ContainsKey(itemName))
        {
            return items[itemName] >= quantity;
        }
        else
        {
            Debug.LogError($"Item {itemName} not found in inventory.");
            return false;
        }
    }

    public void RemoveNItems(string itemName, int quantity) // Retire un objet de l'inventaire
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogError("Item name cannot be empty.");
            return;
        }
        if (items.ContainsKey(itemName))
        {
            if (items[itemName] >= quantity)
            {
                items[itemName] -= quantity;
                if (items[itemName] <= 0)
                {
                    items.Remove(itemName);
                }
            }
            else
            {
                Debug.LogError($"Not enough {itemName} in inventory.");
            }
        }
        else
        {
            Debug.LogError($"Item {itemName} not found in inventory.");
        }
    }

    public void RemoveItem(string itemName)
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogError("Item name cannot be empty.");
            return;
        }
        if (items.ContainsKey(itemName))
        {
            items.Remove(itemName);
        }
        else
        {
            Debug.LogError($"Item {itemName} not found in inventory.");
        }
    }

    public void addNitems(string itemName, int quantity) // Ajoute un objet à l'inventaire
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogError("Item name cannot be empty.");
            return;
        }
        if (quantity <= 0)
        {
            Debug.LogError("Quantity must be greater than zero.");
            return;
        }
        if (items.ContainsKey(itemName))
        {
            items[itemName] += quantity;
        }
        else
        {
            items[itemName] = quantity;
        }
    }

    public void ClearInventory() // Vide l'inventaire
    {
        items.Clear();
    }

    public void PrintInventory() // Affiche l'inventaire
    {
        foreach (var item in items)
        {
            Debug.Log($"Item: {item.Key}, Quantity: {item.Value}");
        }
    }


    // save and load JS:
/*    public void SaveInventoryJS() // Sauvegarde l'inventaire
    {
        string json = JsonConvert.SerializeObject(items); // Utilisation de JsonConvert.SerializeObject
        File.WriteAllText("inventory.json", json);
    }
    public void LoadInventoryJS() // Charge l'inventaire
    {
        if (File.Exists("inventory.json"))
        {
            string json = File.ReadAllText("inventory.json");
            items = JsonConvert.DeserializeObject<Dictionary<string, int>>(json); // Utilisation de JsonConvert.DeserializeObject
        }
        else
        {
            Debug.LogError("Inventory file not found.");
        }
    }*/

    public void SaveInventory(int slot) // Sauvegarde l'inventaire
    {
        GameData gameData = saveSystemBin.LoadGame(slot);
        gameData.SaveGameData(slot);
        if (gameData != null)
        {
            // Créer une instance de InventoryData et y associer l'inventaire
            InventoryData inventoryData = new InventoryData(this);
            gameData.inventory = inventoryData;
            saveSystemBin.SaveGame(gameData, slot);
            Debug.Log("Inventory saved successfully.");
        }
        else
        {
            Debug.LogError("Failed to save inventory data.");
        }
    }

    public void LoadInventory(int slot) // Charge l'inventaire
    {
        GameData gameData = saveSystemBin.LoadGame(slot);
        gameData.LoadGameData(slot);
        if (gameData != null)
        {
            // Récupérer l'inventaire du personnage
            InventoryData inventoryData = gameData.inventory;
            if (inventoryData != null)
            {
                items = inventoryData.items;
                Debug.Log("Inventory loaded successfully.");
            }
            else
            {
                Debug.LogError("Failed to load inventory data.");
            }
        }
        else
        {
            Debug.LogError("Failed to load game data.");
        }
    }
    public void AssignInventoryToCharacter(Character character)
    {
        character.Inventory = this; // Associe cet inventaire au personnage
    }
}
