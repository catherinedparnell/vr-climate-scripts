using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;

public class Request : MonoBehaviour
{
    public const string serverURL = "https://vr-climate-api.herokuapp.com";
    private string yearClimate;
    private string scenarioClimate;
    private string formattedURL;

    public bool retrieved = false;

    public GameObject WeatherController;

    public ClimateData data;


    public void StartSimulation()
    {
        GenerateRequest(2090, 2); // testing with best scenario
    }

    public void GenerateRequest (int year, int scenario)
    {
        yearClimate = year.ToString();
        scenarioClimate = scenario.ToString();
        formattedURL = serverURL + "/decade?year=" + yearClimate + "&scenario=" + scenarioClimate;
        StartCoroutine(ProcessRequest(formattedURL));
    }

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
            data = new ClimateData(getAttribute("precip", response), getAttribute("sea_level", response), getAttribute("temperature", response));
            retrieved = true;
        }
    }

    private float getAttribute(string attr, JSONNode response)
    {
        return (float)response[attr];
    }
}
