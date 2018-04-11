using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 此类用于随机生成场景
/// </summary>
public class RandomObstacle : MonoBehaviour
{
    /// <summary>
    /// 障碍物属性类
    /// </summary>
    public class ObstacleAttributes
    {
        /// <summary>
        /// 障碍物名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 障碍物半径
        /// </summary>
        public float Redius;
        /// <summary>
        /// 障碍物预制体路径
        /// </summary>
        public string Path;
        /// <summary>
        /// 障碍物是否在场景中出现
        /// </summary>
        public bool isAppear = false;
        /// <summary>
        /// 此障碍物上是否挂有收集物
        /// </summary>
        public bool IsHaveCollection = false;
        /// <summary>
        /// 此障碍物上是否挂有目标位置
        /// </summary>
        public bool IsHaveTargetPosition = false;
        /// <summary>
        /// 此障碍物属性是否为收集物
        /// </summary>
        public bool IsCollection = false;
    }
    /// <summary>
    /// 随机生成障碍物的区域,读配置表
    /// </summary>
    private Vector2[] randomArea = { new Vector2(-3, -3), new Vector2(3, 3)};
    /// <summary>
    /// 场景中障碍物的总数,读配置表
    /// </summary>
    private int totalNum;
    /// <summary>
    /// 场景中的障碍收集物的数量
    /// </summary>
    private int obstacleCollectionNum;
    private OOFormArray mFormTbObstacle = null;
    private OOFormArray mFormTbSceneConfig = null;
    /// <summary>
    /// 存贮场景中的所有障碍物的动态数组
    /// </summary>
    private List<ObstacleAttributes> obstacleAttributesList = new List<ObstacleAttributes>();
    /// <summary>
    /// 场景中所有的障碍物
    /// </summary>
    private List<GameObject> obstacleGameObjectList = new List<GameObject>();
    /// <summary>
    /// 场景中独立的收集物(不附属于任何障碍物)
    /// </summary>
    private List<GameObject> obstacleCollectionGameObjectList = new List<GameObject>();
    private void Awake()
    {
        #region 加载TbObstacle属性表
        if (mFormTbObstacle == null)
        {
            mFormTbObstacle = OOFormArray.ReadFromResources("Data/Tables/TbObstacle");
        }
        #endregion

        #region 加载TbSceneConfig属性表
        if (mFormTbSceneConfig == null)
        {
            mFormTbSceneConfig = OOFormArray.ReadFromResources("Data/Tables/TbSceneConfig");
        }
        #endregion



        randomArea[0] = new Vector2(float.Parse(mFormTbSceneConfig.GetString("RandomArea", "0").Split('|')[0].Split(',')[0]), float.Parse(mFormTbSceneConfig.GetString("RandomArea", "0").Split('|')[0].Split(',')[1]));
        randomArea[1] = new Vector2(float.Parse(mFormTbSceneConfig.GetString("RandomArea", "0").Split('|')[1].Split(',')[0]), float.Parse(mFormTbSceneConfig.GetString("RandomArea", "0").Split('|')[1].Split(',')[1]));
        totalNum = mFormTbSceneConfig.GetInt("ObstacleNum", "0");
        obstacleCollectionNum = mFormTbSceneConfig.GetInt("ObstacleNum", "0");
        for (int i = 0; i < totalNum; i++)
        {
            var obstacleAttributes = mFormTbObstacle.GetObject<ObstacleAttributes>(Random.Range(1, mFormTbObstacle.mRowCount - 1));
            if (!obstacleAttributes.IsCollection)
            {
                obstacleAttributesList.Add(obstacleAttributes);
            }
            else
            {
                i--;
            }
        }
        for (int i = 0; i < obstacleCollectionNum; i++)
        {
            var obstacleAttributes = new ObstacleAttributes();
            obstacleAttributes.Name = "Collection_Chest_Position";
            obstacleAttributes.isAppear = false;
            obstacleAttributes.IsCollection = true;
            obstacleAttributes.IsHaveCollection = false;
            obstacleAttributes.IsHaveTargetPosition = false;
            obstacleAttributes.Path = mFormTbObstacle.GetString("Path", "Collection_Chest_Position");
            obstacleAttributes.Redius = mFormTbObstacle.GetFloat("Redius", "Collection_Chest_Position");
            obstacleAttributesList.Add(obstacleAttributes);
        }

    }
    // Use this for initialization
    void Start ()
    {

        for (int i = 0; i < obstacleAttributesList.Count; i++)
        {
            //加载一个障碍物资源
            var obstaclePrefab = (GameObject)Resources.Load(obstacleAttributesList[i].Path);
            var obstacle = Instantiate(obstaclePrefab) as GameObject;
            obstacle.transform.parent = gameObject.transform;
            obstacle.transform.localScale = new Vector3(1, 1, 1);
            Random:
            //在指定区域内随机生成一个点
            float randomX = Random.Range(randomArea[0].x, randomArea[1].x);
            float randomY = Random.Range(randomArea[0].y, randomArea[1].y);
            Vector3 randomPosition_0 = new Vector3(randomX, obstacle.transform.position.y, randomY) + transform.position;
            Vector3 randomPosition_1 = new Vector3(randomX, obstacle.transform.position.y + 0.04f, randomY) + transform.position;
            Vector3 randomPosition_2 = new Vector3(randomX, obstacle.transform.position.y + 0.08f, randomY) + transform.position;
            //检测此障碍物放置在当前位置是否会和其他障碍物重合 
            int angel = 0;
            for (int j = 0; j < 12; j++)
            {
                Ray ray_0 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0, Mathf.Sin(angel)));
                RaycastHit hit_0;
                if (Physics.Raycast(ray_0, out hit_0, obstacleAttributesList[i].Redius))
                {
                    // 如果射线与平面碰撞，打印碰撞物体信息  
                    Debug.Log("碰撞对象: " + hit_0.collider.name + "   重新随机一个位置");
                    goto Random;
                }

                Ray ray_1 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0.04f, Mathf.Sin(angel)));
                RaycastHit hit_1;
                if (Physics.Raycast(ray_1, out hit_1, obstacleAttributesList[i].Redius))
                {
                    // 如果射线与平面碰撞，打印碰撞物体信息  
                    Debug.Log("碰撞对象: " + hit_1.collider.name + "   重新随机一个位置");
                    goto Random;
                }

                Ray ray_2 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0.08f, Mathf.Sin(angel)));
                RaycastHit hit_2;
                if (Physics.Raycast(ray_2, out hit_2, obstacleAttributesList[i].Redius ))
                {
                    // 如果射线与平面碰撞，打印碰撞物体信息  
                    Debug.Log("碰撞对象: " + hit_2.collider.name + "   重新随机一个位置");
                    goto Random;
                }

                angel += 30;
            }
            //确定此位置不会和其他障碍物重合,将此位置坐标赋给次障碍物并随机旋转
            if (angel == 360)
            {
                obstacle.transform.position = randomPosition_0;
                obstacle.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
            }
            //将障碍物分类,并将收集物属性的障碍物隐藏
            if (obstacleAttributesList[i].IsCollection)
            {
                obstacleCollectionGameObjectList.Add(obstacle);
            }
            else
            {
                obstacleGameObjectList.Add(obstacle);
            }
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    /// <summary>
    ///  获取场景中的障碍物
    /// </summary>
    /// <returns>返回一个GameObject类型的动态数组</returns>
    public List<GameObject> GetObstacleGameObject()
    {
        if (obstacleGameObjectList.Count == 0)
        {
            Debug.LogError("场景中没有障碍物,获取失败");
            return null;
        }
        else 
        {
            return obstacleGameObjectList;
        }
    }
    /// <summary>
    /// 获取场景中为障碍物的收集物
    /// </summary>
    /// <returns>返回一个GameObject类型的动态数组</returns>
    public List<GameObject> GetObstacleCollectionGameObject()
    {
        if (obstacleCollectionGameObjectList.Count == 0)
        {
            Debug.LogError("场景中没有障碍属性的收集物,获取失败");
            return null;
        }
        else
        {
            return obstacleCollectionGameObjectList;
        }
    }
}
