using System.Collections;
using UnityEngine;
using DG.Tweening;
using Player;
public class ShadowTrigger : MonoBehaviour{
    public Vector3 SpecifiedPos;
    public Vector3 SpecifiedRotation;

    void OnTriggerEnter(Collider hit)
    {
        if(hit.transform.root.tag == "Player")
        {
           
            hit.transform.root.GetComponent<PlayerController>().canChangeSize = true;
            hit.transform.root.GetComponent<PlayerController>().TolerantPosY = hit.transform.position.y;
        }
    }

    void OnTriggerStay(Collider hit)
    {
        if(Input.GetKeyDown(KeyCode.G)&&hit.transform.root.tag == "Player")
        {
            StartCoroutine(hit.transform.root.GetComponent<PlayerAction>().ShadowAttachAction(SpecifiedPos, SpecifiedRotation));
        }
    }
    void OnTriggerExit(Collider hit)
    {
        if(hit.transform.root.tag == "Player")
        {
            hit.transform.root.GetComponent<PlayerController>().canChangeSize = false;
        }
    }

    
	
}
