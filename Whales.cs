using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whales : MonoBehaviour
{
    public GameObject WhalePrefab;
    public Request Request;

    public int bounds; // how far the whale should spawn from
    private Vector3 center;

    private int startTime;

    async void Start()
    {
        center = this.transform.localPosition;
        StartCoroutine(SpawnWhales());
    }

    void Update() {
        if (Request.chosenScenario == 0) {
            startTime = 20;
        } else if (Request.chosenScenario == 1) {
            startTime = 30;
        }
        else {
            startTime = 50;
        }
    }

    private IEnumerator SpawnWhales()
    {
        int sec = Random.Range(startTime, startTime + 10);
        yield return new WaitForSeconds(sec);
        Vector3 pos = CreatePositionVector();
        GameObject whale = Instantiate(WhalePrefab, transform.localPosition + pos, transform.localRotation); // find a location
        whale.transform.parent = transform;
        StartCoroutine(SpawnWhales());
    }

    private Vector3 CreatePositionVector() {
        Vector3 pos = new Vector3(CreatePosition(center.x, 20), center.y, CreatePosition(center.z, bounds));
        return pos;
    }

    private float CreatePosition(float pos, int bounds) {
        return Random.Range(pos - bounds, pos + bounds);
    }
}
