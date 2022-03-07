using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Downpour : MonoBehaviour
{
    private AudioSource sound;

    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    public void StartDownpourSound()
    {
        sound.Play();
    }

    public void StopDownpourSound()
    {
        sound.Stop();
    }
}
