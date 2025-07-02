using UnityEngine;
using System.Collections.Generic;

public interface ICombatHandler
{
    /// <summary>
    /// Récupère la liste des alliés disponibles.
    /// </summary>
    /// <returns>Une liste des alliés.</returns>
    List<CharacterStats> GetAllies();

    /// <summary>
    /// Récupère la liste des ennemis disponibles.
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
    /// Met à jour le combat.
    /// </summary>
    void Update();

}
