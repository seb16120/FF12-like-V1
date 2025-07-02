using TMPro;
using UnityEngine;

public class BattleLogManager : MonoBehaviour
{
    public TextMeshProUGUI logText;
    public GameObject logEntryPrefab;
    public Transform logContainer;
    public static BattleLogManager Instance;


    private void Awake()
    {
        Instance = this;
    }
    public void LogMessage(string msg)
    {
        var entry = Instantiate(logEntryPrefab, logContainer); // Crée une nouvelle entrée visuelle.
        entry.GetComponent<BattleLogEntry>().SetMessage(msg);  // Configure le message.
    }
    public void Write(string message)
    {
        logText.text += message + "\n";
    }

    public void BattleLog(CharacterStats attacker, CharacterStats target, int damage)
    {
        BattleLogManager.Instance.Write($"{attacker.name} hit {target.name} for {damage}!"); //ex: BattleLogManager.BattleLog(vaan, enemy, calculatedDamage);

    }
}

