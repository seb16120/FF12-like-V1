using System.Collections.Generic;
using UnityEngine;

public static class ItemLists
{
    public static readonly List<Item> Items = new List<Item>
    {
        new Item("Gold", int.MaxValue, ItemType.Miscellaneous), // Pas de limite pour l'or
        new Item("Potion", 99, ItemType.Consumable, "rend 50% des avHP", 1), // Limite de 99 potions
        new Item("Elixir", 10, ItemType.Consumable, "rend 100% des avHP et 100% des MP"), // Limite de 10 élixirs
        new Item("Wood Sword", 99, ItemType.Weapon, "Une épée en bois", 1), 
        new Item("Wood Shield", 99, ItemType.Shield, "un bouclier en bois", 1), 
        new Item("Ancient Relic", 1, ItemType.QuestItem, "une ancienne rélique, à redonner", 10) // Objet de quête
    };

    public static Item GetItemByName(string name)
    {
        return Items.Find(item => item.Name == name);
    }
    public static List<Item> GetItemsByType(ItemType type)
    {
        return Items.FindAll(item => item.Type == type);
    }
    public static bool IsUniqueItem(string name)
    {
        var item = GetItemByName(name);
        return item != null && item.MaxQuantity == 1;
    }
}
public enum ItemType
{
    Gold,
    Consumable,
    Weapon,
    Shield,
    QuestItem,
    Miscellaneous // Pour les objets divers
}
