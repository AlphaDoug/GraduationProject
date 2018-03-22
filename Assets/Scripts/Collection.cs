using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    /// <summary>
    /// 下一个收集物
    /// </summary>
    public GameObject nextCollection;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (nextCollection != null)
            {
                nextCollection.SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().SetNextGroupTrue();

            }
            transform.parent.gameObject.GetComponent<CollectionUI>().AddOneCollection();
        }
    }
}
