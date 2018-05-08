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
    /// <summary>
    /// 下一个场景的index
    /// </summary>
    [SerializeField]
    private int nextSceneIndex;
    [SerializeField]
    private Text chest1Num;
    [SerializeField]
    private Text chest2Num;
    [SerializeField]
    private Text chest3Num;
    [SerializeField]
    private Image showReward;
    private int currentIndex;
    private OOFormArray mFormChestNum = null;
    private OOFormArray mFormTbCollection = null;
    private OOFormArray mFormTbReward = null;
    private List<RewardAttribute> rewardAttributeList = new List<RewardAttribute>();
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
        #endregion

        #region 读取TbReward表
        if (mFormTbReward == null)
        {
            mFormTbReward = OOFormArray.ReadFromResources("Data/Tables/TbRewards");
        }
        #endregion

        for (int i = 1; i < mFormTbReward.mRowCount; i++)
        {
            var buff = mFormTbReward.GetObject<RewardAttribute>(i);
            rewardAttributeList.Add(buff);
        }

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
        RefreshNum();

    }
    // Use this for initialization
    void Start ()
    {
        FPSController.GetComponent<FirstPersonController>().portals = GameObject.FindObjectsOfType<Portal>();
        Collection.CollectOneThingEvent += AddOneChest;
        ActivateChest.OpenOneChestEvent += OpenChest;
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
        mFormChestNum.SaveFormFile("Assets/Resources/Data/DataStored/ChestNum.txt");
    }
    /// <summary>
    /// 刷新UI显示的箱子数量
    /// </summary>
    private void RefreshNum()
    {
        chest1Num.text = mFormChestNum.GetString("DES", "0") + ": " + mFormChestNum.GetString("Num", "0") + "个";
        chest2Num.text = mFormChestNum.GetString("DES", "1") + ": " + mFormChestNum.GetString("Num", "1") + "个";
        chest3Num.text = mFormChestNum.GetString("DES", "2") + ": " + mFormChestNum.GetString("Num", "2") + "个";
    }

    private void OpenChest(int id,string des)
    {
        Debug.Log("箱子ID" + id + "名称" + des + "被打开");
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
                showReward.sprite = Resources.Load(rewardAttributeList[i].Path, typeof(Sprite)) as Sprite;
                break;
            }
        }

    }
}
