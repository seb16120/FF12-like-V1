using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class WorldState : MonoBehaviour
{
    public string currentLocation;
    public string currentWeather;
    public float gameTime;

    public WorldState()
    {
        // Initialisation des valeurs par défaut
        currentLocation = "Unknown";
        currentWeather = "Clear";
        gameTime = 0.0f;
    }

    // Vous pouvez ajouter des méthodes pour mettre à jour l'état du monde si nécessaire
    public event Action<string> OnLocationChanged;
    public event Action<string> OnWeatherChanged;

    public void UpdateLocation(string newLocation)
    {
        currentLocation = newLocation;
        OnLocationChanged?.Invoke(newLocation);
    }

    public void UpdateWeather(string newWeather)
    {
        currentWeather = newWeather;
        OnWeatherChanged?.Invoke(newWeather);
    }

    public void UpdateGameTime(float newGameTime)
    {
        gameTime = newGameTime;
    }
}

