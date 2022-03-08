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

    public bool isRaining = false;

    private CloudFadeInOut cloudFade;

    public void Start()
    {
        Request = gameObject.GetComponentInParent<Request>();
        rainPart = Rain.GetComponent<ParticleSystem>();
        cloudFade = Clouds.GetComponent<CloudFadeInOut>();
    }

    void Update()
    {
        if (Request.retrieved)
        {
            Debug.Log("got data in weather controller");
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
        Debug.Log("making weather");
        makingWeather = true;
        yield return new WaitForSeconds(rainDelay);
        GameObject rain = MakeRain(data.Precipitation);
        yield return new WaitForSeconds(data.Precipitation + rainDelay);
        if (rain != null) {
            Debug.Log("destroying rain");
            ResetRain(data.Precipitation);
        }
        makingWeather = false;
    }

    public GameObject MakeRain(float rainAmount)
    {
        float ifRain = Random.Range(0, 100);
        float ifRainPercent = ifRain / 100;
        Debug.Log("If rain: "+ifRain+" "+ifRainPercent+" Rain prob: "+rainProb);
        if (rainAmount > 0 && ifRainPercent < rainProb)
        {
            Debug.Log("it will rain");
            isRaining = true;
            Rain.SetActive(true);
            cloudFade.SetFadeIn();
            var emission = rainPart.emission.rateOverTime;
            eRate = rainAmount * 100;
            emission = eRate;

            if (rainAmount > rainCutoff && Rain.activeSelf) {
                Thunderstorm.StartThunderstormSound();
            } 
            else if (Rain.activeSelf) {
                Downpour.StartDownpourSound();
            }
            AmbientCoast.PauseCoastSound();
            return Rain;
        }
        return null;
    }

    public void ResetRain(float rainAmount) {
            if (rainAmount > rainCutoff) {
                Thunderstorm.StopThunderstormSound();
            } 
            else if (0 < rainAmount) {
                Downpour.StopDownpourSound();
            }
            Debug.Log("stopped raining");
            Rain.SetActive(false);
            isRaining = false;
            cloudFade.SetFadeOut();

            AmbientCoast.PlayCoastSound();
    }
}