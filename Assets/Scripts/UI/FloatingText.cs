using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // Référence au composant Text (UnityEngine.UI)

    private void Awake()
    {
        // Si textComponent n'est pas assigné, le rechercher automatiquement
        if (textComponent == null)
        {
            textComponent = GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent == null)
            {
                Debug.LogError("TextMeshProUGUI component is missing on FloatingText prefab.");
            }
        }
    }

    public void SetText(string text, Color color)
    {
        if (textComponent == null)
        {
            Debug.LogError("Text component is not assigned.");
            return;
        }

        textComponent.text = text;
        textComponent.color = color;
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Translate(Vector3.up * Time.deltaTime);
    }
}



