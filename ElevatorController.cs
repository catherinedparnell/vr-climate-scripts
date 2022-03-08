using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    private Vector3 permaStart;
    private Vector3 permaEnd;

    private Vector3 startPos;
    private Vector3 endPos;

    public float riseAmount;
    public float riseSpeed;

    private float startTime;
    private bool go;

    // Start is called before the first frame update
    void Start()
    {
        permaStart = this.transform.position;
        permaEnd = new Vector3(permaStart.x, permaStart.y + riseAmount, permaStart.z);
        startPos = permaStart;
        endPos = permaEnd;
        
    }

    public void CheckRiseLower()
    {
        if(Mathf.Abs(this.transform.position.y - permaEnd.y) < 1)
        {
            Lower();
        }
        else if(Mathf.Abs(this.transform.position.y - permaStart.y) < 1)
        {
            Rise();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.Get(OVRInput.Button.One))
        {
            CheckRiseLower();
        }
        
        if (go)
        {
            float distCovered = (Time.time - startTime) * riseSpeed;

            float fracJourney = distCovered / riseAmount;

            this.transform.position = Vector3.Lerp(startPos, endPos, fracJourney);
            
            if (fracJourney > 0.99)
                go = false;
        }
        
    }
    private void Rise()
    {
        startPos = this.transform.position;
        endPos = permaEnd;
        startTime = Time.time;
        go = true;
    }

    private void Lower()
    {
        startTime = Time.time;
        startPos = this.transform.position;
        endPos = permaStart;
        go = true;
    }
}
