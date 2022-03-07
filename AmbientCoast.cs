using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientCoast : MonoBehaviour
{
    private AudioSource sound;

    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    public void PlayCoastSound()
    {
        sound.Play();
    }

    public void PauseCoastSound()
    {
        sound.Pause();
    }
}