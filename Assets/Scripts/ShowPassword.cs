using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Pwd
{
    public GameObject pwdObj;
    public string pwd;
}
public class ShowPassword : MonoBehaviour
{

    public List<Pwd> pwds = new List<Pwd>();
    public TextMesh textMesh;

    public void AddLetter(string x)
    {
        textMesh.text += x;
    }
}
