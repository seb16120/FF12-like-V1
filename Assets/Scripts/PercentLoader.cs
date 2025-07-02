using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PercentLoader : MonoBehaviour
{
    public void LoadPercent(int sceneIndex)
    {
        // Start the asynchronous loading process
        StartCoroutine(LoadSceneAsync(sceneIndex));

    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // Normalize progress to 0-1 range
            // Update the loading progress here
            Debug.Log("Loading progress: " + progress);
            yield return null;
        }
        // Allow the scene to activate once loading is complete
        operation.allowSceneActivation = true;

    }
}
