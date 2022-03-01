using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;

public class Request : MonoBehaviour
{
    private string startDate;
    private string endDate;
    private string formattedURL;

    public bool retrieved = false;

    public GameObject Weather;
    public DatePicker DatePicker;

    public GameObject yearDropdownObj;
    public GameObject monthDropdownObj;
    public GameObject dayDropdownObj;

    public List<HourlyData> data;

    private float getAttribute(string attr, JSONNode response) {
       return (float) response[attr];
    }

    public void GenerateRequest(Date start, Date end)
    {
        startDate = start.ToString();
        endDate = end.ToString();
        formattedURL = ServerConfig.URL+"?postal_code="+ServerConfig.ZIP+"&start_date="+startDate+"&end_date="+endDate+"&units="+ServerConfig.UNITS+"&tz="+ServerConfig.TZ+"&key="+ServerConfig.API_KEY;
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
            
            data = new List<HourlyData>();
            for (int i=0; i<response["data"].Count; i++){
                HourlyData dataByHour = new HourlyData(getAttribute("wind_spd", response["data"][i]), getAttribute("wind_dir", response["data"][i]), getAttribute("clouds", response["data"][i]), getAttribute("precip", response["data"][i]), getAttribute("snow", response["data"][i]), getAttribute("vis", response["data"][i]), getAttribute("solar_rad", response["data"][i]), response["data"][i]["datetime"]);
                data.Add(dataByHour);
            }

            retrieved = true;
        }
    }

    public void StartSimulation()
    {
        List<string> years = DatePicker.years;
        
        TMP_Dropdown yearDropdown = yearDropdownObj.GetComponent<TMP_Dropdown>();
        TMP_Dropdown monthDropdown = monthDropdownObj.GetComponent<TMP_Dropdown>();
        TMP_Dropdown dayDropdown = dayDropdownObj.GetComponent<TMP_Dropdown>();

        Date start = new Date(Int32.Parse(years[yearDropdown.value]), monthDropdown.value, dayDropdown.value);
        Date end = new Date(Int32.Parse(years[yearDropdown.value]), monthDropdown.value, dayDropdown.value + 1);

        Debug.Log("date is "+start.ToString()+ " end is "+ end.ToString());

        GenerateRequest(start, end);
    }
}
