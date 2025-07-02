using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class testingthings : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            var vaan = FindFirstObjectByType<CharacterStats>(); // or get Vaan reference directly
            FindFirstObjectByType<BattleMenuManager>().OpenMenu(vaan);
        }
    }

    public void BattleLog(CharacterStats attacker, CharacterStats target, int damage)
    {
        BattleLogManager.Instance.Write($"{attacker.name} hit {target.name} for {damage}!"); //BattleLog(vaan, enemy, calculatedDamage);
        BattleLogManager.Instance.BattleLog(attacker, target, damage); //BattleLog(vaan, enemy, calculatedDamage);


    }


}
