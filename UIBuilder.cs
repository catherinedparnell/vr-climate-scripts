using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

// Show off all the Debug UI components.
public class UIBuilder : MonoBehaviour
{
    bool inMenu;
    private Text sliderText;
    public Request Request;
    public WeatherController WeatherController;
    public GameObject Rain;

    private Transform sliderPrefab;

    private int radioValue;

    public GameObject leftContent;
    public GameObject rightContent;

	void Start ()
    {
        
        DebugUIBuilder.instance.AddLabel("Future Timeline");
        sliderPrefab = DebugUIBuilder.instance.AddSlider("Time", 2030, 2100, SliderPressed, true);
        var textElementsInSlider = sliderPrefab.GetComponentsInChildren<Text>();
        Assert.AreEqual(textElementsInSlider.Length, 2, "Slider prefab format requires 2 text components (label + value)");
        sliderText = textElementsInSlider[1];
        Assert.IsNotNull(sliderText, "No text component on slider prefab");
        sliderText.text = sliderPrefab.GetComponentInChildren<Slider>().value.ToString();
        DebugUIBuilder.instance.AddDivider();
        DebugUIBuilder.instance.AddLabel("SSP Climate Scenarios");
        DebugUIBuilder.instance.AddRadio("Low Emissions", "group", delegate(Toggle t) { RadioPressed("Scenario A", "group", t); }) ;
        DebugUIBuilder.instance.AddRadio("Medium Emissions", "group", delegate(Toggle t) { RadioPressed("Scenario B", "group", t); }) ;
        DebugUIBuilder.instance.AddRadio("High Emissions", "group", delegate(Toggle t) { RadioPressed("Scenario C", "group", t); }) ;
        DebugUIBuilder.instance.AddButton("Visualize", OnButtonPress);
        DebugUIBuilder.instance.AddLabel("Welcome!", 1);
        DebugUIBuilder.instance.AddDivider(1);
        DebugUIBuilder.instance.AddTextField("Select a year and scenario to model climate predictions.", 1);
        DebugUIBuilder.instance.AddTextField("Press 'B' on the right controller to open and close the menu.", 1);
        DebugUIBuilder.instance.AddLabel("Acknowledgments", 2);
        DebugUIBuilder.instance.AddDivider(2);
        DebugUIBuilder.instance.AddTextField("All data was sourced from IPCC projections from the World Bank.", 2);
        // DebugUIBuilder.instance.AddImage(2);
        DebugUIBuilder.instance.AddTextField("Learn more at WorldBank.org", 2);
        DebugUIBuilder.instance.Show();
        inMenu = true;
	}

    public void RadioPressed(string radioLabel, string group, Toggle t)
    {
        if (t.isOn) {
            if (radioLabel == "Scenario A") {
                radioValue = 0;
            }
            else if (radioLabel == "Scenario B") {
                radioValue = 1;
            }
            else if (radioLabel == "Scenario C") {
                radioValue = 2;
            }
        }
    }

    public void SliderPressed(float f)
    {
        sliderText.text = f.ToString();
    }

    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Start))
        {
            if (inMenu){
                DebugUIBuilder.instance.Hide();
                //leftContent.SetActive(false);
                //Content.SetActive(false);
            } 
            else {
                DebugUIBuilder.instance.Show();
                leftContent.SetActive(false);
                rightContent.SetActive(false);
            }
            inMenu = !inMenu;
        }
    }

    void OnButtonPress()
    {
        int sliderValue = (int) sliderPrefab.GetComponentInChildren<Slider>().value;
        if (WeatherController.makingWeather) {
            StopCoroutine(WeatherController.MakeWeather());
        }
        if (Rain.activeSelf) {
            Rain.SetActive(false);
        }
        Request.StartSimulation(sliderValue, radioValue); 
        Debug.Log("Button pressed with "+sliderValue+" and "+radioValue);
    }
}
