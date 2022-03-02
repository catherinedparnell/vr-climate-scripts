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
    // data variables
    private string formattedURL;
    public int year;
    public int scenario;
    public GameObject water;
    public ClimateData data;

    // lerp variables
    public float riseSpeed=.5f;
    private Vector3 startPos;
    private Vector3 endPos;
    private float lerpFraction = 0;
    private bool updatingWaterLevel = false;
    public int RISE_FACTOR = 5; 

    // // Start is called before the first frame update
    void Start()
    {
        StartSimulation(year, scenario);
    }

    // Update is called once per frame
    void Update()
    {
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

    // gets attribute from JSON object
    private float getAttribute(string attr, JSONNode response) {
       return (float) response[attr];
    }

    // generates the request for API with formatted URL 
    public void GenerateRequest(int year, int scenario)
    {
        formattedURL = "https://vr-climate-api.herokuapp.com/decade?year="+year+"&scenario="+scenario;
        StartCoroutine(ProcessRequest(formattedURL));
    }
    // Processes the request from the API
    private IEnumerator ProcessRequest(string uri)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }

            JSONNode response = JSON.Parse(request.downloadHandler.text);
            print(response);

            // creates new ClimateData object for data collected from API
            data = new ClimateData(getAttribute("precip", response), getAttribute("sea_level", response), getAttribute("temperature", response));
        }
        // sets the start and end position for lerp before it starts
        startPos = water.transform.position;
        endPos = new Vector3(water.transform.position.x, water.transform.position.y+(RISE_FACTOR*data.SeaLevel), water.transform.position.z);
        
        // setting updatingWaterLevel to true starts the lerp aniamtion
        updatingWaterLevel = true;
    }
    
    // starts the request with a given year and scenario
    public void StartSimulation(int year, int scenario)
    {
        GenerateRequest(year, scenario);
    }

}
