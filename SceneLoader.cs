using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    private static SceneManager instance;

    void Start () 
    {
        if (instance != null)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    // Invoked with StartCoroutine(AsyncLoadScene(“<sceneName>”))
    IEnumerator AsyncLoadScene (string sceneName)
    {
        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            // [0, 0.9] > [0, 1]
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            Debug.log("Loading progress: " + (progress * 100) + "%");

            // Loading complete
            if (Mathf.Approximately(ao.progress, 0.9f))
            {
                // Prompt user for
                // Pull vive trigger to start scene
                if (VRTK.triggerClicked())
                {
                    Debug.log("Loading done.");
                    ao.allowSceneActivation = true;
                    Scene nextScene = SceneManager.GetSceneByName(sceneName);
                    Scene prevScene = SceneManager.GetActiveScene().name;
                    SceneManager.SetActiveScene(nextScene);
                    Debug.log("Scene activated");
                    // UpdatePlayerPosition(nextScene);
                    StartCoroutine(AsyncUnloadScene(prevScene));
                }
            }

            yield return null;
        }
    }

    // Invoked with StartCoroutine(AsyncUnloadScene(“<sceneName>”))
    IEnumerator AsyncUnloadScene (string sceneName)
    {
        yield return null;

        AsyncOperation ao = SceneManager.UnloadSceneAsync(sceneName);

        while (!ao.isDone)
        {
            // [0, 0.9] > [0, 1]
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            Debug.log("Unloading progress: " + (progress * 100) + "%");

            // Unloading complete
            if (Mathf.Approximately(ao.progress, 0.9f))
                Debug.log("Unloading done.");

            yield return null;
        }
    }

    void UpdatePlayerPosition (Scene scene)
    {
        GameObject player = VRTK.VRSimulatorCameraRig.FindInScene();
        // Update position if need be
        Debug.log("Player position updated");
    }
}