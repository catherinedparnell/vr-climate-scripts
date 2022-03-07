using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    Request Request;
    public ClimateData data;

    //Rain variables:
    public GameObject Rain; 
    public float eRate; // emission rate of the particles
    private ParticleSystem rainPart;

    public GameObject Clouds;

    public int rainDelay = 2; // how long to wait before start raining
    public bool makingWeather = false;

    public Thunderstorm Thunderstorm;
    public Downpour Downpour;
    public AmbientCoast AmbientCoast;

    private int rainCutoff = 8; // distinction between downpour and thunderstorm
    public float extremeRainProb = 0.6f;
    public float averageRainProb = 0.35f;
    private float rainProb;

    public void Start()
    {
        Request = gameObject.GetComponentInParent<Request>();
        rainPart = Rain.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (Request.retrieved)
        {
            data = Request.data;
            StartCoroutine(MakeWeather());
            Request.retrieved = false;
        }
        if (Request.chosenScenario == 2) {
            rainProb = extremeRainProb;
        } else {
            rainProb = averageRainProb;
        }
    }

    public IEnumerator MakeWeather()
    {
        makingWeather = true;
        yield return new WaitForSeconds(rainDelay);
        MakeRain(data.Precipitation);
        yield return new WaitForSeconds(data.Precipitation + rainDelay);
        ResetRain(data.Precipitation);
        makingWeather = false;
    }

    public void MakeRain(float rainAmount)
    {
        float ifRain = Random.Range(0, 100);
        float ifRainPercent = ifRain / 100;
        Debug.Log("If rain: "+ifRain+" "+ifRainPercent+" Rain prob: "+rainProb);
        if (rainAmount > 0 && ifRainPercent < rainProb)
        {
            Debug.Log("it will rain");
            Rain.SetActive(true);
            Clouds.SetActive(true);
            var emission = rainPart.emission.rateOverTime;
            eRate = rainAmount * 100;
            emission = eRate;

            if (rainAmount > rainCutoff) {
                Thunderstorm.StartThunderstormSound();
            } 
            else {
                Downpour.StartDownpourSound();
            }
            AmbientCoast.PauseCoastSound();
        }
        else
        {
            ResetRain(rainAmount);
        }
    }

    public void ResetRain(float rainAmount) {
        if (rainAmount > rainCutoff) {
            Thunderstorm.StopThunderstormSound();
        } 
        else if (0 < rainAmount) {
            Downpour.StopDownpourSound();
        }

        Rain.SetActive(false);
        Clouds.SetActive(false);
        var emission = rainPart.emission;
        emission.rateOverTime = 0;

        AmbientCoast.PlayCoastSound();
    }
}