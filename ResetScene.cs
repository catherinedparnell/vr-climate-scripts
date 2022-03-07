using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
    public void Update() {
        if (OVRInput.GetDown(OVRInput.Button.One) && OVRInput.GetDown(OVRInput.Button.Two))
        {
		    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex); // loads current scene
		    Time.timeScale = 1f;
        }
	}
}
