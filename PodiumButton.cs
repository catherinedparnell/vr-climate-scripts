using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumButton : MonoBehaviour
{
    public GameObject elevator;
    private Vector3 startElePos;
    private Vector3 endElePos;
    public float riseAmount;
    public float riseSpeed;
    private float startTime;
    private bool go;

    private Vector3 startPos;
    private Vector3 endPos;
    public float movement;
    private AudioSource audioSource;

    //public int buttonId;
    //private int index;
    //private GameObject bigObject;
    //public GameObject[] objectList;
    //public Transform spawnPoint;
    //public Material objectMat;
    //public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        //canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 200.0f))
            {
                if (hit.transform.gameObject == this.transform.gameObject)
                {
                    Debug.Log("You clicked the " + hit.transform.name);
                    StartCoroutine(ButtonPush(0.5f));
                    ActivatePlatform();
                        /*
                        ChangeObject();
                    else if (buttonId == 2)
                        ToggleCanvas();  */
                }
            }
        }
    }

    void ActivatePlatform()
    {
        go = true;
        //if platform position down, get it up
        if (elevator.transform.position == startElePos)
        {
            // Distance moved = time * speed
            float distCovered = (Time.time - startTime) * riseSpeed;

            // Fraction of journey completed = current distance divided by total distance
            float fracJourney = distCovered / riseAmount;

            // Set our position as a fraction of the distance between markers
            transform.position = Vector3.Lerp(startElePos, endElePos, fracJourney);

            // if we're done, stop
            if (fracJourney > 0.99)
                go = false;
        }
        //if platform position up, get it down
        else if (elevator.transform.position == endElePos)
        {
            // Distance moved = time * speed
            float distCovered = (Time.time - startTime) * riseSpeed;

            // Fraction of journey completed = current distance divided by total distance
            float fracJourney = distCovered / riseAmount;

            // Set our position as a fraction of the distance between markers
            transform.position = Vector3.Lerp(endElePos, startElePos, fracJourney);

            // if we're done, stop
            if (fracJourney > 0.99)
                go = false;
        }

    }

    IEnumerator ButtonPush(float seconds)
    {
        startPos = this.transform.position;
        endPos = new Vector3(startPos.x, startPos.y - movement, startPos.z);

        audioSource.Play();
        this.transform.position = endPos;
        yield return new WaitForSeconds(seconds);
        this.transform.position = startPos;
    }
}