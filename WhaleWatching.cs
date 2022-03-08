using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVRTouchSample;
using System;


public class WhaleWatching : MonoBehaviour
{
    OVRVignette vignetteEffect;
    private float vignetteFOV = 60;

    int buttonPressCount = 0;

    public GameObject binocularShader;
    public GameObject otherShaderToIgnore;

    private void Start()
    {
        binocularShader.SetActive(false);
        otherShaderToIgnore.SetActive(false);
    }

    void Update()
    {
        if (TriggerBinocular.isCloseEnough()) // user can check out the binoculars
        {
            //if (buttonPressCount == 0)
            if (OVRInput.GetDown(OVRInput.Button.One) && buttonPressCount == 0)
            {
                binocularShader.SetActive(true); // render texture plane gets activated in front of the camera
                //Camera.main.cullingMask = 3; // only renders the binocular plane to improve performance

                // adds effects to make binoculars view:
                vignetteEffect = GetComponent<OVRVignette>();
                vignetteEffect.enabled = true;
                vignetteEffect.VignetteFieldOfView = vignetteFOV;
                buttonPressCount = 1;
            }

            else if (OVRInput.GetDown(OVRInput.Button.One) && buttonPressCount == 1) // binocular mode disabled
            {
                vignetteEffect.enabled = false; // no vignette
                binocularShader.SetActive(false);
                buttonPressCount = 0;
            }
        }
    }
}