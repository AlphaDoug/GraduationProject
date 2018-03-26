using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    public GameObject imageLoading;
    public int loadingSceneIndex;

    private AsyncOperation async;

    // Use this for initialization
    void Start ()
    {


        StartCoroutine(loadScene());
    }
	
	// Update is called once per frame
	void Update ()
    {
        imageLoading.transform.RotateAround(imageLoading.transform.position, imageLoading.transform.forward, -4f);
    }

    IEnumerator loadScene()
    {
        yield return new WaitForSeconds(3.0f);

        //异步读取场景。
        async = SceneManager.LoadSceneAsync(loadingSceneIndex);

        //读取完毕后返回， 系统会自动进入场景
        yield return async;
    }
}
