using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;
using System.Collections;

/*
    Author: Evan Otero
    Date:   Feb 6, 2017
    
    SceneLoader is responsible for asynchonously loading a Scene using any loading mode.  The
    idea is to delagate the loading of the new scene to Unity's dedicated background process.
    We can periodically query the state of the loading and display it.  The scene is loaded in
    Normal mode by defaut, so that the previously loaded scene is closed.  When all assets are loaded,
    the scene is activated and the previous scene is unloaded.

    This custom process should minimize the performance drop on the scene that is currently being
    played and reduce the lag from activating a scene.

    We will need to investigate further:
        1. Is loading the scene in Additive mode necissary if loading is separated from activating?
        2. Do the background processes significantly change game performance, which is critical when
            using VR HMDs?
    
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
    public static SceneLoader instance; // Singleton

    private uint controllerIndex;
    private RightControllerAppearence rightControllerAppearence;

    private void Start() 
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    private void Update()
    {
        GameObject rightHand = VRTK_DeviceFinder.GetControllerRightHand(true);
        controllerIndex = VRTK_DeviceFinder.GetControllerIndex(rightHand);
        rightControllerAppearence = VRTK_DeviceFinder.GetScriptAliasController(rightHand).GetComponent<RightControllerAppearence>();
    }

    // Invoked with StartCoroutine
    public IEnumerator AsyncLoadScene(string sceneName, bool activateOnReady = false, LoadSceneMode mode = LoadSceneMode.Single, bool unloadCurrent = false)
    {
        string prevScene =  SceneManager.GetActiveScene().name;

        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName, mode);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");

            // Loading complete
            if (Mathf.Approximately(ao.progress, 0.9f))
            {
                // Prompt user to pull trigger
                rightControllerAppearence.toggleTriggerTooltips(true);

                // Pull vive trigger to start scene
                if (VRTK_SDK_Bridge.IsTriggerPressedDownOnIndex(controllerIndex) || activateOnReady)
                {
                    Debug.Log("Loading done.");
                    ao.allowSceneActivation = true;
                    Debug.Log("Scene activated");
                }
            }

            yield return null;
        }

        if (prevScene != "" && unloadCurrent)
            StartCoroutine(AsyncUnloadScene(prevScene));
    }

    // Invoked with StartCoroutine
    public IEnumerator AsyncUnloadScene(string sceneName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(sceneName);

        while (!ao.isDone)
        {
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            Debug.Log("Unloading progress: " + (progress * 100) + "%");

            // Unloading complete
            if (Mathf.Approximately(ao.progress, 0.9f))
                Debug.Log("Unloading done.");

            yield return null;
        }
    }
}
