using UnityEngine;
using TMPro;

public class UnitInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI MPText;
    public TextMeshProUGUI FactionText;

    public void UpdatePanel(CharacterStats stats)
    {
        NameText.text = $"Name: {stats.CharacterName}";
        HPText.text = $"HP: {stats.CurrentHP}/{stats.MaxHP}";
        MPText.text = $"MP: {stats.CurrentMP}/{stats.MaxMP}";
        FactionText.text = $"Faction: {stats.Faction}";
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }
}
