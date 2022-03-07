using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunderstorm : MonoBehaviour
{
    private AudioSource sound;

    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    public void StartThunderstormSound()
    {
        sound.Play();
    }

    public void StopThunderstormSound()
    {
        sound.Stop();
    }
}
