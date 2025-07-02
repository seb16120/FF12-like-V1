using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager Instance;
    public GameObject floatingTextPrefab; // #todo: assign in inspector
    public Transform floatingTextsContainer; // Référence au conteneur FloatingTexts
    public static Vector3 position = new Vector3(0, 0, 0); // Position par défaut

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Trouver ou créer le conteneur FloatingTexts
        if (floatingTextsContainer == null)
        {
            GameObject instance = Instantiate(floatingTextPrefab, position, Quaternion.identity);
            GameObject container = GameObject.Find("FloatingTexts");
            if (container == null)
            {
                container = new GameObject("FloatingTexts");
            }
            floatingTextsContainer = container.transform;
        }
    }


    public void ShowText(string message, Vector3 position, Color color)
    {
        // Instancier le prefab FloatingText
        GameObject floatingTextPrefab = GameObject.Find("FloatingText");

        // Vérifier si le prefab est assigné
        GameObject InsText = Instantiate(floatingTextPrefab, position, Quaternion.identity);
        TextMeshProUGUI textMesh = InsText.GetComponent<TextMeshProUGUI>();
        // Configurer le texte flottant
        FloatingText floatingText = InsText.GetComponent<FloatingText>();


        if (floatingTextPrefab == null)
        {
            Debug.LogError("FloatingTextPrefab is not assigned in the inspector.");
            return;
        }
        if (textMesh == null)
        {
            Debug.LogError("FloatingTextPrefab does not have a TextMeshProUGUI component.");
            Destroy(InsText);
            return;
        }
        if (InsText == null)
        {
            Debug.LogError("Failed to instantiate FloatingTextPrefab.");
            return;
        }
        // Définir le parent comme FloatingTexts
        if (floatingTextsContainer != null)
        {
            InsText.transform.SetParent(floatingTextsContainer);
        }

        if (string.IsNullOrEmpty(message))
        {
            Debug.LogWarning("Message is null or empty. No text will be displayed.");
            return;
        }

        if (position == null)
        {
            Debug.LogWarning("Position is null. Defaulting to Vector3.zero.");
            position = Vector3.zero;
        }


        if (color == null)
        {
            Debug.LogWarning("Color is null. Defaulting to white.");
            color = Color.white;
        }

        if (floatingText != null)
        {
            floatingText.SetText(textMesh.text, color);
        }
        else
        {
            Debug.LogError("Le script FloatingText est manquant sur l'instance.");
        }

        if (textMesh == null)
        {
            Debug.LogError("FloatingTextPrefab does not have a TextMesh component.");
            Destroy(InsText);
            return;
        }

        textMesh.text = message;
        textMesh.color = color;
        Destroy(InsText, 1.5f);
    }


    public void CreateFloatingText(string message, Transform location)
    {
        GameObject text = Instantiate(floatingTextPrefab, location.position + Vector3.up * 2, Quaternion.identity);
        text.GetComponent<TextMesh>().text = message;
        Destroy(text, 1.5f); // Auto destroy after 1.5 seconds
    }
}

