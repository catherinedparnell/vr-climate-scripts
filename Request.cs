using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;

public class Request : MonoBehaviour
{
    private string formattedURL;
    public bool retrieved = false;
    public bool seaRetrieved = false;
    public ClimateData data;
    public int chosenYear;
    public int chosenScenario;

    // void Start() {
    //     StartSimulation(2099, 2);
    // }


    public void StartSimulation(int year, int scenario)
    {
        Debug.Log("STARTING SIMULATION "+year+" "+scenario);
        chosenYear = year;
        chosenScenario = scenario;
        GenerateRequest(year, scenario);
    }

    public void GenerateRequest (int year, int scenario)
    {
        formattedURL = ServerConfig.URL + ServerConfig.QUERY_YEAR + year.ToString() + "&" +ServerConfig.QUERY_SCENARIO + scenario.ToString();
        Debug.Log(formattedURL);
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
            seaRetrieved = true;
        }
    }

    private float getAttribute(string attr, JSONNode response)
    {
        return (float)response[attr];
    }
}