using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    /// <summary>
    /// 下一个收集物
    /// </summary>

    public delegate void CollectOneThing();

    public static event CollectOneThing CollectOneThingEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            CollectOneThingEvent();
            //transform.parent.gameObject.GetComponent<CollectionUI>().AddOneCollection();
        }
    }
}
