using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    Request Request;
    public ClimateData data;

    //Rain variables:
    public GameObject Rain; 
    public ParticleSystem rainPart;
    private float rainAmount; // data retrived from the API
    public float eRate; // emission rate of the particles


    public void Start()
    {
        Request = gameObject.GetComponentInParent<Request>();
        rainPart = GetComponent<ParticleSystem>(); // rain particle system 
    }

    void Update()
    {
        if (Request.retrieved)
        {
            data = Request.data;
            StartCoroutine(MakeWeather());
            Request.retrieved = false;
        }
    }

    IEnumerator MakeWeather()
    {
        Debug.Log("make weather data: " + data);
        MakeRain(data.Precipitation);
        Debug.Log("data.Precipitation: " + data.precipitation);
        yield return new WaitForSeconds(10);
    }

    void MakeRain(float rainAmount)
    {
        if (rainAmount > 0)
        {
            Rain.SetActive(true);
            var emission = rainPart.emission.rateOverTime;
            eRate = rainAmount * 100;
            emission = eRate;
        }
        else
        {
            Rain.SetActive(false);
            var emission = rainPart.emission;
            emission.rateOverTime = 0;
        }
    }
}




/*

    // Start is called before the first frame update
    void Start()
    {
        rainPart = GetComponent<ParticleSystem>();
        StartCoroutine(rainVol());
    }

    // Update is called once per frame
    void Update()
    {
        if (isRaining)
        {
            var emission = rainPart.emission;
            rainAmount = MainController.precipAmount;
            eRate = rainAmount * 10;
            emission.rateOverTime = eRate;
        }
    }

    IEnumerator rainVol()
    {
        yield return new WaitForSeconds(10);
        eRate = 500;
        yield return new WaitForSeconds(6);
        eRate = 200;
        yield return new WaitForSeconds(6);
        eRate = 300;

    }*/