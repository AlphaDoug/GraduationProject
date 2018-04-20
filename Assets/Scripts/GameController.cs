using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject FPSController;
    /// <summary>
    /// 下一个场景的index
    /// </summary>
    [SerializeField]
    private int nextSceneIndex;
    private int currentIndex;
    private OOFormArray mFormChestNum = null;
    private OOFormArray mFormTbCollection = null;
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
    }
    // Use this for initialization
    void Start ()
    {
        FPSController.GetComponent<FirstPersonController>().portals = GameObject.FindObjectsOfType<Portal>();
        Collection.CollectOneThingEvent += AddOneChest;
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
    }

    private void OnApplicationQuit()
    {
        mFormChestNum.SaveFormFile("Assets/Resources/Data/DataStored/ChestNum.txt");
    }
}
