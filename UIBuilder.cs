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

    private Transform sliderPrefab;

    private int radioValue;

	void Start ()
    {
        
        DebugUIBuilder.instance.AddLabel("Modeling Parameters");
        sliderPrefab = DebugUIBuilder.instance.AddSlider("Time", 2030, 2100, SliderPressed, true);
        var textElementsInSlider = sliderPrefab.GetComponentsInChildren<Text>();
        Assert.AreEqual(textElementsInSlider.Length, 2, "Slider prefab format requires 2 text components (label + value)");
        sliderText = textElementsInSlider[1];
        Assert.IsNotNull(sliderText, "No text component on slider prefab");
        sliderText.text = sliderPrefab.GetComponentInChildren<Slider>().value.ToString();
        DebugUIBuilder.instance.AddDivider();
        DebugUIBuilder.instance.AddRadio("Scenario A", "group", delegate(Toggle t) { RadioPressed("Scenario A", "group", t); }) ;
        DebugUIBuilder.instance.AddRadio("Scenario B", "group", delegate(Toggle t) { RadioPressed("Scenario B", "group", t); }) ;
        DebugUIBuilder.instance.AddRadio("Scenario C", "group", delegate(Toggle t) { RadioPressed("Scenario C", "group", t); }) ;
        DebugUIBuilder.instance.AddButton("Visualize", OnButtonPress);
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
        Debug.Log("Radio value changed: "+radioLabel+", from group "+group+". New value: "+t.isOn);
    }

    public void SliderPressed(float f)
    {
        Debug.Log("Slider: " + f);
        sliderText.text = f.ToString();
    }

    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Start))
        {
            if (inMenu) DebugUIBuilder.instance.Hide();
            else DebugUIBuilder.instance.Show();
            inMenu = !inMenu;
        }
    }

    void OnButtonPress()
    {
        int sliderValue = (int) sliderPrefab.GetComponentInChildren<Slider>().value;
        Request.StartSimulation(sliderValue, radioValue); 
        Debug.Log("Button pressed");
    }
}
