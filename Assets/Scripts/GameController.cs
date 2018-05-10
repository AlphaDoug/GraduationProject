using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public class RewardAttribute
    {
        public int ID;
        public string DES;
        public int Weight;
        public string Path;
    }
    public GameObject FPSController;

    [TooltipAttribute("下一个场景的index")]
    [SerializeField]
    private int nextSceneIndex;

    [TooltipAttribute("宝箱1数量的UI")]
    [SerializeField]
    private Text chest1Num;

    [TooltipAttribute("宝箱2数量的UI")]
    [SerializeField]
    private Text chest2Num;

    [TooltipAttribute("宝箱3数量的UI")]
    [SerializeField]
    private Text chest3Num;

    [TooltipAttribute("奖励动画图片")]
    [SerializeField]
    private Image showReward;

    [TooltipAttribute("要开启的宝箱的位置")]
    [SerializeField]
    private GameObject[] chestPosition;

    [TooltipAttribute("开启的宝箱上面的数量显示")]
    [SerializeField]
    private GameObject[] chestNum3DText;
    /// <summary>
    /// 是否正在播放奖励动画
    /// </summary>
    [SerializeField]
    private bool isShowReward;

    private Animator imageAnimator;
    private int currentIndex;
    private OOFormArray mFormChestNum = null;
    private OOFormArray mFormTbCollection = null;
    private OOFormArray mFormTbReward = null;
    private OOFormArray mFormRewardNum = null;
    private List<RewardAttribute> rewardAttributeList = new List<RewardAttribute>();
    private List<TaskController.CollectionAttribute> collectionAttribute = new List<TaskController.CollectionAttribute>();

    private void Awake()
    {
        #region 读取存贮的数据
        if (mFormChestNum == null)
        {
            mFormChestNum = OOFormArray.ReadFromResources("Data/DataStored/ChestNum");
        }
        #endregion

        #region 读取TbCollection表
        if (mFormTbCollection == null)
        {
            mFormTbCollection = OOFormArray.ReadFromResources("Data/Tables/TbCollection");
        }
        for (int i = 1; i < mFormTbCollection.mRowCount; i++)
        {
            var m_collectionAttribute = mFormTbCollection.GetObject<TaskController.CollectionAttribute>(i);
            collectionAttribute.Add(m_collectionAttribute);
        }
        #endregion

        #region 读取TbReward表
        if (mFormTbReward == null)
        {
            mFormTbReward = OOFormArray.ReadFromResources("Data/Tables/TbRewards");
        }
        for (int i = 1; i < mFormTbReward.mRowCount; i++)
        {
            var buff = mFormTbReward.GetObject<RewardAttribute>(i);
            rewardAttributeList.Add(buff);
        }
        #endregion

        #region 读取奖励存贮表
        if (mFormRewardNum == null)
        {
            mFormRewardNum = OOFormArray.ReadFromResources("Data/DataStored/RewardNum");
        }
        #endregion

        #region 宝箱存贮表操作
        if (mFormChestNum.mRowCount != mFormTbCollection.mRowCount)
        {
            //存贮的宝箱数量的表格行数和宝箱配置表行数不相同,代表宝箱种类有变化,需要重新生成存贮表
            mFormChestNum = new OOFormArray();
            mFormChestNum.InsertRow(0);
            mFormChestNum.InsertColumn(0);
            mFormChestNum.InsertColumn(0);
            mFormChestNum.InsertColumn(0);
            mFormChestNum.SetString("ID", 0, 0);
            mFormChestNum.SetString("DES", 1, 0);
            mFormChestNum.SetString("Num", 2, 0);
            for (int i = 1; i < mFormTbCollection.mRowCount; i++)
            {
                mFormChestNum.InsertRow(i);
                mFormChestNum.SetInt(i - 1, 0, i);
                mFormChestNum.SetString(mFormTbCollection.GetString("DES", i.ToString()), 1, i);
                mFormChestNum.SetInt(0, 2, i);
            }        
        }
        #endregion

        #region 卡片存贮表操作
        //两个表格行数不等,需要更新奖励存贮表
        if (mFormTbReward.mRowCount != mFormRewardNum.mRowCount)
        {
            mFormRewardNum = new OOFormArray();
            mFormRewardNum.InsertRow(0);
            mFormRewardNum.InsertColumn(0);
            mFormRewardNum.InsertColumn(0);
            mFormRewardNum.InsertColumn(0);
            mFormRewardNum.SetString("ID", 0, 0);
            mFormRewardNum.SetString("DES", 1, 0);
            mFormRewardNum.SetString("Num", 2, 0);
            for (int i = 1; i < mFormTbReward.mRowCount; i++)
            {
                mFormRewardNum.InsertRow(i);
                //写入ID
                mFormRewardNum.SetInt(i - 1, 0, i);
                //写入DES
                mFormRewardNum.SetString(mFormTbReward.GetString("DES", (i - 1).ToString()), 1, i);
                //写入数量
                mFormRewardNum.SetInt(0, 2, i);
            }
        }
        #endregion

    }
    // Use this for initialization
    void Start ()
    {
        imageAnimator = showReward.GetComponent<Animator>();

        #region 初始化要开启的宝箱
        for (int i = 0; i < collectionAttribute.Count; i++)
        {
            var collectionPrefab = (GameObject)Resources.Load(collectionAttribute[i].Path);
            var collection = Instantiate(collectionPrefab) as GameObject;
            collection.transform.parent = chestPosition[i].transform;
            collection.transform.localPosition = Vector3.zero;
            collection.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            collection.GetComponent<ActivateChest>().enabled = true;
            collection.GetComponent<Collection>().ID = (collectionAttribute[i].ID);
            collection.GetComponent<Collection>().DES = (collectionAttribute[i].DES);
        }
        #endregion

        FPSController.GetComponent<FirstPersonController>().portals = GameObject.FindObjectsOfType<Portal>();
        Collection.CollectOneThingEvent += AddOneChest;
        ActivateChest.OpenOneChestEvent += OpenChest;

        //刷新UI显示
        RefreshNum();

        //刷新3D文字显示
        Refresh3DText();
    }
    private void FixedUpdate()
    {
        if (imageAnimator.GetCurrentAnimatorStateInfo(0).IsName("Reward"))
        {
            isShowReward = true;
        }
        else
        {
            isShowReward = false;
        }
    }

    private void OnDisable()
    {
        Collection.CollectOneThingEvent -= AddOneChest;
        ActivateChest.OpenOneChestEvent -= OpenChest;
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
    /// <summary>
    /// 增加一个箱子
    /// </summary>
    /// <param name="id">箱子ID</param>
    /// <param name="des">箱子的描述</param>
    public void AddOneChest(int id, string des)
    {
        id--;
        var lastNum = mFormChestNum.GetInt("Num", id.ToString(), des);
        mFormChestNum.SetInt(lastNum + 1, "Num", id.ToString(), des);
        RefreshNum();
    }

    private void OnApplicationQuit()
    {
        mFormRewardNum.SaveFormFile("Assets/Resources/Data/DataStored/RewardNum.txt");
        mFormChestNum.SaveFormFile("Assets/Resources/Data/DataStored/ChestNum.txt");
    }
    /// <summary>
    /// 刷新UI显示的箱子数量和存贮表
    /// </summary>
    private void RefreshNum()
    {
        chest1Num.text = mFormChestNum.GetString("DES", "0") + ": " + mFormChestNum.GetString("Num", "0") + "个";
        chest2Num.text = mFormChestNum.GetString("DES", "1") + ": " + mFormChestNum.GetString("Num", "1") + "个";
        chest3Num.text = mFormChestNum.GetString("DES", "2") + ": " + mFormChestNum.GetString("Num", "2") + "个";
        //mFormChestNum.SaveFormFile("Assets/Resources/Data/DataStored/ChestNum.txt");
    }
    /// <summary>
    /// 刷新3DText
    /// </summary>
    private void Refresh3DText()
    {
        for (int i = 1; i < mFormChestNum.mRowCount; i++)
        {
            chestNum3DText[i - 1].GetComponent<TextMesh>().text = "剩余数量:" + mFormChestNum.GetString("Num", i);
        }
    }

    private void OpenChest(int id,string des)
    {
        //检测是否有剩余的箱子可开
        if (mFormChestNum.GetInt("Num",id) <= 0)
        {
            Debug.Log("没有剩余箱子");
            return;
        }
        Debug.Log("箱子ID" + id + "名称" + des + "被打开");
        isShowReward = true;
        //开箱子给予的东西
        int totalWeight = 0;
        for (int i = 0; i < rewardAttributeList.Count; i++)
        {
            totalWeight += rewardAttributeList[i].Weight;
        }
        int randomNum = Random.Range(0, totalWeight);
        int comparisonNum = 0;
        for (int i = 0; i < rewardAttributeList.Count; i++)
        {
            comparisonNum += rewardAttributeList[i].Weight;
            if (comparisonNum > randomNum)
            {
                Debug.Log("奖励名称为" + rewardAttributeList[i].DES);

                showReward.gameObject.SetActive(true);
                //播放奖励动画
                imageAnimator.Play("Reward");
                showReward.sprite = Resources.Load(rewardAttributeList[i].Path, typeof(Sprite)) as Sprite;
                //更新奖励存贮表
                int currentNum = mFormRewardNum.GetInt("Num",  i + 1);
                mFormRewardNum.SetInt(currentNum + 1, 2, i + 1);

                //更新宝箱存贮表
                int currentChestNum = mFormChestNum.GetInt("Num", id);
                mFormChestNum.SetInt(currentChestNum - 1, "Num", id);

                //刷新宝箱显示UI
                RefreshNum();
                Refresh3DText();
                break;
            }
        }

    }
}
