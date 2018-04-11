using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskController : MonoBehaviour
{
    /// <summary>
    /// 存贮场景中所有可能会出现收集物的位置
    /// </summary>
    private GameObject[] obstacleCollectionGameObject;

    // Use this for initialization
    void Start ()
    {
        obstacleCollectionGameObject = GameObject.FindGameObjectsWithTag("Collection");

        //初始化首个收集物
        InitializeOneCollection();

        Collection.CollectOneThingEvent += CollectionEvent;
    }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {
        Collection.CollectOneThingEvent -= CollectionEvent;
    }

    /// <summary>
    /// 一个收集物被收集后触发
    /// </summary>
    private void CollectionEvent()
    {
        Debug.Log("收集一个");
        InitializeOneCollection();
    }
    /// <summary>
    /// 初始化一个收集物并随机放置在场景中的一个位置上
    /// </summary>
    private void InitializeOneCollection()
    {
        var collectionPrefab = (GameObject)Resources.Load("Prefab/Scene/Collection_Chest");
        var collection = Instantiate(collectionPrefab) as GameObject;
        collection.transform.parent = obstacleCollectionGameObject[Random.Range(0, obstacleCollectionGameObject.Length)].transform;
        collection.transform.localPosition = Vector3.zero;
    }
}
