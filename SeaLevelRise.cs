using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine;
using static ClimateData;
using TMPro;

public class SeaLevelRise : MonoBehaviour
{
    public GameObject water;
    public ClimateData data;

    public Request Request;

    // lerp variables
    public float riseSpeed=.5f;
    private Vector3 startPos;
    private Vector3 endPos;
    private float lerpFraction = 0;
    private bool updatingWaterLevel = false;
    public int RISE_FACTOR = 5; 

    void Update()
    {
        Debug.Log(Request.seaRetrieved);
        if (Request.seaRetrieved)
        {
            data = Request.data;
            
            // sets the start and end position for lerp before it starts
            startPos = water.transform.position;
            endPos = new Vector3(water.transform.position.x, water.transform.position.y+(RISE_FACTOR*data.SeaLevel), water.transform.position.z);
            
            // setting updatingWaterLevel to true starts the lerp aniamtion
            updatingWaterLevel = true;
            Request.seaRetrieved = false;
        }

        // if water level is being updated
        if(updatingWaterLevel)
        {
            //lerp the sea level
            LerpSea();
        }
    }
    // lerp animation for sea level
    private void LerpSea()
    {
        // move sea plane
        if (lerpFraction < 0.99)
        {
            lerpFraction += Time.deltaTime * riseSpeed;
            water.transform.position = Vector3.Lerp(startPos, endPos, lerpFraction);
        }
        // sea plane is done moving
        else
        {
            // no longer updating the water level
            updatingWaterLevel = false;

            // reset lerpFraction for next sea level update
            lerpFraction = 0;
        }
    }
}
