using UnityEngine;
using System.Collections.Generic;

public interface ICombatHandler
{
    /// <summary>
    /// R�cup�re la liste des alli�s disponibles.
    /// </summary>
    /// <returns>Une liste des alli�s.</returns>
    List<CharacterStats> GetAllies();

    /// <summary>
    /// R�cup�re la liste des ennemis disponibles.
    /// </summary>
    /// <returns>Une liste des ennemis.</returns>
    List<CharacterStats> GetEnemies();

    /// <summary>
    /// Effectue une attaque d'un personnage sur une cible.
    /// </summary>
    /// <param name="attacker">Le personnage qui attaque.</param>
    /// <param name="target">La cible de l'attaque.</param>
    void PerformAttack(CharacterStats attacker, CharacterStats target);

    /// <summary>
    /// Met � jour le combat.
    /// </summary>
    void Update();

}
