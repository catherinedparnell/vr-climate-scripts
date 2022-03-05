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

    public int rainDelay = 2; // how long to wait before start raining
    public bool makingWeather = false;

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

    public IEnumerator MakeWeather()
    {
        makingWeather = true;
        yield return new WaitForSeconds(rainDelay);
        MakeRain(data.Precipitation);
        yield return new WaitForSeconds(rainAmount);
        ResetRain();
        makingWeather = false;
    }

    public void MakeRain(float rainAmount)
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
            ResetRain();
        }
    }

    public void ResetRain() {
        Rain.SetActive(false);
        var emission = rainPart.emission;
        emission.rateOverTime = 0;
    }
}