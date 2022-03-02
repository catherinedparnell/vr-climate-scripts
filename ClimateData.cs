using UnityEngine;

[System.Serializable]
public class ClimateData
{
    // keeping these public for now so we can see as exposed in request for debugging purposes
    public float temp;
    public float seaLevel;
    public float precipitation;
    //private int year;

    public float Temperature
    { get { return temp; } }

    public float SeaLevel
    { get { return seaLevel; } }

    public float Precipitation
    { get { return precipitation; } }

    //public int Year
   // { get { return year; } }

    public ClimateData(float precip, float sea_level, float temperature)
    {
        precipitation = precip;
        seaLevel = sea_level;
        temp = temperature;
        // year = date;
    }

    override public string ToString()
    {
        return "rain amount: "+ precipitation +"; sea level: "+ seaLevel+"; temperature: "+temp;
    }
}