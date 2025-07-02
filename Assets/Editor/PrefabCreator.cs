#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

public class PrefabCreator : MonoBehaviour
{
    [MenuItem("Custom Tools/Create FloatingText Prefab")]
    static void CreateFloatingText()
    {
        GameObject go = new GameObject("FloatingText");
        TextMeshPro tmpro = go.AddComponent<TextMeshPro>();
        tmpro.text = "Sample Text";
        tmpro.fontSize = 5;
        tmpro.alignment = TextAlignmentOptions.Center;
        go.transform.localScale = Vector3.one * 0.2f;
        go.AddComponent<FloatingText>();

        string localPath = "Assets/Prefabs/UI/FloatingText.prefab";
        PrefabUtility.SaveAsPrefabAsset(go, localPath);
        DestroyImmediate(go);

        Debug.Log("FloatingText Prefab created at " + localPath);
    }

    [MenuItem("Custom Tools/Create ChargeBarCanvas Prefab")]
    static void CreateChargeBarCanvas()
    {
        GameObject canvasGo = new GameObject("ChargeBarCanvas");
        Canvas canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        CanvasScaler scaler = canvasGo.AddComponent<CanvasScaler>();
        canvasGo.AddComponent<GraphicRaycaster>();
        canvasGo.transform.localScale = Vector3.one * 0.01f;

        GameObject imageGo = new GameObject("ChargeBarImage");
        imageGo.transform.SetParent(canvasGo.transform);
        Image img = imageGo.AddComponent<Image>();
        img.type = Image.Type.Filled;
        img.fillMethod = Image.FillMethod.Horizontal;
        img.fillOrigin = (int)Image.OriginHorizontal.Left;

        imageGo.AddComponent<ChargeBarUI>();

        string localPath = "Assets/Prefabs/UI/ChargeBarCanvas.prefab";
        PrefabUtility.SaveAsPrefabAsset(canvasGo, localPath);
        DestroyImmediate(canvasGo);

        Debug.Log("ChargeBarCanvas Prefab created at " + localPath);
    }

    [MenuItem("Custom Tools/Create Ally Capsule With UI")]
    static void CreateAllyCapsule()
    {
        GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        capsule.name = "AllyCapsule";
        CharacterStats stats = capsule.AddComponent<CharacterStats>();

        var chargeBarPrefab = Resources.Load<GameObject>("Prefabs/UI/ChargeBarCanvas");
        if (chargeBarPrefab == null)
        {
            Debug.LogError("ChargeBarCanvas prefab not found in Resources/Prefabs/UI!");
            return;
        }

        GameObject chargeBarCanvas = GameObject.Instantiate(chargeBarPrefab);
        chargeBarCanvas.transform.SetParent(capsule.transform);
        chargeBarCanvas.transform.localPosition = new Vector3(0, 2f, 0);

        ChargeBarUI bar = chargeBarCanvas.GetComponentInChildren<ChargeBarUI>();
        if (bar != null)
            bar.linkedCharacter = stats;

        var floatTextPrefab = Resources.Load<GameObject>("Prefabs/UI/FloatingText");
        if (floatTextPrefab == null)
        {
            Debug.LogError("FloatingText prefab not found in Resources/Prefabs/UI!");
            return;
        }

        GameObject floatingText = GameObject.Instantiate(floatTextPrefab);
        floatingText.transform.SetParent(capsule.transform);
        floatingText.transform.localPosition = new Vector3(0, 2.5f, 0);

        Debug.Log("Ally Capsule created with ChargeBar and FloatingText!");
    }


    [MenuItem("Custom Tools/Create Enemy Sphere With UI")]
    static void CreateEnemySphere()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.name = "EnemySphere";
        CharacterStats stats = sphere.AddComponent<CharacterStats>();

        GameObject chargeBarCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ChargeBarCanvas"));
        chargeBarCanvas.transform.SetParent(sphere.transform);
        chargeBarCanvas.transform.localPosition = new Vector3(0, 1.5f, 0);

        ChargeBarUI bar = chargeBarCanvas.GetComponentInChildren<ChargeBarUI>();
        if (bar != null)
            bar.linkedCharacter = stats;

        GameObject floatingText = Instantiate(Resources.Load<GameObject>("Prefabs/UI/FloatingText"));
        floatingText.transform.SetParent(sphere.transform);
        floatingText.transform.localPosition = new Vector3(0, 2f, 0);

        Debug.Log("Enemy Sphere created with ChargeBar and FloatingText!");
    }
}
#endif
