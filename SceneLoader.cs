using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;

/*
    Author: Evan Otero
    Date:   Feb 6, 2016
    
    SceneLoader is responsible for asynchonously loading a Scene using Additive mode.  The
    idea is to delagate the loading of the new scene to Unity's dedicated background process.
    We can periodically query the state of the loading and display it.  The scene is loaded in
    Additive mode so that the currently loaded scene is not closed.  When all assets are loaded,
    scene is activated and the previous scene is unloaded.

    This custom process should minimize the performance drop on the scene that is currently being
    played and reduce the lag from activating a scene.

    We will need to investigate further:
        1. Is loading the scene in Additive mode necissary if loading is separated from activating?
        2. Do the background processes significantly change game performance, which is critical when
            using VR HMDs?
        3. Are scenes properly unloaded after the loading process completes?
        4. Does the Camera Rig position need to updated manually, or will change to the proper position
            in the new scene?  Is that behavior changed by Additive and Single mode?
    
    ***********************
    ******* LICENSE *******
    ***********************
    JoyceStick is a Boston College digital humanities project employing Unity
    to construct a virtual reality game from Joyce’s Ulysses for viewing on the
    HTC Vive, supported by a Teaching and Mentoring Grant and substantial funding
    from internal bodies at Boston College.
    Copyright (C) 2017  Evan Otero, Drew Hoo, Emaad Ali, Will Bowditch, Matt Harty, Jake Schafer, & Ryan Reede
    http://joycestick.bc.edu/

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

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
        // yield return null;

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
                // TODO: Prompt user
                // Pull vive trigger to start scene
                if (VRTK.triggerClicked())
                {
                    Debug.log("Loading done.");
                    Scene prevScene = SceneManager.GetActiveScene().name;
                    ao.allowSceneActivation = true;
                    Debug.log("Scene activated");
                }
            }

            yield return null;
        }

        // UpdatePlayerPosition(nextScene);
        StartCoroutine(AsyncUnloadScene(prevScene));
    }

    // Invoked with StartCoroutine(AsyncUnloadScene(“<sceneName>”))
    IEnumerator AsyncUnloadScene (string sceneName)
    {
        // yield return null;
        
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
