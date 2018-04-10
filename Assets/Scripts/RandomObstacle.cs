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
    }
    /// <summary>
    /// 随机生成障碍物的区域
    /// </summary>
    private Vector2[] randomArea = { new Vector2(-3, -3), new Vector2(3, 3)};
    private int totalNum;
    private OOFormArray mForm = null;
    private OOFormArray mForm1 = null;
    private List<ObstacleAttributes> obstacleAttributesList = new List<ObstacleAttributes>();
    private void Awake()
    {
        #region 加载TbPortals属性表
        if (mForm == null)
        {
            mForm = OOFormArray.ReadFromResources("Data/Tables/TbObstacle");
        }
        #endregion

        #region 加载TbSceneConfig属性表
        if (mForm1 == null)
        {
            mForm1 = OOFormArray.ReadFromResources("Data/Tables/TbSceneConfig");
        }
        #endregion

        randomArea[0] = new Vector2(float.Parse(mForm1.GetString("RandomArea", "0").Split('|')[0].Split(',')[0]), float.Parse(mForm1.GetString("RandomArea", "0").Split('|')[0].Split(',')[1]));
        randomArea[1] = new Vector2(float.Parse(mForm1.GetString("RandomArea", "0").Split('|')[1].Split(',')[0]), float.Parse(mForm1.GetString("RandomArea", "0").Split('|')[1].Split(',')[1]));
        totalNum = mForm1.GetInt("ObstacleNum", "0");

        for (int i = 0; i < totalNum; i++)
        {
            var obstacleAttributes = mForm.GetObject<ObstacleAttributes>(Random.Range(1, mForm.mRowCount - 1));
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
                    Debug.Log("碰撞对象: " + hit_0.collider.name);
                    // 在场景视图中绘制射线  
                    Debug.DrawLine(ray_0.origin, hit_0.point, Color.red);
                    goto Random;
                }

                Ray ray_1 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0.04f, Mathf.Sin(angel)));
                RaycastHit hit_1;
                if (Physics.Raycast(ray_1, out hit_1, obstacleAttributesList[i].Redius))
                {
                    // 如果射线与平面碰撞，打印碰撞物体信息  
                    Debug.Log("碰撞对象: " + hit_1.collider.name);
                    // 在场景视图中绘制射线  
                    Debug.DrawLine(ray_1.origin, hit_1.point, Color.red);
                    goto Random;
                }

                Ray ray_2 = new Ray(randomPosition_0, new Vector3(Mathf.Cos(angel), 0.08f, Mathf.Sin(angel)));
                RaycastHit hit_2;
                if (Physics.Raycast(ray_2, out hit_2, obstacleAttributesList[i].Redius ))
                {
                    // 如果射线与平面碰撞，打印碰撞物体信息  
                    Debug.Log("碰撞对象: " + hit_2.collider.name);
                    // 在场景视图中绘制射线  
                    Debug.DrawLine(ray_2.origin, hit_2.point, Color.red);
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
            
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
