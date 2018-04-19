using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject FPSController;
    public GameObject portalGroup;
    public GameObject[] maskObjs;
    /// <summary>
    /// 下一个场景的index
    /// </summary>
    [SerializeField]
    private int nextSceneIndex;
    private int currentIndex;
    private OOFormArray mForm = null;
    /// <summary>
    /// 传送门属性类
    /// </summary>
    public class PortalAttributes
    {
        public int PortalGroup;
        public string Position
        {
            set
            {
                position = new Vector3(float.Parse(value.Split(',')[0]), float.Parse(value.Split(',')[1]), float.Parse(value.Split(',')[2]));
            }
        }
        public string Rotation
        {
            set
            {
                rotation = new Vector3(float.Parse(value.Split(',')[0]), float.Parse(value.Split(',')[1]), float.Parse(value.Split(',')[2]));
            }
        }
        public string OutLocalPosition
        {
            set
            {
                outLocalPosition = new Vector3(float.Parse(value.Split(',')[0]), float.Parse(value.Split(',')[1]), float.Parse(value.Split(',')[2]));
            }
        }
        public string Scale
        {
            set
            {
                scale = new Vector3(float.Parse(value.Split(',')[0]), float.Parse(value.Split(',')[1]), float.Parse(value.Split(',')[2]));
            }
        }


        public Vector3 position;
        public Vector3 rotation;
        public Vector3 outLocalPosition;
        public Vector3 scale;
    }

    // Use this for initialization
    void Start ()
    {
        #region 加载TbPortals属性表
        if (mForm == null)
        {
            mForm = OOFormArray.ReadFromResources("Data/Tables/TbPortals");
        }
        #endregion
        for (int i = 1; i < mForm.mRowCount; i += 2)
        {
            //生成传送门组中的两个传送门
            PortalAttributes portalAttributes_1 = mForm.GetObject<PortalAttributes>(i);
            PortalAttributes portalAttributes_2 = mForm.GetObject<PortalAttributes>(i + 1);
            CreatePortal(portalAttributes_1, portalAttributes_2);
        }
        currentIndex = 0;
        maskObjs[currentIndex].SetActive(true);
        FPSController.GetComponent<FirstPersonController>().portals = GameObject.FindObjectsOfType<Portal>();
    }

    private void CreatePortal(PortalAttributes p1,PortalAttributes p2)
    {
        var portalPrefab = (GameObject)Resources.Load("Prefab/Portal");
        var portal = Instantiate(portalPrefab) as GameObject;
        portal.name = "Portal";
        portal.transform.parent = portalGroup.transform;
        foreach (Transform child in portal.transform)
        {
            if (child.gameObject.name == "Portal_1")
            {
                child.gameObject.GetComponent<Portal>().cameraImage = GameObject.FindGameObjectWithTag("CameraImage").GetComponent<Image>();
                child.localPosition = p1.position;
                child.localEulerAngles = p1.rotation;
                child.gameObject.GetComponent<Portal>().outPositionVector3 = p1.outLocalPosition;
                child.localScale = p1.scale;
            }
            if (child.gameObject.name == "Portal_2")
            {
                child.gameObject.GetComponent<Portal>().cameraImage = GameObject.FindGameObjectWithTag("CameraImage").GetComponent<Image>();
                child.localPosition = p2.position;
                child.localEulerAngles = p2.rotation;
                child.gameObject.GetComponent<Portal>().outPositionVector3 = p2.outLocalPosition;
                child.localScale = p2.scale;
            }
        }
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
