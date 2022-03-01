//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Networking;
//using SimpleJSON;
//using TMPro;
//using System;

//public class God : MonoBehaviour
//{
//    public int perPlasticAmount = 25;
//    public GameObject plasticRings;
//    public GameObject plasticPipe;
//    public float deathProbability = 0.9f;
//    [SerializeField] GameObject plasticInput;
//    [SerializeField] TextMeshProUGUI plasticText;

//    public GameObject turtle;
//    public int initialPopulation;
//    [SerializeField] GameObject initialPopulationInput;
//    [SerializeField] TextMeshProUGUI initialPopulationText;
//    public int deathByPlastic = 0;
//    public int deathPopulation = 0;
//    public float femaleProbability;
//    [SerializeField] TextMeshProUGUI femaleText;
//    public int turtleRange;
//    public float babyDeathProbability = 0.8f;
//    [SerializeField] TextMeshProUGUI babyDeathText;

//    private const string URL = ServerConfig.SERVER_API_URL;

//    public int timescale;
//    [SerializeField] GameObject timescaleInput;

//    public float maturityTime;
//    public float maxAge;
//    public float hatchTime;
//    public float incubationTime;
//    public float seasonTime;

//    public float decadeTime;

//    public int bounds = 240;

//    public GameObject beach;
//    public int beachCount;

//    public bool readyToCollect = false;

//    public UnityEngine.UI.Button startButton;

//    void Start()
//    {
//        turtleRange = bounds - 40;
//        UnityEngine.UI.Button btn = startButton.GetComponent<UnityEngine.UI.Button>();
//		btn.onClick.AddListener(StartSimulation);

//        plasticText.text = plasticText.text + " " + deathProbability;
//        babyDeathText.text = babyDeathText.text + " " + (1 - babyDeathProbability);
//    }

//    void StartSimulation()
//    {
//        initialPopulation = (int) Int32.Parse(initialPopulationInput.GetComponent<TMP_InputField>().text);
//        perPlasticAmount = (int) Int32.Parse(plasticInput.GetComponent<TMP_InputField>().text) / 2;
//        timescale = (int) Int32.Parse(timescaleInput.GetComponent<TMP_InputField>().text);
//        femaleProbability = (float) Mathf.Round(GameObject.Find("Slider").GetComponent<Slider>().value * 100f) / 100f;

//        initialPopulationText.text = initialPopulationText.text + " " + initialPopulation;
//        femaleText.text = femaleText.text + " " + femaleProbability;

//        GeneratePlastic();
//        GenerateTimescaleRequest(timescale);
//    }

//    public void GenerateTimescaleRequest(int days_second)
//    {
//        StartCoroutine(ProcessTimescaleRequest(URL + "?" + ServerConfig.API_QUERY_DAYS_PER_SECOND + '=' + days_second));
//    }

//    private IEnumerator ProcessTimescaleRequest(string uri)
//    {
//        using (UnityWebRequest request = UnityWebRequest.Get(uri))
//        {
//            yield return request.SendWebRequest();

//            if (request.isNetworkError)
//            {
//                Debug.Log(request.error);
//            }
//            else
//            {
//                Debug.Log(request.downloadHandler.text);
//            }

//            JSONNode timescaleData = JSON.Parse(request.downloadHandler.text);

//            maturityTime = (float) timescaleData["maturityTime"];
//            maxAge = (float) timescaleData["maxAge"];
//            hatchTime = (float) timescaleData["hatchTime"];
//            incubationTime = (float) timescaleData["incubationTime"];
//            decadeTime = (float) timescaleData["decadeTime"];
//            seasonTime = (float) timescaleData["seasonTime"];

//            GenerateGeoRequest("high");
//        }
//    }

//     public void GenerateGeoRequest(string density)
//    {
//        StartCoroutine(ProcessGeoRequest(URL + ServerConfig.API_GEO_ENDPOINT + "?" + ServerConfig.API_QUERY_DENSITY + '=' + density));
//    }

//    private IEnumerator ProcessGeoRequest(string uri)
//    {
//        using (UnityWebRequest request = UnityWebRequest.Get(uri))
//        {
//            yield return request.SendWebRequest();

//            if (request.isNetworkError)
//            {
//                Debug.Log(request.error);
//            }
//            else
//            {
//                Debug.Log(request.downloadHandler.text);
//            }

//            JSONNode geoData = JSON.Parse(request.downloadHandler.text);

//            GenerateBeaches(geoData);
//        }
//    }

//    private void GeneratePlastic () {
//        GameObject[] plastics = new GameObject[] { plasticRings, plasticPipe };

//        // initialize amount of plastics in the ocean
//        for (int i = 0; i < plastics.Length; i++)
//        {
//            for (int j = 0; j < perPlasticAmount; j++)
//            {
//                Vector3 pos = new Vector3(UnityEngine.Random.Range(-bounds - 10, bounds + 10), 1, UnityEngine.Random.Range(-bounds - 10, bounds + 10));
//                Quaternion rot = Quaternion.Euler(UnityEngine.Random.Range(-150, 0), UnityEngine.Random.Range(-150,150), UnityEngine.Random.Range(-150,0));
//                Instantiate(plastics[i], pos, rot, transform);
//            }
//        } 
//    }

//    private void GenerateTurtles (int x, Vector3 beachPos) {
//        // initialize scene with X number of turtles
//        Debug.Log("generating turtles...");

//        for (int i = 0; i < x; i++)
//        {
//            GameObject Turtle = Instantiate(turtle, new Vector3(UnityEngine.Random.Range(-turtleRange,turtleRange), 0, UnityEngine.Random.Range(-turtleRange, turtleRange)), Quaternion.Euler(0, UnityEngine.Random.Range(-180, 180), 0), transform);
//            Turtle.GetComponent<Turtle>().child = false;

//            float randomFloat = UnityEngine.Random.value;
//            if(UnityEngine.Random.value < femaleProbability)
//            {
//                Turtle.GetComponent<Turtle>().female = true;
//            }

//            Turtle.GetComponent<Turtle>().beach = beachPos;
//        } 
//    }

//    private void GenerateBeaches (JSONNode response) {
//        beachCount = response["high_beaches"].Count;
//        int turtlesPerBeach = initialPopulation / beachCount;

//        float buffer = (bounds) * 4f / beachCount * 2f;

//        float x = -bounds;
//        float z = -bounds;

//        for (int i = 0; i< response["high_beaches"].Count; i++)
//        {
//            Vector3 pos = new Vector3(x, 0, z);

//            GameObject Beach = Instantiate(beach, pos, beach.transform.rotation);
//            Beach.GetComponent<Beach>().name = response["high_beaches"][i]["Beach"];

//            GenerateTurtles(turtlesPerBeach, pos);

//            // this can be made more efficient
//            if (x + buffer <= bounds && z == -bounds)
//            {
//                x = x + buffer;
//            }
//            else if (x == bounds && z + buffer >= bounds)
//            {
//                z = bounds;
//            }
//            else
//            {
//                if (z + buffer <= bounds && x + buffer >= bounds)
//                {
//                    x = bounds;
//                }
//                if (z + buffer <= bounds && x == bounds)
//                {
//                    z = z + buffer;
//                }
//            }
            
//            if (z == bounds && x - buffer >= -bounds)
//            {
//                x = x - buffer;
//            } 
//            else if (z == bounds && x - buffer <= bounds)
//            {
//                x = -bounds;
//            }
//            if (x == -bounds && z - buffer >= -bounds)
//            {
//                z = z - buffer;
//            }

//        }

//        readyToCollect = true;
//    }
//}
