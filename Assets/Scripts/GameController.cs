using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[] maskObjs;
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
        }
        else
        {
            Debug.Log("当前还有任务组,显示下一个任务.");
            maskObjs[currentIndex].SetActive(true);
        }
    }
}
