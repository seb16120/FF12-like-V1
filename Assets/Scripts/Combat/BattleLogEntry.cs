using TMPro;
using UnityEngine;

public class BattleLogEntry : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public TMP_Text messageText;

    public void SetMessage(string msg)
    {
        messageText.text = msg;
    }
}
