using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MyUI/Scripts/Components")]
public class DialogBoxComponent : MonoBehaviour
{

    [Multiline(5)] public string matchingString;

    private void Awake()
    {
        transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = matchingString;
    }
    public void SetString(string m_string)
    {
        transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = m_string;
    }
}
