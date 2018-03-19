using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginSceneToPlayingScene : MonoBehaviour {

    private void Awake()
    {
        StartCoroutine(ThisSceneToNextScene());
    }

    IEnumerator ThisSceneToNextScene()
    {
        yield return new WaitForSeconds(17f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
