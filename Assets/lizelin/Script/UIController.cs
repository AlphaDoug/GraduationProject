using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    // /*-----死亡的ui对应的prefabs--------*/
    // [SerializeField] private GameObject diedUIInteractIcon;
    // private static GameObject diedUIInteractIconStaticTemp;
    // private static GameObject diedUIInteractIconStatic;

    // /*------交互键F 提示-----*/
    // [SerializeField] private GameObject remindSignForInteractWithF;
    // private static GameObject remindSignForInteractWithFStaticTemp;
    // private static GameObject remindSignForInteractWithFStatic;

    private static UIController instance;

    [SerializeField]
    private GameObject messageBox ;

    [SerializeField]
    private GameObject dialogBox;

    [SerializeField]
    private GameObject restartButton;

    /*------DialogBox-----*/
   // [SerializeField] private GameObject dialogBoxPrefab;
  //  private static GameObject dialogBoxPrefabStaticTemp;
  //  private static GameObject dialogPrefabStatic;

    private void Awake()
    {  
         instance=this;

         
        // diedUIInteractIconStatic = diedUIInteractIcon;
        // remindSignForInteractWithFStatic = remindSignForInteractWithF;
        //dialogPrefabStatic = dialogBoxPrefab;
    }

    // public static void SetGameOverUIDisplay(bool judgement)
    // {
    //     if (judgement&&!diedUIInteractIconStaticTemp)
    //     {
    //         diedUIInteractIconStaticTemp = Instantiate(diedUIInteractIconStatic);
    //     }
    //     if (!judgement && diedUIInteractIconStaticTemp != null)
    //     {
    //         Destroy(diedUIInteractIconStaticTemp);
    //     }
    // }




    public static void SetMessageBoxContext(string m_string)
    {

       instance.messageBox.SetActive(true);
        instance.messageBox.transform.GetComponentInChildren<Text>().text=m_string;

    }

    public static void CloseMessageBox()
    {
        instance.messageBox.SetActive(false);
    }
    public static void SetDialogBoxContext(string m_string)
    {
         instance.dialogBox.SetActive(true);
        instance.dialogBox.transform.GetComponentInChildren<Text>().text=m_string;
    }
    public static void CloseDialogBox()
    {
        instance.dialogBox.SetActive(false);
    }
    public  static void RestartButton()
    {
        instance.restartButton.SetActive(true);

    }
    public void CloseMessageBoxButtonEvent()
    {
		UIController.CloseDialogBox();
		Player.PlayerController.EnableControll();
		UIController.CloseMessageBox();
    }
    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
        Time.timeScale=1;
    }
}
