using UnityEngine;
using UnityEngine.UI;

public class ChargeBarUI : MonoBehaviour
{
    public CharacterStats linkedCharacter;
    public Image chargeFillImage;

    private void Update()
    {
        if (linkedCharacter == null) return;

        float chargePercent = linkedCharacter.CurrentChargeTime / 100f;
        chargeFillImage.fillAmount = Mathf.Clamp01(chargePercent);

        // Change Color depending on Charge %
        if (chargePercent < 0.8f)
            chargeFillImage.color = Color.blue;
        else if (chargePercent < 1f)
            chargeFillImage.color = Color.yellow;
        else
            chargeFillImage.color = Color.green;
    }
}

