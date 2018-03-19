using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using DG.Tweening;
using System;
public class PlayerAction : MonoBehaviour {

    public float AutoActionTime = 0.8f;
    public float RevertTime = 0.2f;
    
    
    public static Action fallDown;

    private Transform _transform;
    void Start () {
        _transform = GetComponent<PlayerController>().sizeTransform;
	}
	

    public IEnumerator ShadowAttachAction(Vector3 pos,Vector3 rotate)
    {
        transform.DOMove(pos, AutoActionTime);
        transform.DORotate(rotate, AutoActionTime);
        PlayerController.CloseControll();
        yield return new WaitForSeconds(AutoActionTime);
        //TODO趴下附着

        GetComponent<PlayerController>().animator.SetTrigger("Fall");
        yield return new WaitForSeconds(2);
        UIController.CloseMessageBox();

        GetComponent<PlayerController>().animator.SetTrigger("Recover");
       // UIController.SetDialogBoxContext()


        if(fallDown!=null)fallDown();
       // Debug.Log("趴下附着");
    }
    public void RevertTolerantSize(Vector3 size,float posY)
    {
        _transform.DOScale(size, RevertTime);
        _transform.DOLocalMoveY(posY, RevertTime);
     
    }
    
}
