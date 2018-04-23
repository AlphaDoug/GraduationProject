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
        /// <summary>
        /// 此障碍物所属区域
        /// </summary>
        public int Area;
        /// <summary>
        /// 此障碍物上目标位置的ID
        /// </summary>
        public int TargetID;
        /// <summary>
        /// 此障碍物重新生成的次数限制
        /// </summary>
        public int RandomTimes;
    }
    /// <summary>
    /// 随机生成障碍物的区域,读配置表
    /// </summary>
    private Vector2[] randomArea0 = { new Vector2(0, 0), new Vector2(0, 0) };
    private Vector2[] randomArea1 = { new Vector2(0, 0), new Vector2(0, 0) };
    private Vector2[] randomArea2 = { new Vector2(0, 0), new Vector2(0, 0) };
    private Vector2[] randomArea3 = { new Vector2(0, 0), new Vector2(0, 0) };
    private Vector2[] randomArea4 = { new Vector2(0, 0), new Vector2(0, 0) };
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
    /// <summary>
    /// 四个区域
    /// </summary>
    private GameObject area1, area2, area3, area4;

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

        #region 读取场景中的区域
        randomArea0[0] = new Vector2(float.Parse(mFormTbSceneConfig.GetString("RandomArea0", "0").Split('|')[0].Split(',')[0]), float.Parse(mFormTbSceneConfig.GetString("RandomArea0", "0").Split('|')[0].Split(',')[1]));
        randomArea0[1] = new Vector2(float.Parse(mFormTbSceneConfig.GetString("RandomArea0", "0").Split('|')[1].Split(',')[0]), float.Parse(mFormTbSceneConfig.GetString("RandomArea0", "0").Split('|')[1].Split(',')[1]));

        randomArea1[0] = new Vector2(float.Parse(mFormTbSceneConfig.GetString("RandomArea1", "0").Split('|')[0].Split(',')[0]), float.Parse(mFormTbSceneConfig.GetString("RandomArea1", "0").Split('|')[0].Split(',')[1]));
        randomArea1[1] = new Vector2(float.Parse(mFormTbSceneConfig.GetString("RandomArea1", "0").Split('|')[1].Split(',')[0]), float.Parse(mFormTbSceneConfig.GetString("RandomArea1", "0").Split('|')[1].Split(',')[1]));

        randomArea2[0] = new Vector2(float.Parse(mFormTbSceneConfig.GetString("RandomArea2", "0").Split('|')[0].Split(',')[0]), float.Parse(mFormTbSceneConfig.GetString("RandomArea2", "0").Split('|')[0].Split(',')[1]));
        randomArea2[1] = new Vector2(float.Parse(mFormTbSceneConfig.GetString("RandomArea2", "0").Split('|')[1].Split(',')[0]), float.Parse(mFormTbSceneConfig.GetString("RandomArea2", "0").Split('|')[1].Split(',')[1]));

        randomArea3[0] = new Vector2(float.Parse(mFormTbSceneConfig.GetString("RandomArea3", "0").Split('|')[0].Split(',')[0]), float.Parse(mFormTbSceneConfig.GetString("RandomArea3", "0").Split('|')[0].Split(',')[1]));
        randomArea3[1] = new Vector2(float.Parse(mFormTbSceneConfig.GetString("RandomArea3", "0").Split('|')[1].Split(',')[0]), float.Parse(mFormTbSceneConfig.GetString("RandomArea3", "0").Split('|')[1].Split(',')[1]));

        randomArea4[0] = new Vector2(float.Parse(mFormTbSceneConfig.GetString("RandomArea4", "0").Split('|')[0].Split(',')[0]), float.Parse(mFormTbSceneConfig.GetString("RandomArea4", "0").Split('|')[0].Split(',')[1]));
        randomArea4[1] = new Vector2(float.Parse(mFormTbSceneConfig.GetString("RandomArea4", "0").Split('|')[1].Split(',')[0]), float.Parse(mFormTbSceneConfig.GetString("RandomArea4", "0").Split('|')[1].Split(',')[1]));
        #endregion

        totalNum = mFormTbSceneConfig.GetInt("ObstacleNum", "0");
        obstacleCollectionNum = mFormTbSceneConfig.GetInt("ObstacleNum", "0");
        area1 = GameObject.FindGameObjectWithTag("Area1");
        area2 = GameObject.FindGameObjectWithTag("Area2");
        area3 = GameObject.FindGameObjectWithTag("Area3");
        area4 = GameObject.FindGameObjectWithTag("Area4");
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
            obstacleAttributes.RandomTimes = mFormTbObstacle.GetInt("RandomTimes", "Collection_Chest_Position");
            obstacleAttributesList.Add(obstacleAttributes);
        }

    }
    // Use this for initialization
    void Start()
    {
        float randomX, randomY;
        Vector3 randomPosition_0, randomPosition_1, randomPosition_2;
        int angel;
        for (int i = 0; i < obstacleAttributesList.Count; i++)
        {
            if (obstacleAttributesList[i].IsCollection)
            {
                continue;
            }
            //加载一个障碍物资源
            var obstaclePrefab = (GameObject)Resources.Load(obstacleAttributesList[i].Path);
            var obstacle = Instantiate(obstaclePrefab) as GameObject;
            obstacle.transform.parent = gameObject.transform;
            obstacle.transform.localScale = new Vector3(1, 1, 1);
            var randomTimes = obstacleAttributesList[i].RandomTimes;
            int currentTimes = 0;
            //根据区域编号分类障碍物并进行分区随机生成

            switch (obstacleAttributesList[i].Area)
            {
                //在区域1中
                case 1:
                    Random1:
                    currentTimes++;
                    //若循环次数大于设定值,则此障碍物不会生成
                    if (currentTimes > randomTimes)
                    {
                        Destroy(obstacle);
                        Debug.LogError("障碍物:" + obstacle.gameObject.name + "未生成");
                        continue;
                    }
                    //在指定区域内随机生成一个点
                    randomX = Random.Range(randomArea1[0].x, randomArea1[1].x);
                    randomY = Random.Range(randomArea1[0].y, randomArea1[1].y);
                    randomPosition_0 = new Vector3(randomX, obstacle.transform.position.y, randomY) + area1.transform.position;
                    randomPosition_1 = new Vector3(randomX, obstacle.transform.position.y + 0.04f, randomY) + area1.transform.position;
                    randomPosition_2 = new Vector3(randomX, obstacle.transform.position.y + 0.08f, randomY) + area1.transform.position;
                    //检测此障碍物放置在当前位置是否会和其他障碍物重合 
                    angel = 0;
                    for (int j = 0; j < 12; j++)
                    {
                        Ray ray_0 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0, Mathf.Sin(angel)));
                        RaycastHit hit_0;
                        if (Physics.Raycast(ray_0, out hit_0, obstacleAttributesList[i].Redius))
                        {
                            // 如果射线与平面碰撞，打印碰撞物体信息  
                           // Debug.Log("碰撞对象: " + hit_0.collider.name + "   重新随机一个位置");
                            goto Random1;
                        }

                        Ray ray_1 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0.04f, Mathf.Sin(angel)));
                        RaycastHit hit_1;
                        if (Physics.Raycast(ray_1, out hit_1, obstacleAttributesList[i].Redius))
                        {
                            // 如果射线与平面碰撞，打印碰撞物体信息  
                           // Debug.Log("碰撞对象: " + hit_1.collider.name + "   重新随机一个位置");
                            goto Random1;
                        }

                        Ray ray_2 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0.08f, Mathf.Sin(angel)));
                        RaycastHit hit_2;
                        if (Physics.Raycast(ray_2, out hit_2, obstacleAttributesList[i].Redius))
                        {
                            // 如果射线与平面碰撞，打印碰撞物体信息  
                           // Debug.Log("碰撞对象: " + hit_2.collider.name + "   重新随机一个位置");
                            goto Random1;
                        }

                        angel += 30;
                    }

                    //确定此位置不会和其他障碍物重合,将此位置坐标赋给次障碍物并随机旋转
                    if (angel == 360)
                    {
                        obstacle.transform.position = randomPosition_0;
                        obstacle.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                        Debug.Log("障碍物:" + obstacle.gameObject.name + "经过" + "<color=#00EEEE>" + currentTimes + "</color>" + "次后成功生成");
                    }
                    break;
                //在区域2中
                case 2:
                    Random2:
                    currentTimes++;
                    //若循环次数大于设定值,则此障碍物不会生成
                    if (currentTimes > randomTimes)
                    {
                        Destroy(obstacle);
                        Debug.LogError("障碍物:" + obstacle.gameObject.name + "未生成");
                        continue;
                    }
                    //在指定区域内随机生成一个点
                    randomX = Random.Range(randomArea2[0].x, randomArea2[1].x);
                    randomY = Random.Range(randomArea2[0].y, randomArea2[1].y);
                    randomPosition_0 = new Vector3(randomX, obstacle.transform.position.y, randomY) + area2.transform.position;
                    randomPosition_1 = new Vector3(randomX, obstacle.transform.position.y + 0.04f, randomY) + area2.transform.position;
                    randomPosition_2 = new Vector3(randomX, obstacle.transform.position.y + 0.08f, randomY) + area2.transform.position;
                    //检测此障碍物放置在当前位置是否会和其他障碍物重合 
                    angel = 0;
                    for (int j = 0; j < 12; j++)
                    {
                        Ray ray_0 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0, Mathf.Sin(angel)));
                        RaycastHit hit_0;
                        if (Physics.Raycast(ray_0, out hit_0, obstacleAttributesList[i].Redius))
                        {
                            // 如果射线与平面碰撞，打印碰撞物体信息  
                            //Debug.Log("碰撞对象: " + hit_0.collider.name + "   重新随机一个位置");
                            goto Random2;
                        }

                        Ray ray_1 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0.04f, Mathf.Sin(angel)));
                        RaycastHit hit_1;
                        if (Physics.Raycast(ray_1, out hit_1, obstacleAttributesList[i].Redius))
                        {
                            // 如果射线与平面碰撞，打印碰撞物体信息  
                            //Debug.Log("碰撞对象: " + hit_1.collider.name + "   重新随机一个位置");
                            goto Random2;
                        }

                        Ray ray_2 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0.08f, Mathf.Sin(angel)));
                        RaycastHit hit_2;
                        if (Physics.Raycast(ray_2, out hit_2, obstacleAttributesList[i].Redius))
                        {
                            // 如果射线与平面碰撞，打印碰撞物体信息  
                            //Debug.Log("碰撞对象: " + hit_2.collider.name + "   重新随机一个位置");
                            goto Random2;
                        }

                        angel += 30;
                    }

                    //确定此位置不会和其他障碍物重合,将此位置坐标赋给次障碍物并随机旋转
                    if (angel == 360)
                    {
                        obstacle.transform.position = randomPosition_0;
                        obstacle.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                        Debug.Log("障碍物:" + obstacle.gameObject.name + "经过" + "<color=#00EEEE>" + currentTimes + "</color>" + "次后成功生成");
                    }
                    break;
                //在区域3中
                case 3:
                    Random3:
                    currentTimes++;
                    //若循环次数大于设定值,则此障碍物不会生成
                    if (currentTimes > randomTimes)
                    {
                        Destroy(obstacle);
                        Debug.LogError("障碍物:" + obstacle.gameObject.name + "未生成");
                        continue;
                    }
                    //在指定区域内随机生成一个点
                    randomX = Random.Range(randomArea3[0].x, randomArea3[1].x);
                    randomY = Random.Range(randomArea3[0].y, randomArea3[1].y);
                    randomPosition_0 = new Vector3(randomX, obstacle.transform.position.y, randomY) + area3.transform.position;
                    randomPosition_1 = new Vector3(randomX, obstacle.transform.position.y + 0.04f, randomY) + area3.transform.position;
                    randomPosition_2 = new Vector3(randomX, obstacle.transform.position.y + 0.08f, randomY) + area3.transform.position;
                    //检测此障碍物放置在当前位置是否会和其他障碍物重合 
                    angel = 0;
                    for (int j = 0; j < 12; j++)
                    {
                        Ray ray_0 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0, Mathf.Sin(angel)));
                        RaycastHit hit_0;
                        if (Physics.Raycast(ray_0, out hit_0, obstacleAttributesList[i].Redius))
                        {
                            // 如果射线与平面碰撞，打印碰撞物体信息  
                            //Debug.Log("碰撞对象: " + hit_0.collider.name + "   重新随机一个位置");
                            goto Random3;
                        }

                        Ray ray_1 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0.04f, Mathf.Sin(angel)));
                        RaycastHit hit_1;
                        if (Physics.Raycast(ray_1, out hit_1, obstacleAttributesList[i].Redius))
                        {
                            // 如果射线与平面碰撞，打印碰撞物体信息  
                            //Debug.Log("碰撞对象: " + hit_1.collider.name + "   重新随机一个位置");
                            goto Random3;
                        }

                        Ray ray_2 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0.08f, Mathf.Sin(angel)));
                        RaycastHit hit_2;
                        if (Physics.Raycast(ray_2, out hit_2, obstacleAttributesList[i].Redius))
                        {
                            // 如果射线与平面碰撞，打印碰撞物体信息  
                           // Debug.Log("碰撞对象: " + hit_2.collider.name + "   重新随机一个位置");
                            goto Random3;
                        }

                        angel += 30;
                    }

                    //确定此位置不会和其他障碍物重合,将此位置坐标赋给次障碍物并随机旋转
                    if (angel == 360)
                    {
                        obstacle.transform.position = randomPosition_0;
                        obstacle.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                        Debug.Log("障碍物:" + obstacle.gameObject.name + "经过" + "<color=#00EEEE>" + currentTimes + "</color>" + "次后成功生成");
                    }
                    break;
                //在区域4中
                case 4:
                    Random4:
                    currentTimes++;
                    //若循环次数大于设定值,则此障碍物不会生成
                    if (currentTimes > randomTimes)
                    {
                        Destroy(obstacle);
                        Debug.LogError("障碍物:" + obstacle.gameObject.name + "未生成");
                        continue;
                    }
                    //在指定区域内随机生成一个点
                    randomX = Random.Range(randomArea4[0].x, randomArea4[1].x);
                    randomY = Random.Range(randomArea4[0].y, randomArea4[1].y);
                    randomPosition_0 = new Vector3(randomX, obstacle.transform.position.y, randomY) + area4.transform.position;
                    randomPosition_1 = new Vector3(randomX, obstacle.transform.position.y + 0.04f, randomY) + area4.transform.position;
                    randomPosition_2 = new Vector3(randomX, obstacle.transform.position.y + 0.08f, randomY) + area4.transform.position;
                    //检测此障碍物放置在当前位置是否会和其他障碍物重合 
                    angel = 0;
                    for (int j = 0; j < 12; j++)
                    {
                        Ray ray_0 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0, Mathf.Sin(angel)));
                        RaycastHit hit_0;
                        if (Physics.Raycast(ray_0, out hit_0, obstacleAttributesList[i].Redius))
                        {
                            // 如果射线与平面碰撞，打印碰撞物体信息  
                           // Debug.Log("碰撞对象: " + hit_0.collider.name + "   重新随机一个位置");
                            goto Random4;
                        }

                        Ray ray_1 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0.04f, Mathf.Sin(angel)));
                        RaycastHit hit_1;
                        if (Physics.Raycast(ray_1, out hit_1, obstacleAttributesList[i].Redius))
                        {
                            // 如果射线与平面碰撞，打印碰撞物体信息  
                           // Debug.Log("碰撞对象: " + hit_1.collider.name + "   重新随机一个位置");
                            goto Random4;
                        }

                        Ray ray_2 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0.08f, Mathf.Sin(angel)));
                        RaycastHit hit_2;
                        if (Physics.Raycast(ray_2, out hit_2, obstacleAttributesList[i].Redius))
                        {
                            // 如果射线与平面碰撞，打印碰撞物体信息  
                           // Debug.Log("碰撞对象: " + hit_2.collider.name + "   重新随机一个位置");
                            goto Random4;
                        }

                        angel += 30;
                    }

                    //确定此位置不会和其他障碍物重合,将此位置坐标赋给次障碍物并随机旋转
                    if (angel == 360)
                    {
                        obstacle.transform.position = randomPosition_0;
                        obstacle.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                        Debug.Log("障碍物:" + obstacle.gameObject.name + "经过" + "<color=#00EEEE>" + currentTimes + "</color>" + "次后成功生成");
                    }
                    break;
                default:

                    break;
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

            if (obstacleAttributesList[i].IsHaveTargetPosition)
            {
                foreach (Transform child in obstacle.transform)
                {
                    if (child.gameObject.tag == "Target")
                    {
                        child.gameObject.GetComponent<Target>().ID = obstacleAttributesList[i].TargetID;
                    }
                }
            }
        }
        for (int i = 0; i < obstacleAttributesList.Count; i++)
        {
            if (obstacleAttributesList[i].IsCollection)
            {
                //加载一个障碍物资源
                var obstaclePrefab = (GameObject)Resources.Load(obstacleAttributesList[i].Path);
                var obstacle = Instantiate(obstaclePrefab) as GameObject;
                obstacle.transform.parent = gameObject.transform;
                obstacle.transform.localScale = new Vector3(1, 1, 1);
                var randomTimes = obstacleAttributesList[i].RandomTimes;
                int currentTimes = 0;
                Random0:
                currentTimes++;
                //若循环次数大于设定值,则此障碍物不会生成
                if (currentTimes > randomTimes)
                {
                    Destroy(obstacle);
                    Debug.LogError("障碍物:" + obstacle.gameObject.name + "未生成");
                    continue;
                }
                //在指定区域内随机生成一个点
                randomX = Random.Range(randomArea0[0].x, randomArea0[1].x);
                randomY = Random.Range(randomArea0[0].y, randomArea0[1].y);
                randomPosition_0 = new Vector3(randomX, obstacle.transform.position.y, randomY) + transform.position;
                randomPosition_1 = new Vector3(randomX, obstacle.transform.position.y + 0.04f, randomY) + transform.position;
                randomPosition_2 = new Vector3(randomX, obstacle.transform.position.y + 0.08f, randomY) + transform.position;
                //检测此障碍物放置在当前位置是否会和其他障碍物重合 
                angel = 0;
                for (int j = 0; j < 12; j++)
                {
                    Ray ray_0 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0, Mathf.Sin(angel)));
                    RaycastHit hit_0;
                    if (Physics.Raycast(ray_0, out hit_0, obstacleAttributesList[i].Redius))
                    {
                        // 如果射线与平面碰撞，打印碰撞物体信息  
                       // Debug.Log("碰撞对象: " + hit_0.collider.name + "   重新随机一个位置");
                        goto Random0;
                    }

                    Ray ray_1 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0.04f, Mathf.Sin(angel)));
                    RaycastHit hit_1;
                    if (Physics.Raycast(ray_1, out hit_1, obstacleAttributesList[i].Redius))
                    {
                        // 如果射线与平面碰撞，打印碰撞物体信息  
                       // Debug.Log("碰撞对象: " + hit_1.collider.name + "   重新随机一个位置");
                        goto Random0;
                    }

                    Ray ray_2 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0.08f, Mathf.Sin(angel)));
                    RaycastHit hit_2;
                    if (Physics.Raycast(ray_2, out hit_2, obstacleAttributesList[i].Redius))
                    {
                        // 如果射线与平面碰撞，打印碰撞物体信息  
                       // Debug.Log("碰撞对象: " + hit_2.collider.name + "   重新随机一个位置");
                        goto Random0;
                    }

                    angel += 30;
                }

                //确定此位置不会和其他障碍物重合,将此位置坐标赋给次障碍物并随机旋转
                if (angel == 360)
                {
                    obstacle.transform.position = randomPosition_0;
                    obstacle.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                    Debug.Log("障碍物:" + obstacle.gameObject.name + "经过" + "<color=#00EEEE>" + currentTimes + "</color>" + "次后成功生成");
                }
            }
            
        }

    }

    // Update is called once per frame
    void Update()
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

    public List<ObstacleAttributes> GetObstacleAttributes()
    {
        if (obstacleAttributesList.Count == 0)
        {
            Debug.LogError("场景中没有障碍属性的收集物,获取失败");
            return null;
        }
        else
        {
            return obstacleAttributesList;
        }
    }
}
