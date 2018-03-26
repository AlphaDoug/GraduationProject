using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[] maskObjs;
    /// <summary>
    /// 下一个场景的index
    /// </summary>
    [SerializeField]
    private int nextSceneIndex;
    private int currentIndex;
	// Use this for initialization
	void Start ()
    {
        currentIndex = 0;
        maskObjs[currentIndex].SetActive(true);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetNextGroupTrue()
    {
        maskObjs[currentIndex].SetActive(false);
        currentIndex++;
        if (currentIndex > maskObjs.Length - 1)
        {
            Debug.Log("当前没有任务组,此关卡应该结束,切换下一个场景.");
            LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("当前还有任务组,显示下一个任务.");
            maskObjs[currentIndex].SetActive(true);
        }
    }

    public void LoadScene(int index)
    {
        var loadingPrefab = (GameObject)Resources.Load("Prefab/LoadingScene");
        var loading = Instantiate(loadingPrefab) as GameObject;
        loading.GetComponent<LoadingController>().loadingSceneIndex = index;
        loading.transform.parent = GameObject.Find("Canvas").transform;
        loading.GetComponent<RectTransform>().localPosition = Vector2.zero;
        loading.GetComponent<RectTransform>().localScale = Vector2.one;
        loading.GetComponent<RectTransform>().localRotation = new Quaternion();
    }
}
