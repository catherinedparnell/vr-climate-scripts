using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChangeManager : MonoBehaviour
{
    private Color colorClear = Color.grey;
    private Color32 colorRain = new Color32(58, 58, 58, 255);
    [SerializeField] [Range(0f, 5f)] float duration;
    private float lerp = 0f;
    private float start;
    public bool raining = false;
    private bool done = false;
    public GameObject rain;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RainCheck();
        if(raining & !done)
        {
            RainSkyboxLerp();
        }
        else if(!raining & !done)
        {
            ClearSkyboxLerp();
        }
    }
    public void RainCheck(){
        if(rain.activeSelf & !raining)
        {
            raining = true;
            done = false;
            start = Time.time;
            print("starting lerp to raining skybox");
        }
        else if (!rain.activeSelf & raining)
        {
            raining = false;
            done = false;
            start = Time.time;
            print("starting lerp to clear skybox");
        }
    }
    public void RainSkyboxLerp()
    {
        if (lerp < 0.99)
        {
            print("lerping: "+lerp);
            lerp += Time.deltaTime/duration;
            RenderSettings.skybox.SetColor("_Tint", Color.Lerp(colorClear, colorRain, lerp));
            RenderSettings.ambientLight = Color.Lerp(colorClear, colorRain, lerp);
        }
        else{
            done = true;
            lerp = 0;
        }
    }
    public void ClearSkyboxLerp()
    {
        if (lerp < 0.99)
        {
            lerp += Time.deltaTime/duration;
            RenderSettings.skybox.SetColor("_Tint", Color.Lerp(colorRain, colorClear, lerp));
            RenderSettings.ambientLight = Color.Lerp(colorRain, colorClear, lerp);
        }
        else{
            done = true;
            lerp = 0;
        }
    }

    private void OnApplicationQuit() {
        RenderSettings.skybox.SetColor("_Tint", colorClear);
    }
}
