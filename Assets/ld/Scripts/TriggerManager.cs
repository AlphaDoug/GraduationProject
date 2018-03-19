using UnityEngine;
using LightControl;
public class TriggerManager : MonoBehaviour {
    public GameObject[] TriggerObj;

    public void SetOnlyOneActive(DirectEnum dir)
    {
        for(int i = 0; i < TriggerObj.Length; i++)
        {
            if(i == (int)dir) TriggerObj[i].SetActive(true);
            else
                TriggerObj[i].SetActive(false);
        }

    }

}
