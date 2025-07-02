using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string Name { get; private set; }
    public int MaxQuantity { get; private set; }
    public ItemType Type { get; private set; }
    public string Description { get; private set; } // Nouvelle propriété
    public int Rarity { get; private set; } // Nouvelle propriété (par exemple, 1 = commun, 10 = légendaire)

    public Item(string name, int maxQuantity, ItemType type, string description = " ", int rarity = 1)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("Item name cannot be empty.");
            return;
        }
        Name = name;

        if (maxQuantity <= 0)
        {
            Debug.LogError($"Max quantity for {name} must be greater than 0.");
            MaxQuantity = 1; // Valeur par défaut
        }
        MaxQuantity = maxQuantity;

        if (string.IsNullOrEmpty(description))
        {
            Debug.LogError($"Description for {name} cannot be empty.");
            Description = "No description available.";
        }
        Description = description;

        if (rarity < 1 || rarity > 10)
        {
            Debug.LogError($"Rarity for {name} must be between 1 and 10.");
            rarity = 1; // Valeur par défaut
        }
        Rarity = rarity;

        if (type is not ItemType.Consumable and not ItemType.Weapon and not ItemType.Shield and not ItemType.QuestItem and not ItemType.Miscellaneous)
        {
            Debug.LogError($"Invalid item type for {name}. Defaulting to Miscellaneous.");
            type = ItemType.Miscellaneous; // Valeur par défaut
        }
        Type = type;
    }
}
