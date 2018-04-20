using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    /// <summary>
    /// 宝箱的ID
    /// </summary>
    public int ID;
    /// <summary>
    /// 宝箱的描述
    /// </summary>
    public string DES;
    public delegate void CollectOneThing(int id,string des);

    public static event CollectOneThing CollectOneThingEvent;
    /// <summary>
    /// 收集指定物品
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            CollectOneThingEvent(ID, DES);
            Debug.Log("收集到一个" + DES);
        }
    }
}
