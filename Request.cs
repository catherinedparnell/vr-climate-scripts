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


    public void StartSimulation(int year, int scenario)
    {
        GenerateRequest(year - 10, scenario);
    }

    public void GenerateRequest (int year, int scenario)
    {
        formattedURL = ServerConfig.DECADE_URL + ServerConfig.QUERY_YEAR + year.ToString() + "&" +ServerConfig.QUERY_SCENARIO + scenario.ToString();
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
