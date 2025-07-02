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
        // Initialisation des valeurs par d�faut
        currentLocation = "Unknown";
        currentWeather = "Clear";
        gameTime = 0.0f;
    }

    // Vous pouvez ajouter des m�thodes pour mettre � jour l'�tat du monde si n�cessaire
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

