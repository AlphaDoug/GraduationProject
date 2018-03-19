using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginSceneController : MonoBehaviour
{

    public GameObject[] gameObjectsOfChildren;
    [SerializeField] private GameObject nextGameObjectToSetActive;
    [Range(1f, 10f)] [SerializeField] private float timeOfEachPicture;


    private void Awake()
    {
        //片头过场的子物体的数组
        gameObjectsOfChildren = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            gameObjectsOfChildren[i] = transform.GetChild(i).gameObject;
        }

    }

    private void Start()
    {
        //下面的开始进行循环
        /*----------------*/
        StartCoroutine(TheLastHandle());
        for (int i = 0; i < transform.childCount; i++)
        {
            StartCoroutine(TheControllerOfPassingFirst(i));
        }
        /*----------------------*/
    }

    /// <summary>
    /// 过场动画的流程控制
    /// </summary>
    /// <returns></returns>
    IEnumerator TheControllerOfPassingFirst(int index)
    {
        if (transform.childCount <= 0)
        {
            yield return new WaitForEndOfFrame();
        }
        else
        {
            yield return new WaitForSeconds(index * timeOfEachPicture);
            transform.GetChild(index).gameObject.SetActive(true);
            if (index > 0)
            {
                transform.GetChild(index - 1).gameObject.SetActive(false);
            }
        }
    }

    IEnumerator TheLastHandle()
    {
        yield return new WaitForSeconds(transform.childCount * timeOfEachPicture);
        transform.GetChild(transform.childCount - 1).gameObject.SetActive(false);
        if (nextGameObjectToSetActive != null)
        {
            nextGameObjectToSetActive.SetActive(true);
        }
    }

    private void MovingThePassingPicture(GameObject m_gameObject)
    {

    }

}
