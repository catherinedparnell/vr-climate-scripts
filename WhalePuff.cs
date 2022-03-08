using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhalePuff : MonoBehaviour
{
    private ParticleSystem puff;

    void Start()
    {
       puff = GetComponent<ParticleSystem>();
       StartCoroutine(WhaleDoesThing());
    }

    private IEnumerator WhaleDoesThing() {
        yield return new WaitForSeconds(.5f);
        puff.Play();
        yield return new WaitForSeconds(2.25f);
        puff.Stop();
    }
}
