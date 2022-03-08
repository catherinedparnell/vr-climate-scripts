using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBinocular : MonoBehaviour
{
    public static bool isNear;

    void Start()
    {
        isNear = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            Debug.Log("on trigger enter:" + isNear);
            isNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            isNear = false;
    }

    public static bool isCloseEnough()
    {
        return (isNear);
    }
}
