using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskController : MonoBehaviour
{
    /// <summary>
    /// 目标位置属性类
    /// </summary>
    public class TargetAttribute
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int ID;
        /// <summary>
        /// 描述(提示)
        /// </summary>
        public string DES;

    }
    /// <summary>
    /// 收集物属性类
    /// </summary>
    public class CollectionAttribute
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int ID;
        /// <summary>
        /// 描述(提示)
        /// </summary>
        public string DES;
        /// <summary>
        /// 预制体路径
        /// </summary>
        public string Path;
    }

    [TooltipAttribute("任务UI的标题")]
    [SerializeField]
    private GameObject taskUITitle;

    [TooltipAttribute("任务UI的内容")]
    [SerializeField]
    private GameObject taskUIContent;

    /// <summary>
    /// 存贮场景中所有可能会出现收集物的位置
    /// </summary>
    private GameObject[] obstacleCollectionGameObject;
    /// <summary>
    /// 存贮场景中所有可能出现前往目标位置的物体
    /// </summary>
    private GameObject[] targetGameobject;

    private OOFormArray mFormTarget = null;
    private OOFormArray mFormCollection = null;

    private List<TargetAttribute> targetAttribute = new List<TargetAttribute>();
    private List<CollectionAttribute> collectionAttribute = new List<CollectionAttribute>();

    private void Awake()
    {
        #region 加载TbTarget属性表
        if (mFormTarget == null)
        {
            mFormTarget = OOFormArray.ReadFromResources("Data/Tables/TbTarget");
        }
        #endregion

        #region 加载TbCollection属性表
        if (mFormCollection == null)
        {
            mFormCollection = OOFormArray.ReadFromResources("Data/Tables/TbCollection");
        }
        #endregion

        for (int i = 1; i < mFormTarget.mRowCount; i++)
        {
            var m_targetAttribute = mFormTarget.GetObject<TargetAttribute>(i);
            targetAttribute.Add(m_targetAttribute);
        }

        for (int i = 1; i < mFormCollection.mRowCount; i++)
        {
            var m_collectionAttribute = mFormCollection.GetObject<CollectionAttribute>(i);
            collectionAttribute.Add(m_collectionAttribute);
        }
    }
    // Use this for initialization
    void Start ()
    {
        obstacleCollectionGameObject = GameObject.FindGameObjectsWithTag("Collection");
        targetGameobject = GameObject.FindGameObjectsWithTag("Target");
        for (int i = 0; i < targetGameobject.Length; i++)
        {
            targetGameobject[i].SetActive(false);
        }
        //初始化首个任务
        CollectionEvent(-1,"");
        //绑定收集事件
        Collection.CollectOneThingEvent += CollectionEvent;
        Target.ReachOnePositionEvent += CollectionEvent;
    }

    private void OnDisable()
    {
        //解绑收集事件
        Collection.CollectOneThingEvent -= CollectionEvent;
        Target.ReachOnePositionEvent -= CollectionEvent;
    }

    /// <summary>
    /// 一个收集物被收集后触发
    /// </summary>
    private void CollectionEvent(int id ,string des)
    {
        Debug.Log("收集一个");
        switch (Random.Range(0,1))
        {
            case 0:
                //收集
                InitializeOneCollection();
                break;
            case 1:
                //前往指定位置
                InitializeOneTarget();
                break;
            default:
                break;
        }

    }
    /// <summary>
    /// 初始化一个收集物并随机放置在场景中的一个位置上
    /// </summary>
    private void InitializeOneCollection()
    {
        //随机选取一个收集物属性
        var randomCollectionAttribute = collectionAttribute[Random.Range(0, collectionAttribute.Count)];

        var collectionPrefab = (GameObject)Resources.Load(randomCollectionAttribute.Path);
        var collection = Instantiate(collectionPrefab) as GameObject;
        collection.GetComponent<Collection>().ID = randomCollectionAttribute.ID;
        collection.GetComponent<Collection>().DES = randomCollectionAttribute.DES;
        collection.transform.parent = obstacleCollectionGameObject[Random.Range(0, obstacleCollectionGameObject.Length)].transform;
        collection.transform.localPosition = Vector3.zero;
        taskUITitle.GetComponent<Text>().text = "任务:收集宝箱";
        taskUIContent.GetComponent<Text>().text = "提示:" + randomCollectionAttribute.DES;
    }
    /// <summary>
    /// 在场景中的所有目标位置中随机激活一个位置
    /// </summary>
    private void InitializeOneTarget()
    {
        var obj = targetGameobject[Random.Range(0, targetGameobject.Length)];
        obj.SetActive(true);
        taskUITitle.GetComponent<Text>().text = "任务:前往指定位置";
        var ID = obj.GetComponent<Target>().ID;

        taskUIContent.GetComponent<Text>().text = "提示:" + mFormTarget.GetString("DES", ID.ToString());
    }


}
