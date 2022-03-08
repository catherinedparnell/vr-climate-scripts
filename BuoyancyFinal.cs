using UnityEngine;
using System.Collections;

public class Buoyancy1 : MonoBehaviour {
    public GameObject sea;
    private float CurrentUpwardForce; // 9.81 is the opposite of the default gravity, which is 9.81. If we want the boat not to behave like a submarine the upward force has to be higher than the gravity in order to push the boat to the surface
    private bool goingUp;
    public float normalUpForce = 20;
    private float goingUpForce = 35;
    private bool isInWater = false;
    
    void Start()
    {
        goingUp = sea.GetComponent<SeaLevelRise>().goingUp;
        CurrentUpwardForce = normalUpForce;
    }
    void Update()
    {
        goingUp = sea.GetComponent<SeaLevelRise>().goingUp;
        if(goingUp)
        {
            CurrentUpwardForce = goingUpForce;
        }
        else if(!goingUp)
        {
            CurrentUpwardForce = normalUpForce;
        }
    }
    void OnTriggerEnter(Collider collidier) {
        isInWater = true;
        GetComponent<Rigidbody>().drag = 4f;
    }

    void OnTriggerExit(Collider collidier) {
        isInWater = false;
        GetComponent<Rigidbody>().drag = 0.01f;
    }
    float RandomizeUpwardForce(float upForce){
        float lower= upForce-1;
        float upper = upForce+1;
        return Random.Range(lower, upper);
    }
    void FixedUpdate() {
        if(isInWater) {
            // apply upward force
            float upForce = RandomizeUpwardForce(CurrentUpwardForce);
            print(upForce); 
            Vector3 force = transform.up * upForce;
            this.GetComponent<Rigidbody>().AddRelativeForce(force, ForceMode.Acceleration);
            Debug.Log("Upward force: " + force+" @"+Time.time);
        }
    }
}
