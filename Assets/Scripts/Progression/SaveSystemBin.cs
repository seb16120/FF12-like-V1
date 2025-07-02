using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

public class SaveSystemBin : MonoBehaviour
{
    public static SaveSystemBin instance;
    private string saveDirectory;
    private const int maxSaveSlots = 12;
    // Méthode publique pour accéder à maxSaveSlots
    public int GetMaxSaveSlots()
    {
        return maxSaveSlots;
    }
    //public string SaveDirectory;
    public static int lastSaveLoaded;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        saveDirectory = Path.Combine(Application.persistentDataPath, "saves");
        if (!Directory.Exists(saveDirectory))
            Directory.CreateDirectory(saveDirectory);
    }

    public void SaveGame(GameData data, int slot)
    {
        if (data == null)
        {
            Debug.LogError("GameData is null. Cannot save.");
            return;
        }
        if (slot < 0 || slot > maxSaveSlots) return;

        string savePath = GetSavePath(slot);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                formatter.Serialize(stream, data);
            }
            Debug.Log($"Game Saved to Slot {slot}: {savePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving game: {e.Message}");
        }
    }

    public GameData LoadGame(int slot)
    {
        if (slot < 0 || slot > maxSaveSlots || !SaveExists(slot)) return null;
        if (slot == 0) // Slot 0 = Quick Auto-Save
        {
            // ToDo?
            Debug.Log("Loading Auto-Save...");
        }

        string savePath = GetSavePath(slot);
        lastSaveLoaded = slot;
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(savePath, FileMode.Open))
            {
                return (GameData)formatter.Deserialize(stream);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading game: {e.Message}");
            // Optionnel : On pourrait également supprimer le fichier de sauvegarde corrompu ici
            return null;
        }
    }

    public void AutoSave(GameData data)
    {
        SaveGame(data, 0); // Slot 0 = Quick Auto-Save
    }

    public bool SaveExists(int slot) => File.Exists(GetSavePath(slot));

    private string GetSavePath(int slot) => Path.Combine(saveDirectory, $"save_slot_{slot}.save");

    // Where is stream.Close()? does it need to be called or is it handled by the using statement?
    // -> The using statement will automatically call the Dispose method on the stream object when it goes out of scope.
}
