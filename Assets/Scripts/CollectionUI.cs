using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionUI : MonoBehaviour
{
    public GameObject collectionUI;
    public GameObject specifiedLocationUI;
    public Text collectingProcess;

    private int totalCollectionsNum;
    private int currentNum;
    
	// Use this for initialization
	void Start ()
    {
        currentNum = 0;
        collectionUI.SetActive(true);
        specifiedLocationUI.SetActive(false);
        totalCollectionsNum = GetComponentsInChildren<Collection>(true).Length;
        collectingProcess.text ="收集进度:" + currentNum + "/" + totalCollectionsNum;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void AddOneCollection()
    {
        if (currentNum >= totalCollectionsNum)
        {
            Debug.LogError("收集物数量大于总数,出错!!!!");
            return;
        }
        currentNum++;
        collectingProcess.text = "收集进度:" + currentNum + "/" + totalCollectionsNum;
    }
}
