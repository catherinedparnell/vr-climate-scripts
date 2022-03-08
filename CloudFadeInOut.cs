using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudFadeInOut : MonoBehaviour
{
    public GameObject lowClouds;
    public GameObject highClouds;
    private Color32 cloudColor;
    private float cloudColorAlpha;
    private float upperAlphaBound = 130;
    private float lowerAlphaBound = 5;

    public bool fadeCloudsIn = false;
    public bool fadeCloudsOut = false;
    // Start is called before the first frame update
    void Start()
    {
        cloudColor = lowClouds.GetComponent<Renderer>().material.GetColor("_CloudColor");
        cloudColorAlpha = cloudColor.a;
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.Get(OVRInput.Button.One)){
            SetFadeIn();
        }
        if(OVRInput.Get(OVRInput.Button.Two)){
            SetFadeOut();
        }
        if(fadeCloudsIn & !fadeCloudsOut){
            FadeIn();
        }
        else if(!fadeCloudsIn & fadeCloudsOut){
            FadeOut();
        }    
    }
    public void SetFadeIn()
    {
        fadeCloudsOut = false;
        fadeCloudsIn = true;
    }
    public void SetFadeOut()
    {
        fadeCloudsOut = true;
        fadeCloudsIn = false;
    }
    public void FadeIn()
    {
        if(cloudColorAlpha < upperAlphaBound)
        cloudColorAlpha += 50f * Time.deltaTime;
        Color32 newColor = new Color32(cloudColor.r, cloudColor.g, cloudColor.b, (byte) cloudColorAlpha);
        lowClouds.GetComponent<Renderer>().material.SetColor("_CloudColor", newColor);
        highClouds.GetComponent<Renderer>().material.SetColor("_CloudColor", newColor);
    }
    public void FadeOut()
    {
        if(cloudColorAlpha > lowerAlphaBound)
        cloudColorAlpha -= 50f * Time.deltaTime;
        Color32 newColor = new Color32(cloudColor.r, cloudColor.g, cloudColor.b, (byte) cloudColorAlpha);
        lowClouds.GetComponent<Renderer>().material.SetColor("_CloudColor", newColor);
        highClouds.GetComponent<Renderer>().material.SetColor("_CloudColor", newColor);
    }
}
