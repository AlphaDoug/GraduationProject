using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CruiseRadius : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        transform.parent.GetComponent<RobotAIController>().OnTriggerEnterChild(other);
    }

    private void OnTriggerExit(Collider other)
    {
        transform.parent.GetComponent<RobotAIController>().OnTriggerExitChild(other);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("111111111");
    }
}
